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

        public class TestObject
        {
            public decimal Number { get; set; }

            public string Name { get; set; }

            public bool SomeBoolean { get; set; }

#pragma warning disable 169
            private bool backingBoolean;
#pragma warning restore 169

            public Guid Id { get; set; }

            public TestObject NestedObject { get; set; }

        }

        [TestMethod()]
        public void TestGetPropertyString_IsSourceNull()
        {
            TestObject obj = null;
            Assert.IsNull(obj.GetPropertyInfoString("Something"));
            Assert.IsNull(obj.GetPropertyInfoString());
        }

        [TestMethod()]
        public void TestGetPropertyString_MissingProperty()
        {
            TestObject obj = new TestObject();
            Assert.AreEqual("Name: [*NULL*], SomeRandom: [*NO SUCH PROPERTY*]",
                obj.GetPropertyInfoString(nameof(TestObject.Name), "SomeRandom"));
        }

        [TestMethod()]
        public void TestGetPropertyString_HappyPath()
        {
            TestObject obj = new TestObject()
            {
                Number = 3.14M,
                Name = "Jim Beam",
                SomeBoolean = true,
                Id = Guid.NewGuid(),
                NestedObject = new TestObject()
            };

            Assert.AreEqual($"Number: [{obj.Number}], Name: [{obj.Name}], SomeBoolean: [True], Id: [{obj.Id}], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+TestObject]",
                obj.GetPropertyInfoString( nameof(TestObject.Number), nameof(TestObject.Name), nameof(TestObject.SomeBoolean), nameof(TestObject.Id), nameof(TestObject.NestedObject)));

            Assert.AreEqual($"Number: [{obj.Number}], Name: [{obj.Name}], SomeBoolean: [True], Id: [{obj.Id}], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+TestObject]",
                obj.GetPropertyInfoString());

          
        }

        [TestMethod()]
        public void TestGetPropertyString_Nulls()
        {
            TestObject obj = new TestObject();
            Assert.AreEqual($"Number: [0], Name: [*NULL*], SomeBoolean: [False], Id: [00000000-0000-0000-0000-000000000000], NestedObject: [*NULL*]",
                obj.GetPropertyInfoString(nameof(TestObject.Number), nameof(TestObject.Name), nameof(TestObject.SomeBoolean), nameof(TestObject.Id), nameof(TestObject.NestedObject)));


            Assert.AreEqual($"Name: [*NULL*], Number: [0], SomeBoolean: [False], Id: [00000000-0000-0000-0000-000000000000], NestedObject: [*NULL*]",
                obj.GetPropertyInfoString()); 
        }

    }
    }