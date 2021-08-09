
// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar test generate'.
// DO NOT EDIT THIS FILE.

using Elements;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Elements.Serialization.glTF;
using System.Linq;

namespace PantryLayout
{
    public class PantryLayoutTest2
    {
        [Fact]
        public void TestExecute()
        {
            var input = GetInput();

            var modelDependencies = new Dictionary<string, Model> {
                {"Levels", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/LayoutFunctions/PantryLayout/test/Generated/PantryLayoutTest/model_dependencies/Levels/model.json")) },
                {"Space Planning Zones", Model.FromJson(File.ReadAllText(@"/Users/andrewheumann/Dev/HyparSpace/LayoutFunctions/PantryLayout/test/Generated/PantryLayoutTest/model_dependencies/Space Planning Zones/model.json")) },
            };

            var input2 = new PantryLayoutInputs(null, null, null, null, null, null, null);
            var result2 = PantryLayout.Execute(modelDependencies, input2);
            File.WriteAllText("../../../Generated/PantryLayoutTest/results/PantryLayoutTest_wo_overrides.json", result2.Model.ToJson());

            var elementInstanceCount2 = result2.Model.AllElementsOfType<ElementInstance>().Count();

            var result1 = PantryLayout.Execute(modelDependencies, input);
            File.WriteAllText("../../../Generated/PantryLayoutTest/results/PantryLayoutTest_w_overrides.json", result1.Model.ToJson());
            var elementInstanceCount1 = result1.Model.AllElementsOfType<ElementInstance>().Count();


            Assert.Equal(elementInstanceCount1, elementInstanceCount2);

        }

        public PantryLayoutInputs GetInput()
        {
            var inputText = @"
            {
  ""model_input_keys"": {
    ""Levels"": ""077330bc-994e-4f97-9077-f3df3355db5d_61dbb9f8-aaae-4295-9112-c8ae81655361_elements.zip"",
    ""Space Planning Zones"": ""aa1ebffe-6c0e-48b6-a4af-6de79a131d8d_09b8407f-6c93-4741-ad6c-31288213f4f7_elements.zip""
  },
  ""overrides"": {
    ""Furniture Locations"": [
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.7317657490894888,
                0.6815562254572248,
                0,
                -7.816076131995981,
                -0.6815562254572248,
                -0.7317657490894888,
                0,
                -11.46328499776061,
                0,
                0,
                1,
                0.30000000000000066
              ]
            }
          },
          ""origin"": {
            ""X"": -7.816076131995981,
            ""Y"": -11.46328499776061,
            ""Z"": 0.30000000000000066
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -5.826834917552739,
            ""Y"": -11.609534011321033,
            ""Z"": 0.3
          }
        },
        ""id"": ""ee4df1d5-64bd-465a-aa26-9c73d1ff9490""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                0.7071067811865567,
                0.7071067811865384,
                0,
                -7.83066240956401,
                -0.7071067811865384,
                0.7071067811865567,
                0,
                -10.801832047815395,
                0,
                0,
                1,
                0.30000000000000066
              ]
            }
          },
          ""origin"": {
            ""X"": -7.83066240956401,
            ""Y"": -10.801832047815395,
            ""Z"": 0.30000000000000066
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -5.841421195120768,
            ""Y"": -10.948081061375818,
            ""Z"": 0.3
          }
        },
        ""id"": ""c1e6bc52-7071-4da4-8e6f-d60be52aae0a""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.6677941973635861,
                -0.7443459612085793,
                0,
                -8.500618773355864,
                0.7443459612085793,
                -0.6677941973635861,
                0,
                -11.448969400896889,
                0,
                0,
                1,
                0.30000000000000066
              ]
            }
          },
          ""origin"": {
            ""X"": -8.500618773355864,
            ""Y"": -11.448969400896889,
            ""Z"": 0.30000000000000066
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -6.511377558912622,
            ""Y"": -11.595218414457312,
            ""Z"": 0.3
          }
        },
        ""id"": ""978707bf-8e02-4916-ae50-af5176d94a78""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                0.7071067811860267,
                -0.7071067811870685,
                0,
                -8.476790014614028,
                0.7071067811870685,
                0.7071067811860267,
                0,
                -10.804700882025339,
                0,
                0,
                1,
                0.30000000000000066
              ]
            }
          },
          ""origin"": {
            ""X"": -8.476790014614028,
            ""Y"": -10.804700882025339,
            ""Z"": 0.30000000000000066
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -6.487548800170787,
            ""Y"": -10.950949895585762,
            ""Z"": 0.3
          }
        },
        ""id"": ""c0cffcd9-e5a7-4547-903f-fcaf09f619cc""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.7327811184057017,
                0.6804644241311143,
                0,
                -7.510741138892701,
                -0.6804644241311143,
                -0.7327811184057017,
                0,
                -11.187536031854114,
                0,
                0,
                1,
                0.30000000000000066
              ]
            }
          },
          ""origin"": {
            ""X"": -7.510741138892701,
            ""Y"": -11.187536031854114,
            ""Z"": 0.30000000000000066
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -5.52149992444946,
            ""Y"": -11.333785045414537,
            ""Z"": 0.3
          }
        },
        ""id"": ""73665661-2837-45db-aa29-bf99188d9860""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.7327811184057017,
                0.6804644241311143,
                0,
                0.5078656678830149,
                -0.6804644241311143,
                -0.7327811184057017,
                0,
                -11.379749452840683,
                0,
                0,
                1,
                0.2999999999999996
              ]
            }
          },
          ""origin"": {
            ""X"": 0.5078656678830149,
            ""Y"": -11.379749452840683,
            ""Z"": 0.2999999999999996
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -3.052308111139263,
            ""Y"": -11.333785045414537,
            ""Z"": 0.3
          }
        },
        ""id"": ""ee5907b2-4b0c-4619-b09b-da47596dca1d""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                0.7071067811860267,
                -0.7071067811870685,
                0,
                -0.45818320783831235,
                0.7071067811870685,
                0.7071067811860267,
                0,
                -10.996914303011907,
                0,
                0,
                1,
                0.2999999999999996
              ]
            }
          },
          ""origin"": {
            ""X"": -0.45818320783831235,
            ""Y"": -10.996914303011907,
            ""Z"": 0.2999999999999996
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -4.01835698686059,
            ""Y"": -10.950949895585762,
            ""Z"": 0.3
          }
        },
        ""id"": ""04f2c731-da9b-45ad-a449-761a36fa8b98""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.6677941973635861,
                -0.7443459612085793,
                0,
                -0.4820119665801479,
                0.7443459612085793,
                -0.6677941973635861,
                0,
                -11.641182821883458,
                0,
                0,
                1,
                0.2999999999999996
              ]
            }
          },
          ""origin"": {
            ""X"": -0.4820119665801479,
            ""Y"": -11.641182821883458,
            ""Z"": 0.2999999999999996
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -4.042185745602426,
            ""Y"": -11.595218414457312,
            ""Z"": 0.3
          }
        },
        ""id"": ""6e65ae14-6703-4c87-9bff-8f24b91eaf9a""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                0.7071067811865567,
                0.7071067811865384,
                0,
                0.18794439721170653,
                -0.7071067811865384,
                0.7071067811865567,
                0,
                -10.994045468801966,
                0,
                0,
                1,
                0.2999999999999996
              ]
            }
          },
          ""origin"": {
            ""X"": 0.18794439721170653,
            ""Y"": -10.994045468801966,
            ""Z"": 0.2999999999999996
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -3.3722293818105715,
            ""Y"": -10.94808106137582,
            ""Z"": 0.3
          }
        },
        ""id"": ""529b8b07-d48c-4404-a0ca-92afe25821b2""
      },
      {
        ""value"": {
          ""Transform"": {
            ""Matrix"": {
              ""Components"": [
                -0.7317657490894888,
                0.6815562254572248,
                0,
                0.20253067477973508,
                -0.6815562254572248,
                -0.7317657490894888,
                0,
                -11.655498418747179,
                0,
                0,
                1,
                0.2999999999999996
              ]
            }
          },
          ""origin"": {
            ""X"": 0.20253067477973508,
            ""Y"": -11.655498418747179,
            ""Z"": 0.2999999999999996
          }
        },
        ""identity"": {
          ""OriginalLocation"": {
            ""X"": -3.357643104242543,
            ""Y"": -11.609534011321033,
            ""Z"": 0.3
          }
        },
        ""id"": ""a4b03722-7ea3-4fac-8b7e-238b0d833c0f""
      }
    ]
  }
}
            ";
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PantryLayoutInputs>(inputText);
        }
    }
}