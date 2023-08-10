
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace PrivateOfficeLayout
{
    public class PrivateOfficeLayoutTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Space Planning Zones", Model.FromJson(File.ReadAllText(@"D:\Hypar\Forks\HyparSpace\LayoutFunctions\PrivateOfficeLayout\test\Generated\PrivateOfficeLayoutTest\model_dependencies\Space Planning Zones\67ea11f8-5ba9-4147-9d7b-ebd05962ed28.json")) }, 
                {"Levels", Model.FromJson(File.ReadAllText(@"D:\Hypar\Forks\HyparSpace\LayoutFunctions\PrivateOfficeLayout\test\Generated\PrivateOfficeLayoutTest\model_dependencies\Levels\5b7f0dd5-506e-4962-b209-2e244b912c9f.json")) }, 
                {"Circulation", Model.FromJson(File.ReadAllText(@"D:\Hypar\Forks\HyparSpace\LayoutFunctions\PrivateOfficeLayout\test\Generated\PrivateOfficeLayoutTest\model_dependencies\Circulation\eb2ebab9-f213-4927-a13d-df87c7565d09.json")) }, 
            };

            var result = PrivateOfficeLayout.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/PrivateOfficeLayoutTest/results/PrivateOfficeLayoutTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/PrivateOfficeLayoutTest/results/PrivateOfficeLayoutTest.glb");
            File.WriteAllText("../../../Generated/PrivateOfficeLayoutTest/results/PrivateOfficeLayoutTest.json", result.Model.ToJson());

        }

        public PrivateOfficeLayoutInputs GetInput()
        {
            var json = File.ReadAllText("../../../Generated/PrivateOfficeLayoutTest/inputs.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PrivateOfficeLayoutInputs>(json);
        }
    }
}