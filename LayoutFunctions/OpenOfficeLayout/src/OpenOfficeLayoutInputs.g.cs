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

namespace OpenOfficeLayout
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public  class OpenOfficeLayoutInputs : S3Args
    
    {
        [Newtonsoft.Json.JsonConstructor]
        
        public OpenOfficeLayoutInputs(double @gridRotation, double @integratedCollaborationSpaceDensity, OpenOfficeLayoutInputsDeskType @deskType, Overrides @overrides, string bucketName, string uploadsBucket, Dictionary<string, string> modelInputKeys, string gltfKey, string elementsKey, string ifcKey):
        base(bucketName, uploadsBucket, modelInputKeys, gltfKey, elementsKey, ifcKey)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<OpenOfficeLayoutInputs>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @gridRotation, @integratedCollaborationSpaceDensity, @deskType, @overrides});
            }
        
            this.GridRotation = @gridRotation;
            this.IntegratedCollaborationSpaceDensity = @integratedCollaborationSpaceDensity;
            this.DeskType = @deskType;
            this.Overrides = @overrides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Grid Rotation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0D, 360D)]
        public double GridRotation { get; set; } = 0D;
    
        /// <summary>Increase this number to add more distributed collaboration spaces throughout open office areas.</summary>
        [Newtonsoft.Json.JsonProperty("Integrated Collaboration Space Density", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0D, 1D)]
        public double IntegratedCollaborationSpaceDensity { get; set; } = 0.2D;
    
        [Newtonsoft.Json.JsonProperty("Desk Type", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public OpenOfficeLayoutInputsDeskType DeskType { get; set; } = OpenOfficeLayoutInputsDeskType.Simple_Desk__29x70;
    
        [Newtonsoft.Json.JsonProperty("overrides", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Overrides Overrides { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum OpenOfficeLayoutInputsDeskType
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 24x48")]
        Simple_Desk__24x48 = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 30x60")]
        Simple_Desk__30x60 = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 29x70")]
        Simple_Desk__29x70 = 2,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 30x72")]
        Simple_Desk__30x72 = 3,
    
        [System.Runtime.Serialization.EnumMember(Value = @"L-Shaped")]
        LShaped = 4,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Double Desk")]
        Double_Desk = 5,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Enclosed Pair")]
        Enclosed_Pair = 6,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class Overrides 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public Overrides(IList<SpaceSettingsOverride> @spaceSettings, IList<FurnitureLocationsOverride> @furnitureLocations)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<Overrides>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @spaceSettings, @furnitureLocations});
            }
        
            this.SpaceSettings = @spaceSettings;
            this.FurnitureLocations = @furnitureLocations;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Space Settings", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<SpaceSettingsOverride> SpaceSettings { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Furniture Locations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<FurnitureLocationsOverride> FurnitureLocations { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class SpaceSettingsOverride 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public SpaceSettingsOverride(string @id, SpaceSettingsIdentity @identity, SpaceSettingsValue @value)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SpaceSettingsOverride>();
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
        public SpaceSettingsIdentity Identity { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public SpaceSettingsValue Value { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class FurnitureLocationsOverride 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public FurnitureLocationsOverride(string @id, FurnitureLocationsIdentity @identity, FurnitureLocationsValue @value)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<FurnitureLocationsOverride>();
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
        public FurnitureLocationsIdentity Identity { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FurnitureLocationsValue Value { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class SpaceSettingsIdentity 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public SpaceSettingsIdentity(Vector3 @parentCentroid)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SpaceSettingsIdentity>();
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
    
        [Newtonsoft.Json.JsonProperty("ParentCentroid", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 ParentCentroid { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class SpaceSettingsValue 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public SpaceSettingsValue(double @gridRotation, double @integratedCollaborationSpaceDensity, SpaceSettingsValueDeskType @deskType)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<SpaceSettingsValue>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @gridRotation, @integratedCollaborationSpaceDensity, @deskType});
            }
        
            this.GridRotation = @gridRotation;
            this.IntegratedCollaborationSpaceDensity = @integratedCollaborationSpaceDensity;
            this.DeskType = @deskType;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Grid Rotation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0D, 360D)]
        public double GridRotation { get; set; } = 0D;
    
        /// <summary>Increase this number to add more distributed collaboration spaces throughout open office areas.</summary>
        [Newtonsoft.Json.JsonProperty("Integrated Collaboration Space Density", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0D, 1D)]
        public double IntegratedCollaborationSpaceDensity { get; set; } = 0.2D;
    
        [Newtonsoft.Json.JsonProperty("Desk Type", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SpaceSettingsValueDeskType DeskType { get; set; } = SpaceSettingsValueDeskType.Simple_Desk__29x70;
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class FurnitureLocationsIdentity 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public FurnitureLocationsIdentity(Vector3 @originalLocation)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<FurnitureLocationsIdentity>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @originalLocation});
            }
        
            this.OriginalLocation = @originalLocation;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("OriginalLocation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Vector3 OriginalLocation { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    
    public partial class FurnitureLocationsValue 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public FurnitureLocationsValue(Transform @transform)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<FurnitureLocationsValue>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @transform});
            }
        
            this.Transform = @transform;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Transform", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Transform Transform { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum SpaceSettingsValueDeskType
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 24x48")]
        Simple_Desk__24x48 = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 30x60")]
        Simple_Desk__30x60 = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 29x70")]
        Simple_Desk__29x70 = 2,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Simple Desk - 30x72")]
        Simple_Desk__30x72 = 3,
    
        [System.Runtime.Serialization.EnumMember(Value = @"L-Shaped")]
        LShaped = 4,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Double Desk")]
        Double_Desk = 5,
    
        [System.Runtime.Serialization.EnumMember(Value = @"Enclosed Pair")]
        Enclosed_Pair = 6,
    
    }
}