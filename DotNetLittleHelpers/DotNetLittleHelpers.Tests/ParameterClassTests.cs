using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Shouldly;

namespace DotNetLittleHelpers.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class ParameterClassTests
    {

        public class PersonParamSet : ParameterSet
        {
            public PersonParamSet()
            {
            }

            public PersonParamSet(string[] args) : base(args)
            {
            }

            public PersonParamSet(string argumentString) : base(argumentString)
            {
            }

            public string Name { get; set; }
            public string LastName { get; set; }

            public string Email { get; set; }

            public int Age { get; set; }
            public int? NullableOne { get; set; }
            public int NullableTwo { get; set; }
            public decimal Weight { get; set; }

            public string Path { get; set; }

            public DateTime RegisteredDate { get; set; }

            public bool Happy { get; set; }

            public bool Drunk { get; set; }
            public bool Rich { get; set; }

            public void SetPrivateInstanceProperty(bool val)
            {
                PrivateInstanceProperty = val;
            }
            public bool PrivateInstanceProperty{ get;private  set; }

           public static string PublicStaticProperty { get; set; }

            public string PublicField;
        }

        static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        static readonly DateTime DateTimeStamp = DateTime.Now;

        readonly string simplePersonParametersInput = $"-Email john@doe.com -Age 34 --happy -RegisteredDate={DateTimeStamp:O} --drunk -name=\"John \"FunnyGuy\" Doe\" -nullableTwo=666 /Path=\"C:\\My Home\\Living-Room\" --weight=123{DecimalSeparator}43 -NonExistentParameter=HahaNothing";

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_Stringify_RoundTrip()
        {
            PersonParamSet person = new PersonParamSet
            {
                Weight = 134.2M, 
                Age = 24,
                NullableOne = 1,
                Drunk = true,
                Email = "jdoe@does.com",
                Path = "Somewhere Over The Rain...forest",
                RegisteredDate = new DateTime(1988,02,28)
            };
            string stringified = person.SaveAsParameters();
            stringified.ShouldNotContain(nameof(ParameterSet.OriginalParameterInputString));
            PersonParamSet destringified = new PersonParamSet(stringified);

            destringified.ThrowIfPublicPropertiesNotEqual(person, ignoreProperties: new []{ nameof(PersonParamSet.OriginalParameterInputString), nameof(PersonParamSet.OriginalParameterCollection) });
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_Stringify_RoundTrip_OnePropOnly_TestIgnorable()
        {
            PersonParamSet person = new PersonParamSet
            {
                Name = "John \"FunnyGuy\" Doe"
            };
            person.PublicField = "PublicField Value";
            PersonParamSet.PublicStaticProperty = "PublicStaticProperty Value";
            person.SetPrivateInstanceProperty(true);

            string stringified = person.SaveAsParameters();
            stringified.ShouldNotContain(nameof(ParameterSet.OriginalParameterInputString));

            PersonParamSet destringified = new PersonParamSet(stringified);

            destringified.ShouldNotBeSameAs(person);
            destringified.Name.ShouldBe("John \"FunnyGuy\" Doe");
            destringified.PublicField.ShouldNotBe(person.PublicField);
            destringified.PrivateInstanceProperty.ShouldNotBe(person.PrivateInstanceProperty);

            stringified.ShouldBe("--Name=\"John \"FunnyGuy\" Doe\" ");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_Stringify_RoundTrip_OnePropOnly()
        {
            PersonParamSet person = new PersonParamSet
            {
                Name = "John \"FunnyGuy\" Doe"
            };

            string stringified = person.SaveAsParameters();

            PersonParamSet destringified = new PersonParamSet(stringified);
            destringified.ThrowIfPublicPropertiesNotEqual(person, ignoreProperties: new []{ nameof(PersonParamSet.OriginalParameterInputString), nameof(PersonParamSet.OriginalParameterCollection) });
            stringified.ShouldBe("--Name=\"John \"FunnyGuy\" Doe\" ");
        }


        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_GetParameterCollection()
        {
            PersonParamSet person = new PersonParamSet
            {
                Name = "John \"FunnyGuy\" Doe",
                Happy = true,
                NullableTwo = 666
                
            };

            string stringified = person.SaveAsParameters();
            PersonParamSet destringified = new PersonParamSet(stringified);

            var onlyExplicitCollection = destringified.GetParameterCollection(true, true);

            onlyExplicitCollection.ShouldSatisfyAllConditions(
                ()=> onlyExplicitCollection.Count.ShouldBe(3),
                () => onlyExplicitCollection.ShouldContain(new KeyValuePair<string, string>("Name", "John \"FunnyGuy\" Doe")),
                () => onlyExplicitCollection.ShouldContain(new KeyValuePair<string, string>("Happy", "True")),
                () => onlyExplicitCollection.ShouldContain(new KeyValuePair<string, string>("NullableTwo", "666"))


            );

            var allParams = destringified.GetParameterCollection();

            allParams.ShouldSatisfyAllConditions(
                () => allParams.Count.ShouldBe(12),
                
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Email", null)),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Name", "John \"FunnyGuy\" Doe")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("LastName", null)),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Path", null)),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Age", "0")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Happy", "True")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Drunk", "False")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Rich", "False")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("Weight", "0")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("NullableTwo","666")),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("NullableOne",null)),
                () => allParams.ShouldContain(new KeyValuePair<string, string>("RegisteredDate", "0001-01-01T00:00:00.0000000"))
            );


        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_CheckParameterSet()
        {
            string[] args = ParameterSet.Parser.Split("-Email john@doe.com -Age 34 --happy");

            PersonParamSet person1 = new PersonParamSet();
            person1.LoadParameters(args);
            person1.OriginalParameterCollection.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Email", "john@doe.com"),
                    new KeyValuePair<string, string>("Age", "34"),
                    new KeyValuePair<string, string>("happy", "True"),
                }
            );


            PersonParamSet person2 = new PersonParamSet(args);
            person2.OriginalParameterCollection.ShouldBe(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Email", "john@doe.com"),
                    new KeyValuePair<string, string>("Age", "34"),
                    new KeyValuePair<string, string>("happy", "True"),

                }
            );
        }



        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_LoadMethod()
        {
            string[] args = ParameterSet.Parser.Split(this.simplePersonParametersInput);

            PersonParamSet person = new PersonParamSet();
            person.LoadParameters(args);
            AssertSimplePerson(person);

            person = new PersonParamSet();
            person.LoadParameters(this.simplePersonParametersInput);
            AssertSimplePerson(person);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_Constructor()
        {
            string[] args = ParameterSet.Parser.Split(this.simplePersonParametersInput);

            PersonParamSet person = new PersonParamSet(args);
            person.OriginalParameterInputString.ShouldBe(string.Join(" ", args));

            AssertSimplePerson(person);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_Constructor_FromString()
        {
            PersonParamSet person = new PersonParamSet(this.simplePersonParametersInput);
            
            person.OriginalParameterInputString.ShouldBe(this.simplePersonParametersInput);

            AssertSimplePerson(person);
        }

        private static void AssertSimplePerson(PersonParamSet person)
        {
            person.ShouldSatisfyAllConditions(
                () => person.Email.ShouldBe("john@doe.com"),
                () => person.Age.ShouldBe(34),
                () => person.Happy.ShouldBe(true),
                () => person.RegisteredDate.ShouldBe(DateTimeStamp),
                () => person.Drunk.ShouldBe(true),
                () => person.Name.ShouldBe("John \"FunnyGuy\" Doe"),
                () => person.Path.ShouldBe(@"C:\My Home\Living-Room"),
                () => person.NullableTwo.ShouldBe(666),
                () => person.Rich.ShouldBe(false),
                () => person.LastName.ShouldBe(null),
                () => person.Weight.ShouldBe(123.43M),
                () => person.NullableOne.ShouldBe(null)
            );
        }
    }
}