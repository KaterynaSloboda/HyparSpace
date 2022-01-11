
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace Circulation
{
    public class CirculationTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Levels", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/ZonePlanningFunctions/Circulation/test/Generated/CirculationTest/model_dependencies/Levels/8a8be7b6-5f77-4616-aae7-a85486f68f20.json")) }, 
                {"Program Requirements", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/ZonePlanningFunctions/Circulation/test/Generated/CirculationTest/model_dependencies/Program Requirements/4458270a-2a59-4a3c-a789-dedf2c349e58.json")) }, 
                {"Floors", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/ZonePlanningFunctions/Circulation/test/Generated/CirculationTest/model_dependencies/Floors/2241d024-c993-4d5b-98c1-721653b7f94e.json")) }, 
                {"Core", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/ZonePlanningFunctions/Circulation/test/Generated/CirculationTest/model_dependencies/Core/54e0cb90-2a4f-4e03-8f02-7dd002e21fb5.json")) }, 
            };

            var result = Circulation.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/CirculationTest/results/CirculationTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/CirculationTest/results/CirculationTest.glb");
            File.WriteAllText("../../../Generated/CirculationTest/results/CirculationTest.json", result.Model.ToJson());
        }

        public CirculationInputs GetInput()
        {
            var json = File.ReadAllText("../../../Generated/CirculationTest/inputs.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CirculationInputs>(json);
        }
    }
}