﻿using Elements;
using Elements.Geometry;
using Elements.Spatial.AdaptiveGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elements.Geometry;
using GridVertex = Elements.Spatial.AdaptiveGrid.Vertex;

namespace TravelDistanceAnalyzer
{
    internal class AdaptiveGridBuilder
    {
        private const double RoomToWallTolerance = 1e-3;

        private AdaptiveGrid _grid;

        private Dictionary<SpaceBoundary, List<GridVertex>> _roomExits;

        public AdaptiveGrid Build(IEnumerable<CirculationSegment> corridors,
                                  IEnumerable<SpaceBoundary> rooms,
                                  List<WallCandidate>? walls = null,
                                  List<Door>? doors = null)
        {
            var centerlines = new List<(CirculationSegment Segment, Polyline Centerline)>();
            foreach (var item in corridors)
            {
                var centerLine = CorridorCenterLine(item);
                if (centerLine != null && centerLine.Vertices.Count > 1)
                {
                    centerlines.Add((item, centerLine));
                }
            }

            _grid = new AdaptiveGrid(new Transform());

            foreach (var line in centerlines)
            {
                _grid.AddVertices(line.Centerline.Vertices,
                    AdaptiveGrid.VerticesInsertionMethod.ConnectAndSelfIntersect);
            }

            Intersect(centerlines);
            Extend(centerlines);

            _roomExits = new Dictionary<SpaceBoundary, List<GridVertex>>();
            foreach (var room in rooms)
            {
                var exits = AddRoom(room, centerlines, walls, doors, _grid);
                _roomExits.Add(room, exits);
            }

            return _grid;
        }


        /// <summary>
        /// Add end point to the grid that are close enough to any of existing edges.
        /// </summary>
        /// <param name="exits">Exit points positions.</param>
        /// <param name="grid">AdaptiveGrid to insert new vertices and edge into.</param>
        /// <returns>Ids of exit vertices that are added to the grid.</returns>
        public ulong AddEndPoint(Vector3 exit, double snapDistance, out GridVertex closestVertex)
        {
            var edge = ClosestEdgeOnElevation(exit, out var closest);
            closestVertex = null;
            if (edge == null)
            {
                return 0u;
            }

            var startVertex = _grid.GetVertex(edge.StartId);
            var endVertex = _grid.GetVertex(edge.EndId);

            var exitOnLevel = new Vector3(exit.X, exit.Y, closest.Z);
            var distance = exitOnLevel.DistanceTo(closest);

            if (closest.IsAlmostEqualTo(startVertex.Point, _grid.Tolerance))
            {
                closestVertex = startVertex;
            }
            else if (closest.IsAlmostEqualTo(endVertex.Point, _grid.Tolerance))
            {
                closestVertex = endVertex;
            }
            else
            {
                closestVertex = _grid.CutEdge(edge, closest);
            }

            //Snap to existing vertex if it's close enough.
            if (distance < snapDistance)
            {
                return closestVertex.Id;
            }
            else
            {
                var vertex = _grid.AddVertex(exitOnLevel, new ConnectVertexStrategy(closestVertex), cut: false);
                return vertex.Id;
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
                                    if (!closestRightItem.IsAlmostEqualTo(start.Point) &&
                                        !closestRightItem.IsAlmostEqualTo(end.Point))
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
                    if (l.PointOnLine(line.Start) || l.PointOnLine(line.End))
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
            List<Door>? doors,
            AdaptiveGrid grid)
        {
            var roomExits = new List<GridVertex>();
            var perimeter = room.Boundary.Perimeter.CollinearPointsRemoved().TransformedPolygon(room.Transform);
            foreach (var roomEdge in perimeter.Segments())
            {
                var exitVertex = FindRoomExit(roomEdge, centerlines, walls, doors, grid);
                if (exitVertex != null)
                {
                    roomExits.Add(exitVertex);
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
        private GridVertex FindRoomExit(
            Line roomEdge,
            List<(CirculationSegment Segment, Polyline Centerline)> centerlines,
            List<WallCandidate>? walls,
            List<Door>? doors,
            AdaptiveGrid grid)
        {
            var door = doors?.FirstOrDefault(d => roomEdge.PointOnLine(d.Transform.Origin, false, RoomToWallTolerance));
            var wall = walls?.FirstOrDefault(w => w.Line.PointOnLine(roomEdge.PointAtNormalized(0.25), true, RoomToWallTolerance) &&
                                             w.Line.PointOnLine(roomEdge.PointAtNormalized(0.75), true, RoomToWallTolerance));

            // There are doors in the workflow and this segment is a wall without a door.
            if (wall != null && doors != null && door == null)
            {
                return null;
            }

            var midpoint = door?.Transform.Origin ?? roomEdge.Mid();
            var exitDirection = door?.Transform.XAxis ?? roomEdge.Direction();

            foreach (var line in centerlines)
            {
                for (int i = 0; i < line.Centerline.Vertices.Count - 1; i++)
                {
                    var segment = new Line(line.Centerline.Vertices[i], line.Centerline.Vertices[i + 1]);
                    var distance = midpoint.DistanceTo(segment, out var closest);
                    if (distance > line.Segment.Geometry.GetWidth() / 2 + 0.10)
                    {
                        continue;
                    }

                    GridVertex exitVertex = null;
                    grid.TryGetVertexIndex(segment.Start, out var id);
                    var vertex = grid.GetVertex(id);

                    //We know corridor line but it can already be split into several edges.
                    //Need to find exact edge to insert new vertex into.
                    //First vertex corresponding start of the segment is found.
                    //Then, edges that do in the same direction as segment is traversed
                    //until target edge is found or end vertex is reached.
                    //This is much faster than traverse every single edge in the grid.
                    if (vertex.Point.IsAlmostEqualTo(closest, grid.Tolerance))
                    {
                        exitVertex = vertex;
                    }
                    else
                    {
                        grid.TryGetVertexIndex(segment.End, out var endId);
                        var edge = FindOnCollinearEdges(vertex, endId, segment.Direction(), closest);
                        if (edge != null)
                        {
                            var start = grid.GetVertex(edge.StartId);
                            var end = grid.GetVertex(edge.EndId);

                            if (start.Point.IsAlmostEqualTo(closest, grid.Tolerance))
                            {
                                exitVertex = start;
                            }
                            else if (end.Point.IsAlmostEqualTo(closest, grid.Tolerance))
                            {
                                exitVertex = end;
                            }
                            else
                            {
                                exitVertex = grid.AddVertex(closest, new ConnectVertexStrategy(start, end), cut: false);
                                grid.RemoveEdge(edge);
                            }
                        }
                    }

                    if (exitVertex != null)
                    {
                        if (!exitVertex.Point.IsAlmostEqualTo(midpoint, grid.Tolerance))
                        {
                            var delta = closest - midpoint;
                            var dot = delta.Dot(segment.Direction());
                            if (dot.ApproximatelyEquals(0) || dot.ApproximatelyEquals(delta.Length()))
                            {
                                return grid.AddVertex(midpoint, new ConnectVertexStrategy(exitVertex));
                            }
                            else
                            {
                                var cornerPoint = Math.Abs(exitDirection.Dot(segment.Direction())) > 1 / Math.Sqrt(2) ?
                                    closest - dot * segment.Direction() : midpoint + dot * segment.Direction();
                                    
                                var strip = grid.AddVertices(
                                    new List<Vector3> { midpoint, cornerPoint, closest },
                                    AdaptiveGrid.VerticesInsertionMethod.ConnectAndCut);
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
