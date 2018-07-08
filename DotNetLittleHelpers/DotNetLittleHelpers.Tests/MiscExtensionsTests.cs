using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    using System;
    using System.Reflection.Emit;

    [TestClass()]
    public class MiscExtensionsTests
    {

       

        [TestMethod()]
        public void GetOrdinalTest()
        {
            Assert.AreEqual("1st", 1.GetOrdinalNumber());

            Assert.AreEqual("21st", 21.GetOrdinalNumber());

            Assert.AreEqual("2nd", 2.GetOrdinalNumber());

            Assert.AreEqual("323rd", 323.GetOrdinalNumber());

            Assert.AreEqual("56776th", 56776.GetOrdinalNumber());
            Assert.AreEqual("0", 0.GetOrdinalNumber());

        }

        [TestMethod()]
        public void GetOrdinalSuffixTest()
        {
            Assert.AreEqual("", 0.GetOrdinalSuffix());

            Assert.AreEqual("st", 1.GetOrdinalSuffix());
            Assert.AreEqual("th", 11.GetOrdinalSuffix());
            Assert.AreEqual("st", 21.GetOrdinalSuffix());

            Assert.AreEqual("nd", 2.GetOrdinalSuffix());
            Assert.AreEqual("th", 12.GetOrdinalSuffix());
            Assert.AreEqual("nd", 22.GetOrdinalSuffix());
            Assert.AreEqual("nd", 262.GetOrdinalSuffix());

            Assert.AreEqual("rd", 3.GetOrdinalSuffix());
            Assert.AreEqual("th", 13.GetOrdinalSuffix());
            Assert.AreEqual("rd", 23.GetOrdinalSuffix());
            Assert.AreEqual("rd", 323.GetOrdinalSuffix());


            Assert.AreEqual("th", 4.GetOrdinalSuffix());
            Assert.AreEqual("th", 45.GetOrdinalSuffix());
            Assert.AreEqual("th", 16.GetOrdinalSuffix());
            Assert.AreEqual("th", 56776.GetOrdinalSuffix());
        }

        public class NameIdStringTestObject
        {
            public int Id { get; set; }
            public Guid UserGuid { get; set; }
            public string Name { get; set; }
            private string OtherName { get; set; }
            public string IdStartingProp { get; set; }
            public string NameOfSomething { get; set; }
            public string SomethingElse { get; set; }
        }

        public class PropertyStringTestObject
        {
            public decimal Number { get; set; }
            private string OtherName { get; set; } 

            public string Name { get; set; }

            public bool SomeBoolean { get; set; }

#pragma warning disable 169
            private bool backingBoolean;
#pragma warning restore 169

            public Guid Id { get; set; }

            public PropertyStringTestObject NestedObject { get; set; }

        }

        [TestMethod()]
        public void TestGetPropertyString_IsSourceNull()
        {
            PropertyStringTestObject obj = null;
            Assert.IsNull(obj.GetPropertyInfoString("Something"));
            Assert.IsNull(obj.GetPropertyInfoString());
            Assert.IsNull(obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_MissingProperty()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();
            Assert.AreEqual("Name: [*NULL*], SomeRandom: [*NO SUCH PROPERTY*]",
                obj.GetPropertyInfoString(nameof(PropertyStringTestObject.Name), "SomeRandom"));
        }

        [TestMethod()]
        public void TestGetNameIdPropertyString()
        {
            var obj = new NameIdStringTestObject()
            {
                Id = 2,
                IdStartingProp = "Boo",
                Name = "JackDaniels",
                NameOfSomething = "BillyJean",
                UserGuid = Guid.NewGuid(),
                SomethingElse = "Elsewhere"
            };
            Assert.AreEqual($"Id: [2], Name: [JackDaniels], NameOfSomething: [BillyJean], UserGuid: [{obj.UserGuid}]",
                obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetNameIdPropertyString_Defaults()
        {
            var obj = new NameIdStringTestObject();
            Assert.AreEqual("Id: [0], Name: [*NULL*], NameOfSomething: [*NULL*], UserGuid: [00000000-0000-0000-0000-000000000000]",
                obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_HappyPath()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject()
            {
                Number = 3.14M,
                Name = "Jim Beam",
                SomeBoolean = true,
                Id = Guid.NewGuid(),
                NestedObject = new PropertyStringTestObject()
            };

            Assert.AreEqual($"Number: [{obj.Number}], Name: [{obj.Name}], SomeBoolean: [True], Id: [{obj.Id}], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+PropertyStringTestObject]",
                obj.GetPropertyInfoString( nameof(PropertyStringTestObject.Number), nameof(PropertyStringTestObject.Name), nameof(PropertyStringTestObject.SomeBoolean), nameof(PropertyStringTestObject.Id), nameof(PropertyStringTestObject.NestedObject)));

            Assert.AreEqual($"Id: [{obj.Id}], Name: [Jim Beam], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+PropertyStringTestObject], Number: [{obj.Number}], SomeBoolean: [True]",
                obj.GetPropertyInfoString());

          
        }

        [TestMethod()]
        public void TestGetPropertyString_Nulls()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();
            Assert.AreEqual($"Number: [0], Name: [*NULL*], SomeBoolean: [False], Id: [00000000-0000-0000-0000-000000000000], NestedObject: [*NULL*]",
                obj.GetPropertyInfoString(nameof(PropertyStringTestObject.Number), nameof(PropertyStringTestObject.Name), nameof(PropertyStringTestObject.SomeBoolean), nameof(PropertyStringTestObject.Id), nameof(PropertyStringTestObject.NestedObject)));
        }

        [TestMethod()]
        public void TestGetPropertyStringAllProps_Nulls()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();
           
            Assert.AreEqual($"Id: [00000000-0000-0000-0000-000000000000], Name: [*NULL*], NestedObject: [*NULL*], Number: [0], SomeBoolean: [False]",
                obj.GetPropertyInfoString());
        }

        bool Evaluate<T>(Func<T, T, bool> @operator, T arg1, T arg2) where T:IComparable<T>
        {
            return (@operator(arg1, arg2));
        }

        [TestMethod()]
        public void TestComparerOperator()
        {
            var smallerDate = new DateTime(1000, 1, 1);
            var largerDate = new DateTime(1999,9,9);
            Assert.IsTrue(this.Evaluate(ComparisonOperator.Greater<DateTime>(), largerDate, smallerDate));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Greater<DateTime>(), smallerDate, largerDate));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Greater<DateTime>(), smallerDate, largerDate));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(),2,1));
            Assert.IsTrue(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(),2,2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(), 1, 2));


            Assert.IsTrue(this.Evaluate(ComparisonOperator.Equal<int>(), 2, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Equal<int>(), 1, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Equal<int>(), 2, 1));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.Smaller<int>(),1,2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Smaller<int>(),2,1));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Smaller<int>(),2,2));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(), 1, 2));
            Assert.IsTrue(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(), 2, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(),2, 1));
        }
    }
    }