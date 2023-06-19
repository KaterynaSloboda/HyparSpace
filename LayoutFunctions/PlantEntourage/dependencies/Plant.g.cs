//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.1.21.0 (Newtonsoft.Json v13.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------
using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Elements.Geometry.Solids;
using Elements.Spatial;
using Elements.Validators;
using Elements.Serialization.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace Elements
{
    #pragma warning disable // Disable all warnings

    /// <summary>A plant potted or otherwise.</summary>
    [JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Plant : GeometricElement
    {
        [JsonConstructor]
        public Plant(double @baseWidth, double @baseLength, double @height, Transform @transform = null, Material @material = null, Representation @representation = null, bool @isElementDefinition = false, System.Guid @id = default, string @name = null)
            : base(transform, material, representation, isElementDefinition, id, name)
        {
            this.BaseWidth = @baseWidth;
            this.BaseLength = @baseLength;
            this.Height = @height;
            }
        
        // Empty constructor
        public Plant()
            : base()
        {
        }
    
        /// <summary>Width of the plant base</summary>
        [JsonProperty("Base Width", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double BaseWidth { get; set; }
    
        /// <summary>Length of the plant base.</summary>
        [JsonProperty("Base Length", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double BaseLength { get; set; }
    
        /// <summary>Height of the plant</summary>
        [JsonProperty("Height", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Height { get; set; }
    
    
    }
}