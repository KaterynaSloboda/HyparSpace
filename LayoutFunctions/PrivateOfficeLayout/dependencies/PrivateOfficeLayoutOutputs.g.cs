// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace PrivateOfficeLayout
{
    public class PrivateOfficeLayoutOutputs: ResultsBase
    {
		/// <summary>
		/// The number of private offices.
		/// </summary>
		[JsonProperty("Private offices count")]
		public double PrivateOfficesCount {get; set;}



        /// <summary>
        /// Construct a PrivateOfficeLayoutOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public PrivateOfficeLayoutOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a PrivateOfficeLayoutOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public PrivateOfficeLayoutOutputs(double privateOfficesCount): base()
        {
			this.PrivateOfficesCount = privateOfficesCount;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}