using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using System.Linq;
using LayoutFunctionCommon;
using System;
using System.Runtime.CompilerServices;
using Elements.Spatial;
using Elements.Geometry.Solids;

[assembly: InternalsVisibleTo("InteriorPartitions.Tests")]
namespace InteriorPartitions
{
    public static class InteriorPartitions
    {
        private const string wallCandidatePropertyName = "Wall Candidate";
        private static double mullionSize = 0.07;
        private static double doorWidth = 0.9;
        private static double doorHeight = 2.1;
        private static double sideLightWidth = 0.4;
        private static double defaultHeight = 3;

        private static Dictionary<string, int> interiorPartitionTypePriority = new Dictionary<string, int>()
        {
            {"Solid", 3},
            {"Partition", 2},
            {"Glass", 1}
        };

        private static Material wallMat = new Material("Drywall", new Color(0.9, 0.9, 0.9, 1.0), 0.01, 0.01);
        private static Material glassMat = new Material("Glass", new Color(0.7, 0.7, 0.7, 0.3), 0.3, 0.6);
        private static Material mullionMat = new Material("Storefront Mullions", new Color(0.5, 0.5, 0.5, 1.0));

        /// <summary>
        /// The InteriorPartitions function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A InteriorPartitionsOutputs instance containing computed results and the model with any new elements.</returns>
        public static InteriorPartitionsOutputs Execute(Dictionary<string, Model> inputModels, InteriorPartitionsInputs input)
        {
            var interiorPartitionCandidates = new List<InteriorPartitionCandidate>();
            var modelDependencies = new[] {
                "Private Office Layout",
                "Phone Booth Layout",
                "Classroom Layout",
                "Meeting Room Layout",
                "Space Planning Zones",
                "Bedroom Layout",
                "Living Room Layout",
                "Kitchen Layout",
                "Workshop Layout",
                "Home Office Layout",
                "Bathroom Layout",
                "Restroom Layout",
                "Laundry Room Layout",
                "Entertainment Room Layout",
                "Room Layout",
                "Furniture and Equipment"
                 };
            foreach (var md in modelDependencies)
            {
                if (inputModels.TryGetValue(md, out var mdModel))
                {
                    interiorPartitionCandidates.AddRange(mdModel?.AllElementsOfType<InteriorPartitionCandidate>());
                }
            }

            inputModels.TryGetValue("Circulation", out var circulationModel);
            List<CirculationSegment> circulationSegments = circulationModel?.AllElementsOfType<CirculationSegment>().ToList() ?? new List<CirculationSegment>();

            var output = new InteriorPartitionsOutputs();
            var wallCandidates = CreateWallCandidates(input, interiorPartitionCandidates);

            var wallCandidatesGroups = wallCandidates.GroupBy(w => (w.LevelTransform, w.Height));

            foreach (var wallCandidate in wallCandidates)
            {
                var elements = GenerateWall(wallCandidate);

                output.Model.AddElements(elements);
            }

            // foreach (var wallCandidatesGroup in wallCandidatesGroups)
            // {
            //     var elements = WallGeneration.GenerateWalls(wallCandidatesGroup.Select(w => (w.Line, w.Type, w.Id, w.Thickness)), wallCandidatesGroup.Key.Height, wallCandidatesGroup.Key.LevelTransform, false);

            //     output.Model.AddElements(elements);
            // }

            output.Model.AddElements(wallCandidates);

            return output;
        }


        private static GeometricElement CreateMullion(double height)
        {
            var totalStorefrontHeight = height;
            var mullion = new Mullion
            {
                BaseLine = new Line(new Vector3(-mullionSize / 2, 0, 0), new Vector3(mullionSize / 2, 0, 0)),
                Width = mullionSize,
                Height = totalStorefrontHeight,
                Material = mullionMat,
                IsElementDefinition = true
            };
            return mullion;
        }

        public static List<Element> GenerateWall(WallCandidate wallCandidate)
        {
            var elements = new List<Element>();

            var representations = new List<RepresentationInstance>();

            var totalStorefrontHeight = Math.Min(2.7, wallCandidate.Height);

            var mullion = CreateMullion(totalStorefrontHeight);

            var line = wallCandidate.Line;
            var thickness = wallCandidate.Thickness;
            var type = wallCandidate.Type;
            var height = wallCandidate.Height;
            var levelTransform = wallCandidate.LevelTransform;
            var wallCandidateId = wallCandidate.Id;

            var lineProjected = line.TransformedLine(new Transform(0, 0, -line.End.Z));
            if (thickness != null && thickness.Value.innerWidth == 0 && thickness.Value.outerWidth == 0)
            {
                return elements;
            }
            var thicknessOrDefault = thickness ?? (type == "Solid" ? (0.1, 0.1) : (0.05, 0.05));
            var sumThickness = thicknessOrDefault.innerWidth + thicknessOrDefault.outerWidth;
            // the line we supply for the wall creation is always a
            // centerline. If the left thickness doesn't equal the right
            // thickness, we have to offset the centerline by their
            // difference.
            var offset = (thicknessOrDefault.outerWidth - thicknessOrDefault.innerWidth) / 2.0;
            lineProjected = lineProjected.Offset(offset, false);
            if (sumThickness < 0.01)
            {
                sumThickness = 0.2;
            }

            StandardWall wall = null;

            if (type == "Solid")
            {
                wall = new StandardWall(lineProjected, sumThickness, height, wallMat, levelTransform);
                wall.AdditionalProperties[wallCandidatePropertyName] = wallCandidateId;

                if (wall.CenterLine.Length() > doorWidth + 2 * sideLightWidth && wallCandidate.PrimaryEntryEdge != null ? wallCandidate.PrimaryEntryEdge.Value : false)
                {
                    double tPos = (sideLightWidth + doorWidth / 2) / wall.CenterLine.Length();
                    // Adding Wall as a property to door makes the wall not show up :shrug:
                    var door = new Door(null, wall.CenterLine, tPos, doorWidth, doorHeight, DoorOpeningSide.LeftHand, DoorOpeningType.SingleSwing, 1, 1, true)
                    {
                    };
                    door.Transform.Concatenate(new Transform(0, 0, 0.005));
                    wall.Openings.Add(door.Opening);

                    elements.Add(door);
                }

                RepresentationInstance wallRepresentationInstance = CreateWallRepresentationInstance(wall);
                wall.RepresentationInstances.Add(wallRepresentationInstance);
            }
            else if (type == "Partition")
            {
                wall = new StandardWall(lineProjected, sumThickness, height, wallMat, levelTransform);
                wall.AdditionalProperties[wallCandidatePropertyName] = wallCandidateId;

                RepresentationInstance wallRepresentationInstance = CreateWallRepresentationInstance(wall);
                wall.RepresentationInstances.Add(wallRepresentationInstance);
            }
            else if (type == "Glass")
            {
                wall = new StorefrontWall(lineProjected, 0.05, height, glassMat, levelTransform);
                wall.AdditionalProperties[wallCandidatePropertyName] = wallCandidateId;
                var grid = new Grid1d(lineProjected);
                var offsets = new[] { sideLightWidth, sideLightWidth + doorWidth }.Where(o => grid.Domain.Min + o < grid.Domain.Max);

                if (wall.CenterLine.Length() > doorWidth + 2 * sideLightWidth && wallCandidate.PrimaryEntryEdge != null ? wallCandidate.PrimaryEntryEdge.Value : false)
                {
                    double tPos = (sideLightWidth + doorWidth / 2) / wall.CenterLine.Length();
                    var door = new Door(null, wall.CenterLine, tPos, doorWidth, doorHeight, DoorOpeningSide.LeftHand, DoorOpeningType.SingleSwing, 1, 1, true)
                    {
                    };

                    wall.Openings.Add(door.Opening);
                    elements.Add(door);
                }

                RepresentationInstance wallRepresentationInstance = CreateWallRepresentationInstance(wall);
                wall.RepresentationInstances.Add(wallRepresentationInstance);

                grid.SplitAtOffsets(offsets);
                if (grid.Cells != null && grid.Cells.Count >= 3)
                {
                    grid[2].DivideByApproximateLength(2);
                }
                var separators = grid.GetCellSeparators(true);
                var beam = new Beam(lineProjected, Polygon.Rectangle(mullionSize, mullionSize), null, mullionMat)
                {
                    IsElementDefinition = true
                };
                var mullionInstances = new[] {
                        beam.CreateInstance(levelTransform, "Base Mullion"),
                        beam.CreateInstance(levelTransform.Concatenated(new Transform(0, 0, doorHeight)), "Base Mullion"),
                        beam.CreateInstance(levelTransform.Concatenated(new Transform(0, 0, totalStorefrontHeight)), "Base Mullion")
                    };

                var baseMullions = new List<Beam>()
                {
                    new Beam(beam.Curve, beam.Profile, new Transform(), beam.Material, null, false, default, "Base Mullion"),
                    new Beam((BoundedCurve)beam.Curve.Transformed(new Transform(0, 0, doorHeight)), beam.Profile, new Transform(0, 0, doorHeight), beam.Material, null, false, default, "Base Mullion"),
                    new Beam((BoundedCurve)beam.Curve.Transformed(new Transform(0, 0, totalStorefrontHeight)), beam.Profile, new Transform(0, 0, totalStorefrontHeight), beam.Material, null, false, default, "Base Mullion")
                };

                foreach (var baseMullion in baseMullions)
                {
                    baseMullion.UpdateRepresentations();
                    var mullionRep = baseMullion.Representation;
                    var repInstance = new RepresentationInstance(new SolidRepresentation(mullionRep.SolidOperations), baseMullion.Material, true);

                    wall.RepresentationInstances.Add(repInstance);
                }

                // foreach (var mullionInstance in mullionInstances)
                // {
                //     mullionInstance.AdditionalProperties["Wall"] = wall.Id;
                //     mullionInstance.BaseDefinition.UpdateRepresentations();

                //     elements.Add(mullionInstance);
                // }

                foreach (var separator in separators)
                {
                    var mullionLine = new Line(new Vector3(-mullionSize / 2, 0, 0), new Vector3(mullionSize / 2, 0, 0));

                    var mullionObject = new Mullion()
                    {
                        BaseLine = (Line)mullionLine.Transformed(new Transform(separator, lineProjected.Direction(), Vector3.ZAxis, 0).Concatenated(levelTransform)),
                        Width = mullionSize,
                        Height = totalStorefrontHeight,
                        Material = mullionMat
                    };

                    mullionObject.UpdateRepresentations();
                    var mullionRep = mullionObject.Representation;
                    wall.RepresentationInstances.Add(new RepresentationInstance(new SolidRepresentation(mullionRep.SolidOperations), mullionObject.Material, true));

                    // var line = new Line(separator, separator + new Vector3(0, 0, height));
                    // elements.Add(new ModelCurve(line, BuiltInMaterials.XAxis, levelTransform));
                    // var instance = mullion.CreateInstance(new Transform(separator, lineProjected.Direction(), Vector3.ZAxis, 0).Concatenated(levelTransform), "Mullion");
                    // mullionObject.AdditionalProperties["Wall"] = wall.Id;
                    // elements.Add(mullionObject);
                }

                var headerHeight = height - totalStorefrontHeight;
                if (headerHeight > 0.01)
                {
                    var header = new Header((Line)lineProjected.Transformed(levelTransform.Concatenated(new Transform(0, 0, totalStorefrontHeight))), sumThickness, headerHeight, wallMat);
                    // header.AdditionalProperties["Wall"] = wall.Id;
                    // elements.Add(header);
                    // header.AdditionalProperties[wallCandidatePropertyName] = wallCandidateId;
                    header.UpdateRepresentations();
                    var headerRep = header.Representation;
                    wall.RepresentationInstances.Add(new RepresentationInstance(new SolidRepresentation(headerRep.SolidOperations), header.Material, true));
                }
            }

            wall.UpdateRepresentations();

            elements.Add(wall);

            return elements;
        }

        private static RepresentationInstance CreateWallRepresentationInstance(StandardWall wall)
        {
            Line wallLine1 = wall.CenterLine.Offset(wall.Thickness / 2.0, flip: false);
            Line wallLine2 = wall.CenterLine.Offset(wall.Thickness / 2.0, flip: true);
            Polygon polygon = new Polygon(wallLine1.Start, wallLine1.End, wallLine2.End, wallLine2.Start);
            var wallExtrude = new Extrude(polygon, wall.Height, Vector3.ZAxis);
            var wallRepresentationInstance = new RepresentationInstance(new SolidRepresentation(wallExtrude), wall.Material, true);
            return wallRepresentationInstance;
        }

        internal static List<WallCandidate> CreateWallCandidates(InteriorPartitionsInputs input, List<InteriorPartitionCandidate> interiorPartitionCandidates)
        {
            // TODO: Update this when the upstream Layout functions set the PrimaryEntryEdge correctly
            foreach (var interiorPartitionCandidate in interiorPartitionCandidates)
            {
                foreach (var roomEdge in interiorPartitionCandidate.WallCandidateLines)
                {
                    if (roomEdge.Type.Contains("Glass"))
                    {
                        roomEdge.PrimaryEntryEdge = true;
                    }
                }
            }

            // TODO: don't assume one height for all walls on a level — pass height through deduplication.
            var levelGroups = interiorPartitionCandidates.Where(c => c.WallCandidateLines.Count > 0).GroupBy(c => c.LevelTransform);
            var wallCandidates = new List<WallCandidate>();
            var userAddedWallLinesCandidates = new List<WallCandidate>();
            if (input.Overrides?.InteriorPartitionTypes != null)
            {
                userAddedWallLinesCandidates = input.Overrides.InteriorPartitionTypes.CreateElementsFromEdits(
                    (wall, ident) => MatchIdentityWallCandidate(wall, ident),
                    (wall, edit) => UpdateWallCandidate(wall, edit)
                );
            }

            foreach (var levelGroup in levelGroups)
            {
                var candidates = WallGeneration.DeduplicateWallLines(levelGroup.ToList());
                var height = levelGroup.OrderBy(l => l.Height).FirstOrDefault()?.Height ?? defaultHeight;
                var levelWallCandidates = candidates.Select(c =>
                    new WallCandidate(c.Line.TransformedLine(levelGroup.Key),
                                      c.Type,
                                      height,
                                      levelGroup.Key,
                                      new List<SpaceBoundary>())
                    {
                        Thickness = c.Thickness,
                        PrimaryEntryEdge = c.PrimaryEntryEdge
                    });
                if (input.Overrides?.InteriorPartitionTypes != null)
                {
                    levelWallCandidates = UpdateLevelWallCandidates(levelWallCandidates, input.Overrides.InteriorPartitionTypes);
                }

                var splittedCandidates = WallGeneration.SplitOverlappingWallCandidates(
                    levelWallCandidates.Select(w => new RoomEdge
                    {
                        Line = w.Line,
                        Type = w.Type,
                        Thickness = w.Thickness,
                        PrimaryEntryEdge = w.PrimaryEntryEdge

                    }),
                    userAddedWallLinesCandidates.Select(w => new RoomEdge()
                    {
                        Line = w.Line.TransformedLine(w.LevelTransform),
                        Type = w.Type,
                        Thickness = w.Thickness,
                        PrimaryEntryEdge = w.PrimaryEntryEdge
                    }));
                var splittedWallCandidates = splittedCandidates
                    .Select(c => new WallCandidate(c.Line, c.Type, height, levelGroup.Key, new List<SpaceBoundary>())
                    {
                        Thickness = c.Thickness,
                        PrimaryEntryEdge = c.PrimaryEntryEdge
                    })
                    .ToList();

                wallCandidates.AddRange(splittedWallCandidates);
                // wallCandidates.AddRange(levelWallCandidates);
            }
            AttachOverrides(input.Overrides.InteriorPartitionTypes, wallCandidates);

            return wallCandidates;
        }

        private static bool MatchIdentityWallCandidate(WallCandidate wallCandidate, InteriorPartitionTypesIdentity ident)
        {
            var isLinesEqual = ident.Line.IsAlmostEqualTo(wallCandidate.Line, false, 0.1);
            return ident.AddId?.Equals(wallCandidate.AddId?.ToString()) == true && isLinesEqual;
        }

        private static WallCandidate UpdateWallCandidate(WallCandidate wallCandidate, InteriorPartitionTypesOverride edit)
        {
            wallCandidate.Type = edit.Value.Type.ToString();
            return wallCandidate;
        }

        public static List<WallCandidate> CreateElementsFromEdits(
            this IList<InteriorPartitionTypesOverride> edits,
            Func<WallCandidate, InteriorPartitionTypesIdentity, bool> identityMatch,
            Func<WallCandidate, InteriorPartitionTypesOverride, WallCandidate> modifyElement)
        {
            var resultElements = new List<WallCandidate>();
            if (edits != null)
            {
                foreach (var editedElement in edits)
                {
                    var elementToEdit = resultElements.FirstOrDefault(e => identityMatch(e, editedElement.Identity));
                    if (elementToEdit != null)
                    {
                        resultElements.Remove(elementToEdit);
                        var newElement = modifyElement(elementToEdit, editedElement);
                        resultElements.Add(newElement);
                        Identity.AddOverrideIdentity(newElement, editedElement);
                    }
                    else
                    {
                        // Not editing line, so we are using the original identity line
                        var newElement = new WallCandidate(editedElement.Identity.Line, editedElement.Value.Type.ToString(), defaultHeight, new Transform(), null);
                        resultElements.Add(newElement);
                        Identity.AddOverrideIdentity(newElement, editedElement);
                    }
                }
            }
            return resultElements;
        }

        private static List<WallCandidate> UpdateLevelWallCandidates(
            IEnumerable<WallCandidate> levelWallCandidates,
            IList<InteriorPartitionTypesOverride> edits)
        {
            var resultElements = new List<WallCandidate>(levelWallCandidates);
            if (edits != null)
            {
                foreach (var editedElement in edits)
                {
                    WallCandidate overlappingWallCandidate = null;
                    var identityLine = editedElement.Identity.Line;
                    foreach (var wallCandidate in resultElements)
                    {
                        if (!wallCandidate.Line.IsCollinear(identityLine))
                        {
                            continue;
                        }

                        // check if secondLine lies inside firstLine
                        if (!Line.PointOnLine(identityLine.Start, wallCandidate.Line.Start, wallCandidate.Line.End, true)
                            || !Line.PointOnLine(identityLine.End, wallCandidate.Line.Start, wallCandidate.Line.End, true))
                        {
                            continue;
                        }

                        overlappingWallCandidate = wallCandidate;
                        break;
                    }

                    if (overlappingWallCandidate != null)
                    {
                        var overlappingLine = overlappingWallCandidate.Line;
                        var vectors = new List<Vector3>() { overlappingLine.Start, overlappingLine.End, identityLine.Start, identityLine.End };
                        var direction = overlappingLine.Direction();
                        var orderedVectors = vectors.OrderBy(v => (v - overlappingLine.Start).Dot(direction)).ToList();

                        resultElements.Remove(overlappingWallCandidate);
                        if (!orderedVectors[0].IsAlmostEqualTo(orderedVectors[1]))
                        {
                            resultElements.Add(new WallCandidate(new Line(orderedVectors[0], orderedVectors[1]), overlappingWallCandidate.Type, overlappingWallCandidate.Height, overlappingWallCandidate.LevelTransform)
                            {
                                Thickness = overlappingWallCandidate.Thickness,
                                PrimaryEntryEdge = overlappingWallCandidate.PrimaryEntryEdge
                            });
                        }

                        // Not editing line, so we are using the original identity line
                        resultElements.Add(new WallCandidate(editedElement.Identity.Line, editedElement.Value.Type.ToString(), overlappingWallCandidate.Height, overlappingWallCandidate.LevelTransform)
                        {
                            Thickness = overlappingWallCandidate.Thickness,
                            PrimaryEntryEdge = overlappingWallCandidate.PrimaryEntryEdge
                        });

                        if (!orderedVectors[2].IsAlmostEqualTo(orderedVectors[3]))
                        {
                            resultElements.Add(new WallCandidate(new Line(orderedVectors[2], orderedVectors[3]), overlappingWallCandidate.Type, overlappingWallCandidate.Height, overlappingWallCandidate.LevelTransform)
                            {
                                Thickness = overlappingWallCandidate.Thickness,
                                PrimaryEntryEdge = overlappingWallCandidate.PrimaryEntryEdge
                            });
                        }
                    }
                }
            }
            return resultElements;
        }

        public static void AttachOverrides(this IList<InteriorPartitionTypesOverride> overrideData, IEnumerable<WallCandidate> existingElements)
        {
            if (overrideData != null)
            {
                foreach (var overrideValue in overrideData)
                {
                    // Not editing line, so we are using the original identity line
                    var matchingElement = existingElements.FirstOrDefault(e => overrideValue.Identity.Line.IsAlmostEqualTo(e.Line, false, 0.01));
                    if (matchingElement != null)
                    {
                        matchingElement.Type = overrideValue.Value.Type.ToString();
                        Identity.AddOverrideIdentity(matchingElement, overrideValue);
                    }
                }
            }
        }
    }
}