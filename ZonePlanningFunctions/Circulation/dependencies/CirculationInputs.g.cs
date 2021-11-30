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

namespace Circulation
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
    
    /// <summary>A collection of points used to split a set of polygons </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class PolygonSplitCollection 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public PolygonSplitCollection(IList<SplitLocations> @splitLocations)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<PolygonSplitCollection>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @splitLocations});
            }
        
            this.SplitLocations = @splitLocations;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>The lines representing split locations. The start of each line should be treated as its anchor - the second is only an indication of its direction</summary>
        [Newtonsoft.Json.JsonProperty("SplitLocations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<SplitLocations> SplitLocations { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public  class CirculationInputs : S3Args
    
    {
        [Newtonsoft.Json.JsonConstructor]
        
        public CirculationInputs(string @defaultProgramAssignment, IList<ThickenedPolyline> @corridors, double @corridorWidth, double @outerBandDepth, CirculationInputsCirculationMode @circulationMode, double @depthAtEnds, IList<Vector3> @additionalCorridorLocations, PolygonSplitCollection @addCorridors, string bucketName, string uploadsBucket, Dictionary<string, string> modelInputKeys, string gltfKey, string elementsKey, string ifcKey):
        base(bucketName, uploadsBucket, modelInputKeys, gltfKey, elementsKey, ifcKey)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<CirculationInputs>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @defaultProgramAssignment, @corridors, @corridorWidth, @outerBandDepth, @circulationMode, @depthAtEnds, @additionalCorridorLocations, @addCorridors});
            }
        
            this.DefaultProgramAssignment = @defaultProgramAssignment;
            this.Corridors = @corridors;
            this.CorridorWidth = @corridorWidth;
            this.OuterBandDepth = @outerBandDepth;
            this.CirculationMode = @circulationMode;
            this.DepthAtEnds = @depthAtEnds;
            this.AdditionalCorridorLocations = @additionalCorridorLocations;
            this.AddCorridors = @addCorridors;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>What would you like the default program for all zones to be? This program type will be assigned to all spaces, and then you can pick specific programs for individual spaces with the Edit Program Assignments button.</summary>
        [Newtonsoft.Json.JsonProperty("Default Program Assignment", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DefaultProgramAssignment { get; set; } = "unspecified";
    
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
    
        /// <summary>How should circulation be calculated? 
        /// Automatic: a typical circulation network will be generated for you. 
        /// Manual: you draw the circulation paths yourself.</summary>
        [Newtonsoft.Json.JsonProperty("Circulation Mode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CirculationInputsCirculationMode CirculationMode { get; set; } = CirculationInputsCirculationMode.Automatic;
    
        /// <summary>If your floorplate is rectangular, or has roughly rectangular ends, how deep should the spaces be at these ends?</summary>
        [Newtonsoft.Json.JsonProperty("Depth at Ends", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0D, 20.0D)]
        public double DepthAtEnds { get; set; } = 8D;
    
        /// <summary>Add new points to this list to insert additional corridor locations, to further subdivide the space. Corridors extend perpendicularly from the closest point on the boundary.</summary>
        [Newtonsoft.Json.JsonProperty("Additional Corridor Locations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<Vector3> AdditionalCorridorLocations { get; set; }
    
        /// <summary>Insert additional corridors, to further subdivide the space.</summary>
        [Newtonsoft.Json.JsonProperty("Add Corridors", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PolygonSplitCollection AddCorridors { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class SplitLocations 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public SplitLocations(Vector3 @position, Vector3 @direction)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SplitLocations>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @position, @direction});
            }
        
            this.Position = @position;
            this.Direction = @direction;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("position", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 Position { get; set; }
    
        [Newtonsoft.Json.JsonProperty("direction", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 Direction { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum CirculationInputsCirculationMode
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Automatic")]
        Automatic = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Manual")]
        Manual = 1,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    internal class JsonInheritanceAttribute : System.Attribute
    {
        public JsonInheritanceAttribute(string key, System.Type type)
        {
            Key = key;
            Type = type;
        }
    
        public string Key { get; }
    
        public System.Type Type { get; }
    }
}