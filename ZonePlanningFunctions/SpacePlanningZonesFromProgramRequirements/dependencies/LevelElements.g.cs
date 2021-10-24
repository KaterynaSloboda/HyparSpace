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

    /// <summary>An element containing a collection of Elements that belong to one level.</summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class LevelElements : Element
    {
        [Newtonsoft.Json.JsonConstructor]
        public LevelElements(IList<Element> @elements, System.Guid @id = default, string @name = null)
            : base(id, name)
        {
            this.Elements = @elements;
        }

        // Empty constructor
        public LevelElements()
            : base()
        {
        }

        /// <summary>The list of elements.</summary>
        [Newtonsoft.Json.JsonProperty("Elements", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<Element> Elements { get; set; }


    }
}