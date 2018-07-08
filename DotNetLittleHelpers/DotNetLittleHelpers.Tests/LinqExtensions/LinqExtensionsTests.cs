using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetLittleHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers.Tests
{
    using NUnit.Framework;

    [TestClass()]
    [TestFixture]
    public class LinqExtensionsTests
    {
        private class TestObject
        {
            public int Number { get; set; }

            public string String { get; set; }

            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }
        }

       

        [Test]
        public void AnyAndNotNull()
        {
            List<TestObject> list = null;
            Assert.IsFalse(list.AnyAndNotNull());

            list = new List<TestObject>();
            Assert.IsFalse(list.AnyAndNotNull());

            list = new List<TestObject> {null};
            Assert.IsTrue(list.AnyAndNotNull());

            list = new List<TestObject> { new TestObject() };
            Assert.IsTrue(list.AnyAndNotNull());
        }


        [Test]
        public void Enumerable_IsNullOrEmpty()
        {
            List<TestObject> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());

            list = new List<TestObject>();
            Assert.IsTrue(list.IsNullOrEmpty());

            list = new List<TestObject> { null };
            Assert.IsFalse(list.IsNullOrEmpty());

            list = new List<TestObject> { new TestObject() };
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrDefault()
        {
            Guid? nullableGuid = null;
            Assert.IsTrue(nullableGuid.IsNullOrDefault());

            nullableGuid = Guid.Empty;
            Assert.IsTrue(nullableGuid.IsNullOrDefault());

            nullableGuid = Guid.NewGuid();
            Assert.IsFalse(nullableGuid.IsNullOrDefault());
        }

    }
}