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
        public void TestMaxMinBy()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 4, Date = new DateTime(5000,2,2)},
                new TestObject() {Number = 1, Date = DateTime.MinValue},
                new TestObject() {Number = 4, Date = new DateTime(5000,2,2)},
                new TestObject() {Number = 3, Date = DateTime.MaxValue},
                new TestObject() {Number = 1, Date = DateTime.MinValue},
                new TestObject() {Number = 2, Date = new DateTime(2000,2,2)},
                new TestObject() {Number = 5, Date = new DateTime(4000,2,2)},
                new TestObject() {Number = 2, Date = new DateTime(2000,2,2)},
                new TestObject() {Number = 3, Date = DateTime.MaxValue},
                new TestObject() {Number = 2, Date = new DateTime(2000,2,2)},
                new TestObject() {Number = 3, Date = DateTime.MaxValue}
            };
            var maxObjects = list.MaxBy(x => x.Date).ToList();
            var minObjects = list.MinBy(x => x.Date).ToList();
            Assert.AreEqual(3, maxObjects.Count());
            Assert.IsTrue(maxObjects.All(x=>x.Number ==3));
           Assert.AreEqual(2, minObjects.Count());
            Assert.IsTrue(minObjects.All(x => x.Number == 1));

        }

        [Test]
        public void TestMaxMinFirstBy()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 1, Date = DateTime.MinValue},
                new TestObject() {Number = 2, Date = new DateTime(2000,2,2)},
                new TestObject() {Number = 2, Date = new DateTime(2000,2,2)},
                new TestObject() {Number = 3, Date = DateTime.MaxValue}
            };
            TestObject maxObj = list.MaxFirstBy(x => x.Date);
            TestObject minObj = list.MinFirstBy(x => x.Date);
            Assert.AreEqual(3, maxObj.Number);
            Assert.AreEqual(1, minObj.Number);
        }

        [Test]
        public void TestMaxMinBy_SameValues()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 2, String="One"},
                new TestObject() {Number = 2, String="Two"},
            };
            TestObject maxObj = list.MaxFirstBy(x => x.Number);
            TestObject minObj = list.MinFirstBy(x => x.Number);
            Assert.AreEqual("One", maxObj.String);
            Assert.AreEqual("One", minObj.String);
        }

        [Test]
        public void TestMaxMinFirstBy_SingleElement()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 1},
            };
            TestObject maxObj = list.MaxFirstBy(x => x.Number);
            TestObject minObj = list.MinFirstBy(x => x.Number);
            Assert.AreEqual(1, maxObj.Number);
            Assert.AreEqual(1, minObj.Number);
        }

        [Test]
        public void TestMaxMinBy_SingleElement()
        {
            List<TestObject> list = new List<TestObject>()
            {
                new TestObject() {Number = 1},
            };
            var maxObjs = list.MaxBy(x => x.Number);
            var minObjs = list.MinBy(x => x.Number);
            Assert.AreEqual(1, maxObjs.Single().Number);
            Assert.AreEqual(1, minObjs.Single().Number);
        }

        [Test]
        public void TestMinMaxBy_NullOrEmptyList()
        {
            List<TestObject> list = null;

            Assert.Throws<ArgumentNullException>(() => list.MaxBy(x => x.Date));
            Assert.Throws<ArgumentNullException>(() => list.MinBy(x => x.Date));

            list = new List<TestObject>();

            Assert.That(() => list.MaxBy(x => x.Date), Throws.ArgumentException.With.Message.EqualTo("Source collection is empty."));
            Assert.That(() => list.MinBy(x => x.Date), Throws.ArgumentException.With.Message.EqualTo("Source collection is empty."));

        }

        [Test]
        public void TestMinMaxFirstBy_NullOrEmptyList()
        {
            List<TestObject> list = null;

            Assert.Throws<ArgumentNullException>(() => list.MaxFirstBy(x => x.Date));
            Assert.Throws<ArgumentNullException>(() => list.MinFirstBy(x => x.Date));

            list = new List<TestObject> ();

            Assert.That(() => list.MaxFirstBy(x => x.Date), Throws.ArgumentException.With.Message.EqualTo("Source collection is empty."));
            Assert.That(() => list.MinFirstBy(x => x.Date), Throws.ArgumentException.With.Message.EqualTo("Source collection is empty."));

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