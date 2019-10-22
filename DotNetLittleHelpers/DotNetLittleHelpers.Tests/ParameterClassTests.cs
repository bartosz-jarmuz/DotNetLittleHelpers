using System;
using System.Globalization;
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
            var person = new PersonParamSet
            {
                Weight = 134.2M, 
                Age = 24,
                NullableOne = 1,
                Drunk = true,
                Email = "jdoe@does.com",
                Path = "Somewhere Over The Rain...forest",
                RegisteredDate = new DateTime(1988,02,28)
            };
            var stringified = person.SaveAsParameters();
            var destringified = new PersonParamSet(stringified);

            destringified.ThrowIfPublicPropertiesNotEqual(person);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_Stringify_RoundTrip_OnePropOnly_TestIgnorable()
        {
            var person = new PersonParamSet
            {
                Name = "John \"FunnyGuy\" Doe"
            };
            person.PublicField = "PublicField Value";
            PersonParamSet.PublicStaticProperty = "PublicStaticProperty Value";
            person.SetPrivateInstanceProperty(true);

            var stringified = person.SaveAsParameters();
            var destringified = new PersonParamSet(stringified);

            destringified.ShouldNotBeSameAs(person);
            destringified.Name.ShouldBe("John \"FunnyGuy\" Doe");
            destringified.PublicField.ShouldNotBe(person.PublicField);
            destringified.PrivateInstanceProperty.ShouldNotBe(person.PrivateInstanceProperty);

            stringified.ShouldBe("--Name=\"John \"FunnyGuy\" Doe\" ");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void Test_Stringify_RoundTrip_OnePropOnly()
        {
            var person = new PersonParamSet
            {
                Name = "John \"FunnyGuy\" Doe"
            };

            var stringified = person.SaveAsParameters();

            var destringified = new PersonParamSet(stringified);
            destringified.ThrowIfPublicPropertiesNotEqual(person);
            stringified.ShouldBe("--Name=\"John \"FunnyGuy\" Doe\" ");
        }


        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_LoadMethod()
        {
            var args = ParameterSet.Parser.Split(this.simplePersonParametersInput);

            var person = new PersonParamSet();
            person.LoadParameters(args);
            AssertSimplePerson(person);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_Constructor()
        {
            var args = ParameterSet.Parser.Split(this.simplePersonParametersInput);
            var person = new PersonParamSet(args);
            AssertSimplePerson(person);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestLoadSimplePerson_Constructor_FromString()
        {
            var person = new PersonParamSet(this.simplePersonParametersInput);
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