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
    public class MinMaxExtensionsTests
    {
        private class TestObject
        {
            public int Number { get; set; }

            public string String { get; set; }

            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }
        }

        [Test]
        public void TestActualData()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 1},
                new TestObject() {Number = 2}
            };

            Assert.AreEqual(2, list.MaxOrDefault(x => x.Number));
            Assert.AreEqual(1, list.MinOrDefault(x => x.Number));
            Assert.AreEqual(2, list.MaxOrDefaultIfNull(x => x.Number));
            Assert.AreEqual(1, list.MinOrDefaultIfNull(x => x.Number));


            Assert.AreEqual(2, list.MaxOrNull(x => x.Number));
            Assert.AreEqual(1, list.MinOrNull(x => x.Number));
            Assert.AreEqual(2, list.MaxOrNullIfNull(x => x.Number));
            Assert.AreEqual(1, list.MinOrNullIfNull(x => x.Number));
        }

        [Test]
        public void TestEmptyList()
        {
            List<TestObject> list = new List<TestObject>();

            Assert.AreEqual(default(DateTime), list.MaxOrDefault(x => x.Date));
            Assert.AreEqual(default(DateTime), list.MinOrDefault(x => x.Date));
            Assert.AreEqual(default(DateTime), list.MaxOrDefaultIfNull(x => x.Date));
            Assert.AreEqual(default(DateTime), list.MinOrDefaultIfNull(x => x.Date));

            Assert.AreEqual(null, list.MaxOrDefault(x => x.NullableDate));
            Assert.AreEqual(null, list.MinOrDefault(x => x.NullableDate));
            Assert.AreEqual(null, list.MaxOrDefault(x => x.String));
            Assert.AreEqual(null, list.MinOrDefault(x => x.String));

            Assert.AreEqual(null, list.MaxOrDefaultIfNull(x => x.NullableDate));
            Assert.AreEqual(null, list.MinOrDefaultIfNull(x => x.NullableDate));
            Assert.AreEqual(null, list.MaxOrDefaultIfNull(x => x.String));
            Assert.AreEqual(null, list.MinOrDefaultIfNull(x => x.String));

            Assert.AreEqual(null, list.MaxOrNull(x => x.Date));
            Assert.AreEqual(null, list.MinOrNull(x => x.Date));
            Assert.AreEqual(null, list.MaxOrNullIfNull(x => x.Date));
            Assert.AreEqual(null, list.MinOrNullIfNull(x => x.Date));
        }

        [Test]
        public void TestNullList()
        {
            List<TestObject> list = null;

            Assert.Throws<ArgumentNullException>(() => list.MaxOrDefault(x => x.Date));
            Assert.Throws<ArgumentNullException>(() => list.MinOrDefault(x => x.Date));

            Assert.AreEqual(default(DateTime), list.MaxOrDefaultIfNull(x => x.Date));
            Assert.AreEqual(default(DateTime), list.MinOrDefaultIfNull(x => x.Date));

            Assert.AreEqual(null, list.MaxOrDefaultIfNull(x => x.NullableDate));
            Assert.AreEqual(null, list.MinOrDefaultIfNull(x => x.NullableDate));

            Assert.Throws<ArgumentNullException>(() => list.MaxOrNull(x => x.Date));
            Assert.Throws<ArgumentNullException>(() => list.MinOrNull(x => x.Date));
            Assert.AreEqual(null, list.MaxOrNullIfNull(x => x.Date));
            Assert.AreEqual(null, list.MinOrNullIfNull(x => x.Date));
        }


     
    }
}