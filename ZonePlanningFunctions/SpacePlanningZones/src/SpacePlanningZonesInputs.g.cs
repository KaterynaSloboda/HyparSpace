// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Elements.Geometry.Solids;
using Elements.Validators;
using Elements.Serialization.JSON;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace SpacePlanningZones
{
    #pragma warning disable // Disable all warnings

    /// <summary>A polyline that has been thickened into a polygon.</summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class ThickenedPolyline 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public ThickenedPolyline(Polyline @polyline, double @width, bool @flip, double @leftWidth, double @rightWidth)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<ThickenedPolyline>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @polyline, @width, @flip, @leftWidth, @rightWidth});
            }
        
            this.Polyline = @polyline;
            this.Width = @width;
            this.Flip = @flip;
            this.LeftWidth = @leftWidth;
            this.RightWidth = @rightWidth;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("polyline", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Polyline Polyline { get; set; }
    
        [Newtonsoft.Json.JsonProperty("width", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Width { get; set; }
    
        [Newtonsoft.Json.JsonProperty("flip", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Flip { get; set; }
    
        /// <summary>The amount to thicken the polyline on its "left" side, imagining that the polyline is extending away from you. That is, if the polyline starts at (0,0,0) and follows the +Z axis, the left side extends into the -X quadrant.</summary>
        [Newtonsoft.Json.JsonProperty("leftWidth", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double LeftWidth { get; set; }
    
        /// <summary>The amount to thicken the polyline on its "right" side, imagining that the polyline is extending away from you. That is, if the polyline starts at (0,0,0) and follows the +Z axis, the right side extends into the +X quadrant.</summary>
        [Newtonsoft.Json.JsonProperty("rightWidth", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double RightWidth { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public  class SpacePlanningZonesInputs : S3Args
    
    {
        [Newtonsoft.Json.JsonConstructor]
        
        public SpacePlanningZonesInputs(string @defaultProgramAssignment, SpacePlanningZonesInputsCirculationMode @circulationMode, IList<ThickenedPolyline> @corridors, double @corridorWidth, double @outerBandDepth, double @depthAtEnds, IList<Vector3> @additionalCorridorLocations, IList<Vector3> @manualSplitLocations, Overrides @overrides, string bucketName, string uploadsBucket, Dictionary<string, string> modelInputKeys, string gltfKey, string elementsKey, string ifcKey):
        base(bucketName, uploadsBucket, modelInputKeys, gltfKey, elementsKey, ifcKey)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SpacePlanningZonesInputs>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @defaultProgramAssignment, @circulationMode, @corridors, @corridorWidth, @outerBandDepth, @depthAtEnds, @additionalCorridorLocations, @manualSplitLocations, @overrides});
            }
        
            this.DefaultProgramAssignment = @defaultProgramAssignment;
            this.CirculationMode = @circulationMode;
            this.Corridors = @corridors;
            this.CorridorWidth = @corridorWidth;
            this.OuterBandDepth = @outerBandDepth;
            this.DepthAtEnds = @depthAtEnds;
            this.AdditionalCorridorLocations = @additionalCorridorLocations;
            this.ManualSplitLocations = @manualSplitLocations;
            this.Overrides = @overrides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>What would you like the default program for all zones to be? This program type will be assigned to all spaces, and then you can pick specific programs for individual spaces with the Edit Program Assignments button.</summary>
        [Newtonsoft.Json.JsonProperty("Default Program Assignment", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DefaultProgramAssignment { get; set; } = "unspecified";
    
        /// <summary>How should circulation be calculated? 
        /// Automatic: a typical circulation network will be generated for you. 
        /// Manual: you draw the circulation paths yourself.</summary>
        [Newtonsoft.Json.JsonProperty("Circulation Mode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SpacePlanningZonesInputsCirculationMode CirculationMode { get; set; } = SpacePlanningZonesInputsCirculationMode.Automatic;
    
        /// <summary>Define the circulation network by drawing one or more corridor paths.</summary>
        [Newtonsoft.Json.JsonProperty("Corridors", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<ThickenedPolyline> Corridors { get; set; }
    
        /// <summary>How wide should circulation paths be?</summary>
        [Newtonsoft.Json.JsonProperty("Corridor Width", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0.3D, double.MaxValue)]
        public double CorridorWidth { get; set; } = 1.5D;
    
        /// <summary>For the "outer band" of program running along the floor perimeter, how deep should the spaces be?</summary>
        [Newtonsoft.Json.JsonProperty("Outer Band Depth", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0D, 20.0D)]
        public double OuterBandDepth { get; set; } = 6D;
    
        /// <summary>If your floorplate is rectangular, or has roughly rectangular ends, how deep should the spaces be at these ends?</summary>
        [Newtonsoft.Json.JsonProperty("Depth at Ends", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0D, 20.0D)]
        public double DepthAtEnds { get; set; } = 8D;
    
        /// <summary>Add new points to this list to insert additional corridor locations, to further subdivide the space. Corridors extend perpendicularly from the closest point on the boundary.</summary>
        [Newtonsoft.Json.JsonProperty("Additional Corridor Locations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<Vector3> AdditionalCorridorLocations { get; set; }
    
        /// <summary>Add new points to this list to insert additional program split locations, to further subdivide the space. This is similar to the corridor locations input above, but does not insert circulation between split spaces.</summary>
        [Newtonsoft.Json.JsonProperty("Manual Split Locations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<Vector3> ManualSplitLocations { get; set; }
    
        [Newtonsoft.Json.JsonProperty("overrides", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Overrides Overrides { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum SpacePlanningZonesInputsCirculationMode
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Automatic")]
        Automatic = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Manual")]
        Manual = 1,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class Overrides 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public Overrides(IList<ProgramAssignmentsOverride> @programAssignments, IList<MergeZonesOverride> @mergeZones)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<Overrides>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @programAssignments, @mergeZones});
            }
        
            this.ProgramAssignments = @programAssignments;
            this.MergeZones = @mergeZones;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Program Assignments", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<ProgramAssignmentsOverride> ProgramAssignments { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Merge Zones", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<MergeZonesOverride> MergeZones { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class ProgramAssignmentsOverride 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public ProgramAssignmentsOverride(string @id, ProgramAssignmentsIdentity @identity, ProgramAssignmentsValue @value)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<ProgramAssignmentsOverride>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @id, @identity, @value});
            }
        
            this.Id = @id;
            this.Identity = @identity;
            this.Value = @value;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Identity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ProgramAssignmentsIdentity Identity { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ProgramAssignmentsValue Value { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class MergeZonesOverride 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public MergeZonesOverride(string @id, IList<MergeZonesIdentity> @identities)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<MergeZonesOverride>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @id, @identities});
            }
        
            this.Id = @id;
            this.Identities = @identities;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Identities", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<MergeZonesIdentity> Identities { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class ProgramAssignmentsIdentity 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public ProgramAssignmentsIdentity(Vector3 @parentCentroid, Vector3 @individualCentroid)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<ProgramAssignmentsIdentity>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @parentCentroid, @individualCentroid});
            }
        
            this.ParentCentroid = @parentCentroid;
            this.IndividualCentroid = @individualCentroid;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("ParentCentroid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 ParentCentroid { get; set; }
    
        [Newtonsoft.Json.JsonProperty("IndividualCentroid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 IndividualCentroid { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class ProgramAssignmentsValue 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public ProgramAssignmentsValue(string @programType, int @split)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<ProgramAssignmentsValue>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @programType, @split});
            }
        
            this.ProgramType = @programType;
            this.Split = @split;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>What program should be assigned to this zone?</summary>
        [Newtonsoft.Json.JsonProperty("Program Type", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProgramType { get; set; }
    
        /// <summary>If you'd like to subdivide this zone into rooms, set the number of rooms.</summary>
        [Newtonsoft.Json.JsonProperty("Split", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Split { get; set; } = 1;
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class MergeZonesIdentity 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public MergeZonesIdentity(Vector3 @parentCentroid)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<MergeZonesIdentity>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @parentCentroid});
            }
        
            this.ParentCentroid = @parentCentroid;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("ParentCentroid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 ParentCentroid { get; set; }
    
    
    }
}