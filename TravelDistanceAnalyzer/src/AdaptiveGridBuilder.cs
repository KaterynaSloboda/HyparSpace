﻿using Elements;
using Elements.Geometry;
using Elements.Spatial.AdaptiveGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GridVertex = Elements.Spatial.AdaptiveGrid.Vertex;
using DotLiquid.Tags;

namespace TravelDistanceAnalyzer
{
    internal class AdaptiveGridBuilder
    {
        private const double RoomToWallTolerance = 1e-3;
        private const double MinExitWidth = 0.5;

        private AdaptiveGrid _grid;

        private Dictionary<SpaceBoundary, List<GridVertex>> _roomExits;
        private List<(CirculationSegment Segment, Polyline Centerline)> _centerlines = new();

        public AdaptiveGrid Build(IEnumerable<CirculationSegment> corridors,
                                  IEnumerable<SpaceBoundary> rooms,
                                  List<WallCandidate>? walls = null,
                                  List<Door>? doors = null)
        {
            foreach (var item in corridors)
            {
                var centerLine = CorridorCenterLine(item);
                if (centerLine != null && centerLine.Vertices.Count > 1)
                {
                    _centerlines.Add((item, centerLine));
                }
            }

            _grid = new AdaptiveGrid(new Transform());

            foreach (var line in _centerlines)
            {
                _grid.AddVertices(line.Centerline.Vertices,
                    AdaptiveGrid.VerticesInsertionMethod.ConnectAndSelfIntersect);
            }

            Intersect(_centerlines);
            Extend(_centerlines);

            _roomExits = new Dictionary<SpaceBoundary, List<GridVertex>>();
            foreach (var room in rooms)
            {
                var exits = AddRoom(room, _centerlines, walls, doors);
                _roomExits.Add(room, exits);
            }

            return _grid;
        }

        public ulong AddEndPoint(Vector3 exit, double snapDistance)
        {
            List<GridVertex> verticesToConnect = new();
            foreach (var room in _roomExits)
            {
                var p = room.Key.Boundary.Perimeter.TransformedPolygon(room.Key.Transform);
                if (p.Covers(exit))
                {
                    verticesToConnect.AddRange(room.Value);
                    break;
                }
            }

            if (!verticesToConnect.Any())
            {
                ClosestEdgeOnElevation(exit, out var closest);
                if (exit.DistanceTo(closest) < snapDistance)
                {
                    exit = closest;
                }
                var vertex = LinkToCenterlines(new Transform(exit), snapDistance);
                if (vertex != null)
                {
                    AdditionalConnections(vertex);
                    return vertex.Id;
                }
                return 0u;
            }

            GridVertex? exitVertex = null;
            foreach (var item in verticesToConnect)
            {
                if (item.Point.IsAlmostEqualTo(exit, _grid.Tolerance))
                {
                    exitVertex = item;
                }
                else
                {
                    exitVertex = _grid.AddVertex(exit, new ConnectVertexStrategy(item), cut: false);
                }
            }
            return exitVertex == null ? 0u : exitVertex.Id;
        }

        private void AdditionalConnections(GridVertex exit)
        {
            if (exit.Edges.Count > 2)
            {
                return;
            }

            var mainConnection = _grid.GetVertex(exit.Edges.First().OtherVertexId(exit.Id));
            var basePoint = exit.Point;
            var maxDist = basePoint.DistanceTo(mainConnection.Point) * 2;
            var additionalConnections = new (double Distance, Vector3 Point)[4]
            {
                (double.MaxValue, Vector3.Origin),
                (double.MaxValue, Vector3.Origin),
                (double.MaxValue, Vector3.Origin),
                (double.MaxValue, Vector3.Origin)
            };

            var xDir = (mainConnection.Point - basePoint).Unitized();
            var yDir = xDir.Cross(Vector3.ZAxis);

            foreach (var edge in _grid.GetEdges())
            {
                if (edge.StartId == mainConnection.Id || edge.EndId == mainConnection.Id)
                {
                    continue;
                }

                var start = _grid.GetVertex(edge.StartId);
                var end = _grid.GetVertex(edge.EndId);

                var line = new Line(start.Point, end.Point);
                var closest = exit.Point.ClosestPointOn(line);
                var delta = closest - basePoint;
                var length = delta.Length();
                if (length > maxDist)
                {
                    continue;
                }

                var directionIndex = -1;
                var dot = delta.Unitized().Dot(xDir);
                if (dot.ApproximatelyEquals(1, 0.01))
                {
                    directionIndex = 0;
                }
                else if (dot.ApproximatelyEquals(-1, 0.01))
                {
                    directionIndex = 1;
                }

                dot = delta.Unitized().Dot(yDir);
                if (dot.ApproximatelyEquals(1, 0.01))
                {
                    directionIndex = 2;
                }
                else if (dot.ApproximatelyEquals(-1, 0.01))
                {
                    directionIndex = 3;
                }

                if (directionIndex >= 0)
                {
                    var connection = additionalConnections[directionIndex];
                    if (length < connection.Distance)
                    {
                        additionalConnections[directionIndex] = (length, closest);
                    }
                }
            }

            foreach (var connection in additionalConnections)
            {
                if (connection.Distance != double.MaxValue)
                {
                    _grid.AddEdge(exit.Point, connection.Point);
                }
            }
        }


        public AdaptiveGrid Grid
        {
            get { return _grid; }
        }

        public Dictionary<SpaceBoundary, List<GridVertex>> RoomExits
        {
            get { return _roomExits; }
        }

        private Edge ClosestEdgeOnElevation(Vector3 location, out Vector3 point)
        {
            double lowestDist = double.MaxValue;
            Edge closestEdge = null;
            point = Vector3.Origin;
            foreach (var e in Grid.GetEdges())
            {
                var start = Grid.GetVertex(e.StartId);
                var end = Grid.GetVertex(e.EndId);
                if (Math.Abs(start.Point.Z - location.Z) > 0.5 ||
                    Math.Abs(end.Point.Z - location.Z) > 0.5)
                {
                    continue;
                }

                double dist = location.DistanceTo((start.Point, end.Point), out var closest);
                if (dist < lowestDist)
                {
                    lowestDist = dist;
                    closestEdge = e;
                    point = closest;
                }
            }
            return closestEdge;
        }

        private Polyline CorridorCenterLine(CirculationSegment corridor)
        {
            double offsetDistance = corridor.Geometry.GetOffset();
            var corridorPolyline = corridor.Geometry.Polyline;
            if (!offsetDistance.ApproximatelyEquals(0))
            {
                corridorPolyline = corridorPolyline.OffsetOpen(offsetDistance);
            }
            corridorPolyline = corridorPolyline.TransformedPolyline(corridor.Transform);
            return corridorPolyline;
        }


        /// <summary>
        /// Create connection edges between corridors.
        /// Corridors itself are represented as middle lines without width.
        /// For each line points are found with other corridors and itself that are closer that their combined width.
        /// </summary>
        /// <param name="centerlines">Corridor segments with precalculated center lines.</param>
        /// <param name="grid">AdaptiveGrid to insert new vertices and edge into.</param>
        private void Intersect(List<(CirculationSegment Segment, Polyline Centerline)> centerlines)
        {
            foreach (var item in centerlines)
            {
                var leftVertices = item.Centerline.Vertices;
                foreach (var candidate in centerlines)
                {
                    var rightVertices = candidate.Centerline.Vertices;
                    var maxDistance = item.Segment.Geometry.GetWidth() + candidate.Segment.Geometry.GetWidth();
                    for (int i = 0; i < leftVertices.Count - 1; i++)
                    {
                        Vector3 closestLeftItem = Vector3.Origin, closestRightItem = Vector3.Origin;
                        int closestLeftProximity = -1, closestRightProximity = -1;
                        double closestDistance = double.PositiveInfinity;
                        Line leftLine = new Line(leftVertices[i], leftVertices[i + 1]);

                        Action<Line, Vector3, Vector3, int, int, bool> check =
                            (Line line, Vector3 point, Vector3 direction, int leftIndex, int rightIndex, bool left) =>
                            {
                                if (CanConnectDirectional(point, direction, line, Math.Min(maxDistance, closestDistance),
                                                          out var closest, out var d))
                                {
                                    closestDistance = d;
                                    (closestLeftItem, closestRightItem) = left ? (closest, point) : (point, closest);
                                    closestLeftProximity = leftIndex;
                                    closestRightProximity = rightIndex;
                                }
                            };

                        for (int j = 0; j < rightVertices.Count - 1; j++)
                        {
                            if (item == candidate && Math.Abs(i - j) < 2)
                            {
                                continue;
                            }

                            Line rightLine = new Line(rightVertices[j], rightVertices[j + 1]);
                            if (!leftLine.Intersects(rightLine, out var intersection))
                            {
                                check(rightLine, leftLine.Start, leftLine.Direction(), i, j, false);
                                check(rightLine, leftLine.End, leftLine.Direction(), i, j, false);
                                check(leftLine, rightLine.Start, rightLine.Direction(), i, j, true);
                                check(leftLine, rightLine.End, rightLine.Direction(), i, j, true);
                            }
                            else
                            {
                                closestLeftItem = intersection;
                                closestRightItem = intersection;
                                closestLeftProximity = i;
                                closestRightProximity = j;
                            }
                        }

                        if (closestLeftProximity == -1 || closestRightProximity == -1)
                        {
                            continue;
                        }

                        bool leftExist = _grid.TryGetVertexIndex(closestLeftItem, out var leftId);
                        bool rightExist = _grid.TryGetVertexIndex(closestRightItem, out var rightId);
                        if (leftExist && rightExist)
                        {
                            if (leftId != rightId)
                            {
                                _grid.AddEdge(leftId, rightId);
                            }
                        }
                        else
                        {
                            GridVertex? leftVertex = null;
                            if (!leftExist)
                            {
                                _grid.TryGetVertexIndex(leftVertices[closestLeftProximity], out var leftCon);
                                _grid.TryGetVertexIndex(leftVertices[closestLeftProximity + 1], out var rightCon);
                                var segment = new Line(leftVertices[closestLeftProximity], leftVertices[closestLeftProximity + 1]);
                                var vertex = _grid.GetVertex(leftCon);
                                var edge = FindOnCollinearEdges(vertex, rightCon, segment.Direction(), closestLeftItem);
                                if (edge != null)
                                {
                                    leftVertex = _grid.CutEdge(edge, closestLeftItem);
                                }
                            }
                            else
                            {
                                leftVertex = _grid.GetVertex(leftId);
                            }

                            if (!rightExist)
                            {
                                _grid.TryGetVertexIndex(rightVertices[closestRightProximity], out var leftCon);
                                _grid.TryGetVertexIndex(rightVertices[closestRightProximity + 1], out var rightCon);
                                var vertex = _grid.GetVertex(leftCon);
                                var connections = new List<GridVertex>();
                                if (!closestLeftItem.IsAlmostEqualTo(closestRightItem, _grid.Tolerance))
                                {
                                    connections.Add(leftVertex);
                                }

                                var segment = new Line(rightVertices[closestRightProximity], rightVertices[closestRightProximity + 1]);
                                var edge = FindOnCollinearEdges(vertex, rightCon, segment.Direction(), closestRightItem);
                                if (edge != null)
                                {
                                    var start = Grid.GetVertex(edge.StartId);
                                    var end = Grid.GetVertex(edge.EndId);
                                    if (!closestRightItem.IsAlmostEqualTo(start.Point, _grid.Tolerance) &&
                                        !closestRightItem.IsAlmostEqualTo(end.Point, _grid.Tolerance))
                                    {
                                        connections.Add(start);
                                        connections.Add(end);
                                        _grid.AddVertex(closestRightItem,
                                                        new ConnectVertexStrategy(connections.ToArray()),
                                                        cut: false);
                                        _grid.RemoveEdge(edge);
                                    }
                                }
                            }
                            else if (leftVertex.Id != rightId)
                            {
                                _grid.AddEdge(leftVertex.Id, rightId);
                            }
                        }
                    }
                }
            }
        }

        private Edge? FindOnCollinearEdges(GridVertex start, ulong endId, Vector3 direction, Vector3 destination)
        {
            while (start.Id != endId)
            {
                GridVertex otherVertex = null;
                Edge edge = null;
                foreach (var e in start.Edges)
                {
                    otherVertex = _grid.GetVertex(e.OtherVertexId(start.Id));
                    var edgeDirection = (otherVertex.Point - start.Point).Unitized();
                    if (edgeDirection.Dot(direction).ApproximatelyEquals(1))
                    {
                        edge = e;
                        break;
                    }
                }

                if (edge == null)
                {
                    throw new Exception("End edge is not reached");
                }

                var edgeLine = new Line(start.Point, otherVertex.Point);
                if (edgeLine.PointOnLine(destination, true))
                {
                    return edge;
                }

                start = otherVertex;
            }

            return null;
        }

        private void Extend(List<(CirculationSegment Segment, Polyline Centerline)> centerlines)
        {
            foreach (var item in centerlines)
            {
                foreach (var candidate in centerlines)
                {
                    if (item == candidate)
                    {
                        continue;
                    }

                    foreach (var segment in item.Centerline.Segments())
                    {
                        ExtendToCorridor(segment, candidate.Segment);
                    }
                }
            }
        }

        private void ExtendToCorridor(Line l, CirculationSegment segment)
        {
            foreach (var polygon in segment.Geometry.GetPolygons())
            {
                var maxDistance = polygon.offsetPolygon.Segments().Max(s => s.Length());
                var transformedPolygon = polygon.offsetPolygon.TransformedPolygon(segment.Transform);
                var trimLine = new Line(l.Start - l.Direction() * maxDistance,
                                        l.End + l.Direction() * maxDistance);
                var inside = trimLine.Trim(transformedPolygon, out _);
                foreach (var line in inside)
                {
                    if (l.PointOnLine(line.Start, true) || l.PointOnLine(line.End, true) ||
                        line.PointOnLine(l.Start, true) || line.PointOnLine(l.End, true))
                    {
                         Grid.AddEdge(line.Start, line.End);
                    }
                }
            }
        }

        /// <summary>
        /// Add SpaceBoundary, representing a room, to the grid.
        /// There are no defined exits. in the room. Every segment middle point is considered.
        /// This is very simple approaches that ignores voids or obstacles inside room and won't work for complex rooms.
        /// </summary>
        /// <param name="room">Room geometry.</param>
        /// <param name="centerlines">Corridor segments with precalculated center lines.</param>
        /// <param name="grid">AdaptiveGrid to insert new vertices and edge into.</param>
        /// <returns></returns>
        private List<GridVertex> AddRoom(
            SpaceBoundary room,
            List<(CirculationSegment Segment, Polyline Centerline)> centerlines,
            List<WallCandidate>? walls,
            List<Door>? doors)
        {
            var roomExits = new List<GridVertex>();
            var perimeter = room.Boundary.Perimeter.CollinearPointsRemoved().TransformedPolygon(room.Transform);
            foreach (var roomEdge in perimeter.Segments())
            {
                foreach(var exit in FindRoomExits(roomEdge, centerlines, walls, doors))
                {
                    roomExits.Add(exit);
                }
            }
            return roomExits;
        }

        /// <summary>
        /// Find if edge middle point is close enough to any corridor to be considered connected.
        /// If point is closer then half corridor width then it's connected to closest point by new edge.
        /// </summary>
        /// <param name="roomEdge">Line representing room wall.</param>
        /// <param name="centerlines">Corridor segments with precalculated center lines.</param>
        /// <param name="grid">AdaptiveGrid to insert new vertices and edge into.</param>
        /// <returns>New Vertex on room edge midpoint.</returns>
        private List<GridVertex> FindRoomExits(
            Line roomEdge,
            List<(CirculationSegment Segment, Polyline Centerline)> centerlines,
            List<WallCandidate>? walls,
            List<Door>? doors)
        {
            var doorsOnWall = doors?.Where(d => roomEdge.PointOnLine(d.Transform.Origin, false, RoomToWallTolerance));
            var openSections = GetOpenPassages(roomEdge, walls);

            // There are no doors in the workflow and this segment covered by walls.
            // Take middle point to give user at least some exits.
            if (doors == null && !openSections.Any())
            {
                openSections.Add(roomEdge);
            }

            List<Transform> exitLocations = new List<Transform>();
            if (doorsOnWall != null && doorsOnWall.Any())
            {
                exitLocations.AddRange(doorsOnWall.Select(d => d.Transform));
            }

            foreach (var item in openSections)
            {
                exitLocations.Add(new Transform(item.Mid(), item.Direction(), Vector3.ZAxis));
            }

            List<GridVertex> exitVertices = new();
            foreach (var t in exitLocations)
            {
                var v = LinkToCenterlines(t, _grid.Tolerance);
                if (v != null)
                {
                    exitVertices.Add(v);
                }
            }
            return exitVertices;
        }

        public List<Line> GetOpenPassages(Line roomSide, List<WallCandidate>? walls)
        {
            if (walls == null)
            {
                return new List<Line>() { roomSide };
            }

            List<Domain1d> coveredRanges = new();
            foreach (var wall in walls)
            {
                if (roomSide.TryGetOverlap(wall.Line, RoomToWallTolerance, out Line overlap))
                {
                    var d0 = roomSide.GetParameterAt(overlap.Start);
                    var d1 = roomSide.GetParameterAt(overlap.End);
                    coveredRanges.Add(d0 < d1 ? new Domain1d(d0, d1) : new Domain1d(d1, d0));
                }
            }

            var domains = FindUncoveredParametricRanges(roomSide.Domain, coveredRanges);
            List<Line> openExits = new();
            foreach (var domain in domains)
            {
                var a = roomSide.PointAt(domain.Min);
                var b = roomSide.PointAt(domain.Max);
                if (a.DistanceTo(b) > MinExitWidth)
                {
                    openExits.Add(new Line(a, b));
                }
            }
            return openExits;
        }

        private List<Domain1d> FindUncoveredParametricRanges(Domain1d lineDomain, List<Domain1d> wallDomains)
        {
            List<Domain1d> uncoveredRanges = new ();

            wallDomains.Sort((a, b) => a.Min.CompareTo(b.Min));

            double current = lineDomain.Min;
            foreach (var domain in wallDomains)
            {
                if (current < domain.Min)
                {
                    uncoveredRanges.Add(new Domain1d(current, domain.Min));
                }

                current = Math.Max(current, domain.Max);
            }

            if (current < lineDomain.Max)
            {
                uncoveredRanges.Add(new Domain1d(current, lineDomain.Max));
            }

            return uncoveredRanges;
        }

        private GridVertex? LinkToCenterlines(Transform location, 
                                             double snapDistance)
        {
            foreach (var line in _centerlines)
            {
                for (int i = 0; i < line.Centerline.Vertices.Count - 1; i++)
                {
                    var segment = new Line(line.Centerline.Vertices[i], line.Centerline.Vertices[i + 1]);
                    var distance = location.Origin.DistanceTo(segment, out var closest);
                    if (distance > line.Segment.Geometry.GetWidth() / 2 + 0.10)
                    {
                        continue;
                    }

                    GridVertex exitVertex = null;
                    _grid.TryGetVertexIndex(segment.Start, out var id);
                    var vertex = _grid.GetVertex(id);

                    //We know corridor line but it can already be split into several edges.
                    //Need to find exact edge to insert new vertex into.
                    //First vertex corresponding start of the segment is found.
                    //Then, edges that do in the same direction as segment is traversed
                    //until target edge is found or end vertex is reached.
                    //This is much faster than traverse every single edge in the grid.
                    if (vertex.Point.IsAlmostEqualTo(closest, _grid.Tolerance))
                    {
                        exitVertex = vertex;
                    }
                    else
                    {
                        _grid.TryGetVertexIndex(segment.End, out var endId);
                        var edge = FindOnCollinearEdges(vertex, endId, segment.Direction(), closest);
                        if (edge != null)
                        {
                            var start = _grid.GetVertex(edge.StartId);
                            var end = _grid.GetVertex(edge.EndId);

                            if (start.Point.IsAlmostEqualTo(closest, _grid.Tolerance))
                            {
                                exitVertex = start;
                            }
                            else if (end.Point.IsAlmostEqualTo(closest, _grid.Tolerance))
                            {
                                exitVertex = end;
                            }
                            else
                            {
                                exitVertex = _grid.AddVertex(closest, new ConnectVertexStrategy(start, end), cut: false);
                                _grid.RemoveEdge(edge);
                            }
                        }
                    }

                    if (exitVertex != null)
                    {
                        if (!exitVertex.Point.IsAlmostEqualTo(location.Origin, snapDistance))
                        {
                            var delta = closest - location.Origin;
                            var dot = delta.Dot(segment.Direction());
                            if (dot.ApproximatelyEquals(0) || dot.ApproximatelyEquals(delta.Length()))
                            {
                                var v = _grid.AddVertex(location.Origin, new ConnectVertexStrategy(exitVertex));
                                ExtendToCorridor(new Line(v.Point, exitVertex.Point), line.Segment);
                                return v;
                            }
                            else
                            {
                                var cornerPoint = Math.Abs(location.XAxis.Dot(segment.Direction())) > 1 / Math.Sqrt(2) ?
                                    closest - dot * segment.Direction() : location.Origin + dot * segment.Direction();

                                var strip = _grid.AddVertices(
                                    new List<Vector3> { location.Origin, cornerPoint, closest },
                                    AdaptiveGrid.VerticesInsertionMethod.ConnectAndCut);
                                ExtendToCorridor(new Line(strip.First().Point, cornerPoint), line.Segment);
                                return strip.First();
                            }
                        }
                        else
                        {
                            return exitVertex;
                        }
                    }
                }
            }
            return null;
        }

        private bool CanConnectDirectional(Vector3 point,
                                           Vector3 direction,
                                           Line segment,
                                           double maxDistance,
                                           out Vector3 closest,
                                           out double dist)
        {
            InfiniteLine a = new InfiniteLine(point, direction);
            if (a.Intersects(segment, out var result))
            {
                closest = result.First();
                dist = closest.DistanceTo(point);
                return dist < maxDistance;
            }

            closest = Vector3.Origin;
            dist = double.MaxValue;
            return false;
        }
    }
}
