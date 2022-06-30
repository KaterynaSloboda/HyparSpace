//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.1.21.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
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

    /// <summary>Adjust the settings for labels attached to an object by adding a LabelConfiguration property.</summary>
    [JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class LabelConfiguration 
    {
        [JsonConstructor]
        public LabelConfiguration(Color @color, Vector3 @offset, string @customText, string @forceVisible, Vector3 @direction)
        {
            this.Color = @color;
            this.Offset = @offset;
            this.CustomText = @customText;
            this.ForceVisible = @forceVisible;
            this.Direction = @direction;
            }
        
        // Empty constructor
        public LabelConfiguration()
        {
        }
    
        /// <summary>The label background color</summary>
        [JsonProperty("Color", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Color Color { get; set; }
    
        /// <summary>A screen-space offset, in pixels, for the label from the object's bounding box center.</summary>
        [JsonProperty("Offset", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 Offset { get; set; }
    
        /// <summary>A custom format string for the label contents. This will override the selection of any specific Label property.</summary>
        [JsonProperty("Custom Text", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CustomText { get; set; }
    
        /// <summary>If this value is set, then the label will always be visible, regardless of the function's label settings. The content of this property should be the name of the object property you want to render. Specify "Custom" to use the template defined in Custom Text.</summary>
        [JsonProperty("Force Visible", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForceVisible { get; set; }
    
        /// <summary>The label's world-space orientation which will be projected to screen space.</summary>
        [JsonProperty("Direction", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 Direction { get; set; }
    
    
    }
}