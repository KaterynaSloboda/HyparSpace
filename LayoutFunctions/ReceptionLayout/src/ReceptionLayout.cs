using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using Elements.Components;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System;
using Elements.Spatial;
using LayoutFunctionCommon;

namespace ReceptionLayout
{
    public static class ReceptionLayout
    {
        private static readonly List<ElementProxy<SpaceBoundary>> proxies = new List<ElementProxy<SpaceBoundary>>();

        private static readonly string SpaceBoundaryDependencyName = SpaceSettingsOverride.Dependency;

        /// <summary>
        /// Map between the layout and the number of seats it lays out
        /// </summary>
        private static Dictionary<string, int> _configSeats = new Dictionary<string, int>()
        {
            ["Configuration A"] = 3,
            ["Configuration B"] = 2,
            ["Configuration C"] = 1,
            ["Configuration D"] = 5,
            ["Configuration E"] = 6,
        };

        /// <summary>
        /// The ReceptionLayout function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A ReceptionLayoutOutputs instance containing computed results and the model with any new elements.</returns>
        public static ReceptionLayoutOutputs Execute(Dictionary<string, Model> inputModels, ReceptionLayoutInputs input)
        {
            Elements.Serialization.glTF.GltfExtensions.UseReferencedContentExtension = true;
            proxies.Clear();
            var spacePlanningZones = inputModels["Space Planning Zones"];
            inputModels.TryGetValue("Levels", out var levelsModel);
            var levels = spacePlanningZones.AllElementsOfType<LevelElements>();
            if (inputModels.TryGetValue("Circulation", out var circModel))
            {
                var circSegments = circModel.AllElementsOfType<CirculationSegment>();
                foreach (var cs in circSegments)
                {
                    var matchingLevel = levels.FirstOrDefault(l => l.Level == cs.Level);
                    if (matchingLevel != null)
                    {
                        matchingLevel.Elements.Add(cs);
                    }
                }
            }
            var levelVolumes = LayoutStrategies.GetLevelVolumes<LevelVolume>(inputModels);
            var output = new ReceptionLayoutOutputs();
            var configJson = File.ReadAllText("./ReceptionConfigurations.json");
            var configs = JsonConvert.DeserializeObject<SpaceConfiguration>(configJson);
            Configurations.Init(configs);

            var hasCore = inputModels.TryGetValue("Core", out var coresModel) && coresModel.AllElementsOfType<ServiceCore>().Any();
            List<Line> coreSegments = new();
            if (coresModel != null)
            {
                coreSegments.AddRange(coresModel.AllElementsOfType<ServiceCore>().SelectMany(c => c.Profile.Perimeter.Segments()));
            }
            var overridesById = GetOverridesByBoundaryId(input, levels);
            foreach (var lvl in levels)
            {
                var corridors = lvl.Elements.OfType<CirculationSegment>();
                var corridorSegments = corridors.SelectMany(p => p.Profile.Segments());
                var meetingRmBoundaries = lvl.Elements.OfType<SpaceBoundary>().Where(z => z.Name == "Reception");
                var levelVolume = levelVolumes.FirstOrDefault(l =>
                    (lvl.AdditionalProperties.TryGetValue("LevelVolumeId", out var levelVolumeId) &&
                        levelVolumeId as string == l.Id.ToString())) ??
                        levelVolumes.FirstOrDefault(l => l.Name == lvl.Name);

                foreach (var room in meetingRmBoundaries)
                {
                    var seatsCount = 0;
                    var spaceBoundary = room.Boundary;
                    var config = MatchApplicableOverride(overridesById, GetElementProxy(room, meetingRmBoundaries.Proxies(SpaceBoundaryDependencyName)), input);
                    Line orientationGuideEdge = hasCore ? FindEdgeClosestToCore(spaceBoundary.Perimeter, coreSegments) : FindEdgeAdjacentToSegments(spaceBoundary.Perimeter.Segments(), corridorSegments, out var wallCandidates);

                    var orientationTransform = new Transform(Vector3.Origin, orientationGuideEdge.Direction(), Vector3.ZAxis);
                    var boundaryCurves = new List<Polygon>
                    {
                        spaceBoundary.Perimeter
                    };
                    boundaryCurves.AddRange(spaceBoundary.Voids ?? new List<Polygon>());

                    var grid = new Grid2d(boundaryCurves, orientationTransform);
                    foreach (var cell in grid.GetCells())
                    {
                        var rect = cell.GetCellGeometry() as Polygon;
                        var segs = rect.Segments();
                        var width = segs[0].Length();
                        var depth = segs[1].Length();
                        var trimmedGeo = cell.GetTrimmedCellGeometry();
                        var selectedConfigs = Configurations.GetConfigs(config.Value.PrimaryAxisFlipLayout, config.Value.SecondaryAxisFlipLayout);
                        if (!cell.IsTrimmed() && trimmedGeo.Length > 0)
                        {
                            var layout = InstantiateLayout(selectedConfigs, width, depth, rect, room.Transform, out var seats);
                            LayoutStrategies.SetLevelVolume(layout, levelVolume?.Id);
                            output.Model.AddElement(layout);
                            seatsCount += seats;
                        }
                        else if (trimmedGeo.Length > 0)
                        {
                            var largestTrimmedShape = trimmedGeo.OfType<Polygon>().OrderBy(s => s.Area()).Last();
                            var cinchedVertices = rect.Vertices.Select(v => largestTrimmedShape.Vertices.OrderBy(v2 => v2.DistanceTo(v)).First()).ToList();
                            var cinchedPoly = new Polygon(cinchedVertices);
                            // output.Model.AddElement(new ModelCurve(cinchedPoly, BuiltInMaterials.ZAxis, levelVolume.Transform));
                            var layout = InstantiateLayout(selectedConfigs, width, depth, cinchedPoly, room.Transform, out var seats);
                            LayoutStrategies.SetLevelVolume(layout, levelVolume?.Id);
                            output.Model.AddElement(layout);
                            Console.WriteLine("🤷‍♂️ funny shape!!!");
                            seatsCount += seats;
                        }
                    }

                    output.Model.AddElement(new SpaceMetric(room.Id, seatsCount, 0, 0, 0));
                }
            }
            OverrideUtilities.InstancePositionOverrides(input.Overrides, output.Model);
            output.Model.AddElements(proxies);
            return output;
        }

        private static Line FindEdgeClosestToCore(Polygon perimeter, List<Line> coreSegments)
        {
            double dist = double.MaxValue;
            Line bestLine = null;

            foreach (var line in perimeter.Segments())
            {
                var lineMidPt = line.Mid();
                var linePerp = line.Direction().Cross(Vector3.ZAxis).Unitized();
                foreach (var coreSegment in coreSegments)
                {
                    // don't consider perpendicular edges
                    if (Math.Abs(coreSegment.Direction().Dot(line.Direction())) < 0.01)
                    {
                        continue;
                    }
                    var ptOnCoreSegment = lineMidPt.ClosestPointOn(coreSegment);
                    var thisDist = ptOnCoreSegment.DistanceTo(lineMidPt);
                    if (thisDist < dist)
                    {
                        dist = thisDist;
                        bestLine = line;
                    }
                }

            }

            return bestLine;
        }

        private static Line FindEdgeAdjacentToSegments(IEnumerable<Line> edgesToClassify, IEnumerable<Line> corridorSegments, out IEnumerable<Line> otherSegments, double maxDist = 0)
        {
            var minDist = double.MaxValue;
            var minSeg = edgesToClassify.First();
            var allEdges = edgesToClassify.ToList();
            var selectedIndex = 0;
            for (int i = 0; i < allEdges.Count; i++)
            {
                var edge = allEdges[i];
                var midpt = edge.Mid();
                foreach (var seg in corridorSegments)
                {
                    var dist = midpt.DistanceTo(seg);
                    // if two segments are basically the same distance to the corridor segment,
                    // prefer the longer one.
                    if (Math.Abs(dist - minDist) < 0.1)
                    {
                        minDist = dist;
                        if (minSeg.Length() < edge.Length())
                        {
                            minSeg = edge;
                            selectedIndex = i;
                        }
                    }
                    else if (dist < minDist)
                    {
                        minDist = dist;
                        minSeg = edge;
                        selectedIndex = i;
                    }
                }
            }
            if (maxDist != 0)
            {
                if (minDist < maxDist)
                {

                    otherSegments = Enumerable.Range(0, allEdges.Count).Except(new[] { selectedIndex }).Select(i => allEdges[i]);
                    return minSeg;
                }
                else
                {
                    Console.WriteLine($"no matches: {minDist}");
                    otherSegments = allEdges;
                    return null;
                }
            }
            otherSegments = Enumerable.Range(0, allEdges.Count).Except(new[] { selectedIndex }).Select(i => allEdges[i]);
            return minSeg;
        }
        private static ComponentInstance InstantiateLayout(SpaceConfiguration configs, double width, double length, Polygon rectangle, Transform xform, out int seatsCount)
        {
            seatsCount = 0;
            ContentConfiguration selectedConfig = null;
            var orderedKeys = configs.OrderByDescending(kvp => kvp.Value.CellBoundary.Depth * kvp.Value.CellBoundary.Width).Select(kvp => kvp.Key);
            foreach (var key in orderedKeys)
            {
                var config = configs[key];
                if (config.CellBoundary.Width < width && config.CellBoundary.Depth < length)
                {
                    selectedConfig = config;
                    seatsCount += _configSeats[key];
                    break;
                }
            }
            if (selectedConfig == null)
            {
                return null;
            }
            var baseRectangle = Polygon.Rectangle(selectedConfig.CellBoundary.Min, selectedConfig.CellBoundary.Max);
            var rules = selectedConfig.Rules();

            var componentDefinition = new ComponentDefinition(rules, selectedConfig.Anchors());
            var instance = componentDefinition.Instantiate(ContentConfiguration.AnchorsFromRect(rectangle.TransformedPolygon(xform)));
            var allPlacedInstances = instance.Instances;
            foreach (var item in allPlacedInstances)
            {
                if (item is ElementInstance ei && !rectangle.Contains(ei.Transform.Origin))
                {

                }
            }
            return instance;
        }

        private static Dictionary<Guid, SpaceSettingsOverride> GetOverridesByBoundaryId(ReceptionLayoutInputs input, IEnumerable<LevelElements> levels)
        {
            var overridesById = new Dictionary<Guid, SpaceSettingsOverride>();
            foreach (var spaceOverride in input.Overrides?.SpaceSettings ?? new List<SpaceSettingsOverride>())
            {
                var matchingBoundary =
                levels.SelectMany(l => l.Elements)
                    .OfType<SpaceBoundary>()
                    .OrderBy(ob => ob.ParentCentroid.Value
                    .DistanceTo(spaceOverride.Identity.ParentCentroid))
                    .First();

                if (overridesById.ContainsKey(matchingBoundary.Id))
                {
                    var mbCentroid = matchingBoundary.ParentCentroid.Value;
                    if (overridesById[matchingBoundary.Id].Identity.ParentCentroid.DistanceTo(mbCentroid) > spaceOverride.Identity.ParentCentroid.DistanceTo(mbCentroid))
                    {
                        overridesById[matchingBoundary.Id] = spaceOverride;
                    }
                }
                else
                {
                    overridesById.Add(matchingBoundary.Id, spaceOverride);
                }
            }

            return overridesById;
        }

        private static SpaceSettingsOverride MatchApplicableOverride(
            Dictionary<Guid, SpaceSettingsOverride> overridesById,
            ElementProxy<SpaceBoundary> boundaryProxy,
            ReceptionLayoutInputs input)
        {
            var overrideName = SpaceSettingsOverride.Name;
            SpaceSettingsOverride config;

            // See if we already have matching override attached
            var existingOverrideId = boundaryProxy.OverrideIds<SpaceSettingsOverride>(overrideName).FirstOrDefault();
            if (existingOverrideId != null)
            {
                if (overridesById.TryGetValue(Guid.Parse(existingOverrideId), out config))
                {
                    return config;
                }
            }

            // Try to match from identity in configs dictionary. Use a default in case none found
            if (!overridesById.TryGetValue(boundaryProxy.ElementId, out config))
            {
                config = new SpaceSettingsOverride(
                            Guid.NewGuid().ToString(),
                            null,
                            new SpaceSettingsValue(
                                false,
                                false
                            )
                    );
                overridesById.Add(boundaryProxy.ElementId, config);
            }

            // Attach the identity and values data to the proxy
            boundaryProxy.AddOverrideIdentity(overrideName, config.Id, config.Identity);
            boundaryProxy.AddOverrideValue(overrideName, config.Value);

            // Make sure proxies list has the proxy so that it will serialize in the model.
            if (!proxies.Contains(boundaryProxy))
            {
                proxies.Add(boundaryProxy);
            }

            return config;
        }

        private static ElementProxy<SpaceBoundary> GetElementProxy(SpaceBoundary spaceBoundary, IEnumerable<ElementProxy<SpaceBoundary>> allSpaceBoundaries)
        {
            return allSpaceBoundaries.Proxy(spaceBoundary) ?? spaceBoundary.Proxy(SpaceBoundaryDependencyName);
        }
    }

}