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

namespace DataHallLayout
{
    public class DataHallLayoutOutputs: ResultsBase
    {
		/// <summary>
		/// The number of server cabinets.
		/// </summary>
		[JsonProperty("Rack Count")]
		public double RackCount {get; set;}

		/// <summary>
		/// Watts per square foot
		/// </summary>
		[JsonProperty("Watts/sf")]
		public string Wattssf {get; set;}



        /// <summary>
        /// Construct a DataHallLayoutOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public DataHallLayoutOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a DataHallLayoutOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public DataHallLayoutOutputs(double rackCount, string wattssf): base()
        {
			this.RackCount = rackCount;
			this.Wattssf = wattssf;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}