using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Shouldly;

namespace DotNetLittleHelpers.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class ParameterStringExtensionsTests
    {
      


        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestSplittingWorks()
        {
            string input = "-name Bartosz -lastname Jarmuż";
            string[] output = ParameterSet.Parser.Split(input);
            output.ShouldBe(new[] {"-name", "Bartosz", "-lastname", "Jarmuż"});

            input = "test --file=\"some file.txt\"";
            output = ParameterSet.Parser.Split(input);
            output.ShouldBe(new[] { "test", "--file=\"some file.txt\"" });

            input = @"/src:""C:\tmp\Some Folder\Sub Folder"" /users:""abcdefg@hijkl.com"" tasks:""SomeTask,Some Other Task"" -someParam";
            output = ParameterSet.Parser.Split(input);
            output.ShouldBe(new[] { @"/src:""C:\tmp\Some Folder\Sub Folder""", @"/users:""abcdefg@hijkl.com""", @"tasks:""SomeTask,Some Other Task""", "-someParam" });


            input = "-name=Bartosz -lastname Jarmuż -emptySpace \" \"";
            output = ParameterSet.Parser.Split(input);
            output.ShouldBe(new[] { "-name=Bartosz", "-lastname", "Jarmuż", "-emptySpace", " " });
           
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestSplittingWorks_Empty()
        {
            var output = ParameterSet.Parser.Split("");
            output.ShouldBe(new string[] { });

            output = ParameterSet.Parser.Split(null);
            output.ShouldBe(new string[] { });
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestGettingParamsDictionary_Empty()
        {
            var output = ParameterSet.Parser.GetParameters(args: null);
            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {

                }
            );
            output = ParameterSet.Parser.GetParameters(new string[] { });
            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {

                }
            );
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestGettingParamsDictionary_AnonymousParams()
        {
            var input = new[] { @"someArg", "someOtherArg" };
            var output = ParameterSet.Parser.GetParameters(input);

            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("", "someArg"),
                    new KeyValuePair<string, string>("", "someOtherArg"),
                }
            );
            var output2 = ParameterSet.Parser.GetParameters(ParameterSet.Parser.Split(string.Join(" ", input)));
            output2.ShouldBe(output);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestGettingParamsDictionary_OnlySwitches()
        {
            var input = new[] { @"-someSwitch", "-someOtherSwitch" };
           var  output = ParameterSet.Parser.GetParameters(input);

            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("someSwitch", "True"),
                    new KeyValuePair<string, string>("someOtherSwitch", "True"),
                }
            );
            var jointInputString = string.Join(" ", input);
            var output2 = ParameterSet.Parser.GetParameters(ParameterSet.Parser.Split(jointInputString));
            output2.ShouldBe(output);
            var output3 = ParameterSet.Parser.GetParameters(jointInputString);
            output3.ShouldBe(output);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestGettingParamsDictionary_AllSortsOfComplexities()
        {
            string[] input = new[] { "-name=Bartosz", "-lastname", "Jarmuż", "-emptySpace", "\" \"" };
            var output = ParameterSet.Parser.GetParameters(input);

            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("name", "Bartosz"),
                    new KeyValuePair<string, string>("lastname", "Jarmuż"),
                    new KeyValuePair<string, string>("emptySpace", " "),
                }
            );
            var jointInputString = string.Join(" ", input);
            var output2 = ParameterSet.Parser.GetParameters(ParameterSet.Parser.Split(jointInputString));
            output2.ShouldBe(output);
            var output3 = ParameterSet.Parser.GetParameters(jointInputString);
            output3.ShouldBe(output);

            input = new[] { "SomePlainValue",
                @"-src:""C:\tmp\Some Folder\Sub Folder""",
                "--someBoolean",
                @"/users:""abcdefg@hijkl.com""",
                @"--moreWithSpaces:""There are ""slashes"" /spaces and -hyphens here""",
                "AnotherPlainValue",
                "-xmlChunk=\"<ApiResponse Id=\"23\">Error</ApiResponse>\"",
                @"--tasks:""SomeTask,Some Other Task""",
                "-someParam" };
            output = ParameterSet.Parser.GetParameters(input);
          

            output.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("", @"SomePlainValue"),
                    new KeyValuePair<string, string>("src", @"C:\tmp\Some Folder\Sub Folder"),
                    new KeyValuePair<string, string>("someBoolean", @"True"),
                    new KeyValuePair<string, string>("users", "abcdefg@hijkl.com"),
                    new KeyValuePair<string, string>("moreWithSpaces", "There are \"slashes\" /spaces and -hyphens here"),
                    new KeyValuePair<string, string>("", "AnotherPlainValue"),
                    new KeyValuePair<string, string>("xmlChunk", "<ApiResponse Id=\"23\">Error</ApiResponse>"),
                    new KeyValuePair<string, string>("tasks", "SomeTask,Some Other Task"),
                    new KeyValuePair<string, string>("someParam", "True"),
                }
            );

            jointInputString = string.Join(" ", input);
            output2 = ParameterSet.Parser.GetParameters(ParameterSet.Parser.Split(jointInputString));
            output2.ShouldBe(output);
            output3 = ParameterSet.Parser.GetParameters(jointInputString);
            output3.ShouldBe(output);
        }

    }
}

