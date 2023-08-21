
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace ReceptionLayout
{
    public class ReceptionLayoutTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Space Planning Zones", Model.FromJson(File.ReadAllText(@"D:\Hypar\Forks\HyparSpace\LayoutFunctions\ReceptionLayout\test\Generated\ReceptionLayoutTest\model_dependencies\Space Planning Zones\facac8c4-88d1-49b0-8a9c-05b77ba62a57.json")) }, 
                {"Levels", Model.FromJson(File.ReadAllText(@"D:\Hypar\Forks\HyparSpace\LayoutFunctions\ReceptionLayout\test\Generated\ReceptionLayoutTest\model_dependencies\Levels\723250f8-9ea2-4e23-812b-39a8eb69862e.json")) }, 
            };

            var result = ReceptionLayout.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/ReceptionLayoutTest/results/ReceptionLayoutTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/ReceptionLayoutTest/results/ReceptionLayoutTest.glb");
            File.WriteAllText("../../../Generated/ReceptionLayoutTest/results/ReceptionLayoutTest.json", result.Model.ToJson());

        }

        public ReceptionLayoutInputs GetInput()
        {
            var json = File.ReadAllText("../../../Generated/ReceptionLayoutTest/inputs.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ReceptionLayoutInputs>(json);
        }
    }
}