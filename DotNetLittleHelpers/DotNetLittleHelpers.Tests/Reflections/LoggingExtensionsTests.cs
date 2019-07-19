using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass]
    public class LoggingExtensionsTests
    {
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
        public void TestGetPropertyString_IsSourceNull1213458744()
        {
            PropertyStringTestObject obj = null;
            Assert.IsNull(obj.GetPropertyInfoString("Something"));
            Assert.IsNull(obj.GetPropertyInfoString());
            Assert.IsNull(obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_MissingProperty1985170568()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();
            Assert.AreEqual("Name: [*NULL*], SomeRandom: [*NO SUCH PROPERTY*]",
                obj.GetPropertyInfoString(nameof(PropertyStringTestObject.Name), "SomeRandom"));
        }

        [TestMethod()]
        public void TestGetNameIdPropertyString1626648706()
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
        public void TestGetNameIdPropertyString_Defaults1997483625()
        {
            var obj = new NameIdStringTestObject();
            Assert.AreEqual("Id: [0], Name: [*NULL*], NameOfSomething: [*NULL*], UserGuid: [00000000-0000-0000-0000-000000000000]",
                obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_HappyPath2079862639()
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
                obj.GetPropertyInfoString(nameof(PropertyStringTestObject.Number), nameof(PropertyStringTestObject.Name), nameof(PropertyStringTestObject.SomeBoolean), nameof(PropertyStringTestObject.Id), nameof(PropertyStringTestObject.NestedObject)));

            Assert.AreEqual($"Id: [{obj.Id}], Name: [Jim Beam], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+PropertyStringTestObject], Number: [{obj.Number}], SomeBoolean: [True]",
                obj.GetPropertyInfoString());


        }

        [TestMethod()]
        public void TestGetPropertyString_Nulls312297160()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();
            Assert.AreEqual($"Number: [0], Name: [*NULL*], SomeBoolean: [False], Id: [00000000-0000-0000-0000-000000000000], NestedObject: [*NULL*]",
                obj.GetPropertyInfoString(nameof(PropertyStringTestObject.Number), nameof(PropertyStringTestObject.Name), nameof(PropertyStringTestObject.SomeBoolean), nameof(PropertyStringTestObject.Id), nameof(PropertyStringTestObject.NestedObject)));
        }

        [TestMethod()]
        public void TestGetPropertyStringAllProps_Nulls367708067()
        {
            PropertyStringTestObject obj = new PropertyStringTestObject();

            Assert.AreEqual($"Id: [00000000-0000-0000-0000-000000000000], Name: [*NULL*], NestedObject: [*NULL*], Number: [0], SomeBoolean: [False]",
                obj.GetPropertyInfoString());
        }


        [TestMethod()]
        public void TestGetPropertyString_IsSourceNull()
        {
            LoggingExtensionsTests.PropertyStringTestObject obj = null;
            Assert.IsNull(obj.GetPropertyInfoString("Something"));
            Assert.IsNull(obj.GetPropertyInfoString());
            Assert.IsNull(obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_MissingProperty()
        {
            LoggingExtensionsTests.PropertyStringTestObject obj = new LoggingExtensionsTests.PropertyStringTestObject();
            Assert.AreEqual("Name: [*NULL*], SomeRandom: [*NO SUCH PROPERTY*]",
                obj.GetPropertyInfoString(nameof(LoggingExtensionsTests.PropertyStringTestObject.Name), "SomeRandom"));
        }

        [TestMethod()]
        public void TestGetNameIdPropertyString()
        {
            var obj = new LoggingExtensionsTests.NameIdStringTestObject()
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
            var obj = new LoggingExtensionsTests.NameIdStringTestObject();
            Assert.AreEqual("Id: [0], Name: [*NULL*], NameOfSomething: [*NULL*], UserGuid: [00000000-0000-0000-0000-000000000000]",
                obj.GetNameAndIdString());
        }

        [TestMethod()]
        public void TestGetPropertyString_HappyPath()
        {
            LoggingExtensionsTests.PropertyStringTestObject obj = new LoggingExtensionsTests.PropertyStringTestObject()
            {
                Number = 3.14M,
                Name = "Jim Beam",
                SomeBoolean = true,
                Id = Guid.NewGuid(),
                NestedObject = new LoggingExtensionsTests.PropertyStringTestObject()
            };

            Assert.AreEqual($"Number: [{obj.Number}], Name: [{obj.Name}], SomeBoolean: [True], Id: [{obj.Id}], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+PropertyStringTestObject]",
                obj.GetPropertyInfoString(nameof(LoggingExtensionsTests.PropertyStringTestObject.Number), nameof(LoggingExtensionsTests.PropertyStringTestObject.Name), nameof(LoggingExtensionsTests.PropertyStringTestObject.SomeBoolean), nameof(LoggingExtensionsTests.PropertyStringTestObject.Id), nameof(LoggingExtensionsTests.PropertyStringTestObject.NestedObject)));

            Assert.AreEqual($"Id: [{obj.Id}], Name: [Jim Beam], NestedObject: [DotNetLittleHelpers.Tests.MiscExtensionsTests+PropertyStringTestObject], Number: [{obj.Number}], SomeBoolean: [True]",
                obj.GetPropertyInfoString());


        }

        [TestMethod()]
        public void TestGetPropertyString_Nulls()
        {
            LoggingExtensionsTests.PropertyStringTestObject obj = new LoggingExtensionsTests.PropertyStringTestObject();
            Assert.AreEqual($"Number: [0], Name: [*NULL*], SomeBoolean: [False], Id: [00000000-0000-0000-0000-000000000000], NestedObject: [*NULL*]",
                obj.GetPropertyInfoString(nameof(LoggingExtensionsTests.PropertyStringTestObject.Number), nameof(LoggingExtensionsTests.PropertyStringTestObject.Name), nameof(LoggingExtensionsTests.PropertyStringTestObject.SomeBoolean), nameof(LoggingExtensionsTests.PropertyStringTestObject.Id), nameof(LoggingExtensionsTests.PropertyStringTestObject.NestedObject)));
        }

        [TestMethod()]
        public void TestGetPropertyStringAllProps_Nulls()
        {
            LoggingExtensionsTests.PropertyStringTestObject obj = new LoggingExtensionsTests.PropertyStringTestObject();

            Assert.AreEqual($"Id: [00000000-0000-0000-0000-000000000000], Name: [*NULL*], NestedObject: [*NULL*], Number: [0], SomeBoolean: [False]",
                obj.GetPropertyInfoString());
        }
    }
}