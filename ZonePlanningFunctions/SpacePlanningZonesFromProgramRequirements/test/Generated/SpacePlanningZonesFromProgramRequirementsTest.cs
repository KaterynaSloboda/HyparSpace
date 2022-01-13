
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace SpacePlanningZonesFromProgramRequirements
{
    public class SpacePlanningZonesFromProgramRequirementsTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Program Requirements", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/ZonePlanningFunctions/SpacePlanningZonesFromProgramRequirements/test/Generated/SpacePlanningZonesFromProgramRequirementsTest/model_dependencies/Program Requirements/f346e6d0-ef5e-472b-8f5d-42b5f5a55e39.json")) }, 
            };

            var result = SpacePlanningZonesFromProgramRequirements.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/SpacePlanningZonesFromProgramRequirementsTest/results/SpacePlanningZonesFromProgramRequirementsTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/SpacePlanningZonesFromProgramRequirementsTest/results/SpacePlanningZonesFromProgramRequirementsTest.glb");
            File.WriteAllText("../../../Generated/SpacePlanningZonesFromProgramRequirementsTest/results/SpacePlanningZonesFromProgramRequirementsTest.json", result.Model.ToJson());
        }

        public SpacePlanningZonesFromProgramRequirementsInputs GetInput()
        {
            var json = File.ReadAllText("../../../Generated/SpacePlanningZonesFromProgramRequirementsTest/inputs.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SpacePlanningZonesFromProgramRequirementsInputs>(json);
        }
    }
}