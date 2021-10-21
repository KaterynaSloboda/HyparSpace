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
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace Elements
{
    #pragma warning disable // Disable all warnings

    /// <summary>A profile with a program assigned to it, and optional internal cell geometry. </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class SpaceBoundary : GeometricElement
    {
        [Newtonsoft.Json.JsonConstructor]
        public SpaceBoundary(Profile @boundary, IList<Polygon> @cells, double @area, double? @length, double? @depth, Transform @transform = null, Material @material = null, Representation @representation = null, bool @isElementDefinition = false, System.Guid @id = default, string @name = null)
            : base(transform, material, representation, isElementDefinition, id, name)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SpaceBoundary>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @boundary, @cells, @transform, @material, @representation, @isElementDefinition, @id, @name});
            }
        
            this.Boundary = @boundary;
            this.Cells = @cells;
            
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>The boundary of the space</summary>
        [Newtonsoft.Json.JsonProperty("Boundary", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Profile Boundary { get; set; }
    
        /// <summary>Component cells making up the boundary</summary>
        [Newtonsoft.Json.JsonProperty("Cells", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<Polygon> Cells { get; set; }
    
        /// <summary>The area of the boundary</summary>
        [Newtonsoft.Json.JsonProperty("Area", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Area { get; set; }
    
        /// <summary>The rough length of this space boundary, parallel to the accessible edge</summary>
        [Newtonsoft.Json.JsonProperty("Length", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Length { get; set; }
    
        /// <summary>The rough depth of the space boundary, perpendicular to the accessible edge</summary>
        [Newtonsoft.Json.JsonProperty("Depth", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Depth { get; set; }
    
    
    }
}