
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;

namespace InteriorPartitions
{
    public class InteriorPartitionsTest
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> { 
                {"Private Office Layout", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Private Office Layout\a7b37cd9-8ec2-4035-bd40-a1b3fbff1524.json")) }, 
                {"Phone Booth Layout", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Phone Booth Layout\55c48eb7-fb0c-42ec-aff7-e9e651d1896a.json")) }, 
                {"Classroom Layout", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Classroom Layout\718f17e9-a4a0-4c0e-9abb-6e9378f58227.json")) }, 
                {"Meeting Room Layout", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Meeting Room Layout\6ed714fe-85c6-4911-aad5-ff4f3c9e48c7.json")) }, 
                {"Space Planning Zones", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Space Planning Zones\5b0ce623-8e60-4d13-928a-b31ea609934e.json")) }, 
                {"Furniture and Equipment", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Furniture and Equipment\27ed00af-dcc3-4d5a-8124-a993ac3eab62.json")) }, 
                {"Doors", Model.FromJson(File.ReadAllText(@"C:\dev\Hypar\HyparSpace\LayoutFunctions\InteriorPartitions\test\Generated\InteriorPartitionsTest\model_dependencies\Doors\ee4fe433-9de6-4899-923e-2a02b4ba2a11.json")) }, 
            };

            var result = InteriorPartitions.Execute(modelDependencies, input);
            result.Model.ToGlTF("../../../Generated/InteriorPartitionsTest/results/InteriorPartitionsTest.gltf", false);
            result.Model.ToGlTF("../../../Generated/InteriorPartitionsTest/results/InteriorPartitionsTest.glb");
            File.WriteAllText("../../../Generated/InteriorPartitionsTest/results/InteriorPartitionsTest.json", result.Model.ToJson());

        }

        public InteriorPartitionsInputs GetInput()
        {
            var json = File.ReadAllText("../../../Generated/InteriorPartitionsTest/inputs.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<InteriorPartitionsInputs>(json);
        }
    }
}