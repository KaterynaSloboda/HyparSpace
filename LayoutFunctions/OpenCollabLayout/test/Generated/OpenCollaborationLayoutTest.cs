
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace OpenCollaborationLayout
{
    public class OpenCollaborationLayoutTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Levels", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/LayoutFunctions/OpenCollabLayout/test/Generated/OpenCollaborationLayoutTest/model_dependencies/Levels/model.json")) }, 
                {"Space Planning Zones", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/LayoutFunctions/OpenCollabLayout/test/Generated/OpenCollaborationLayoutTest/model_dependencies/Space Planning Zones/model.json")) }, 
                {"Core", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/LayoutFunctions/OpenCollabLayout/test/Generated/OpenCollaborationLayoutTest/model_dependencies/Core/model.json")) }, 
            };

            var result = OpenCollaborationLayout.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/OpenCollaborationLayoutTest/results/OpenCollaborationLayoutTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/OpenCollaborationLayoutTest/results/OpenCollaborationLayoutTest.glb");
            File.WriteAllText("../../../Generated/OpenCollaborationLayoutTest/results/OpenCollaborationLayoutTest.json", result.Model.ToJson());
        }

        public OpenCollaborationLayoutInputs GetInput()
        {
            var inputText = @"
            {
  ""model_input_keys"": {
    ""Levels"": ""8921cfe7-dd29-4224-a287-e4d90aa3182b_61dbb9f8-aaae-4295-9112-c8ae81655361_elements.zip"",
    ""Space Planning Zones"": ""069f3e97-3af6-43e6-825e-784169305c60_09b8407f-6c93-4741-ad6c-31288213f4f7_elements.zip"",
    ""Core"": ""b81534e9-16e9-420b-8f9c-5fc687dac21a_a9cac5a1-f68d-4d2e-bfdd-0d204359bbe4_elements.zip""
  }
}
            ";
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OpenCollaborationLayoutInputs>(inputText);
        }
    }
}