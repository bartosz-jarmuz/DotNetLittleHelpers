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
    public class QueryableOrderingTests
    {
        private class TestObject
        {
            public int Number { get; set; }

            public string String { get; set; }

            public DateTime? NullableDate { get; set; }
        }


        [Test]
        public void TestOrdering()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject(){ Number = 2, String = "b" });
            list.Add(new TestObject(){ Number = 5, String = "e", NullableDate =   DateTime.Parse("2000-01-01") });
            list.Add(new TestObject(){ Number = 3, String = "c"  ,NullableDate =  DateTime.Parse("2222-01-01") });
            list.Add(new TestObject() {Number = 1, String = "a" , NullableDate =  DateTime.Parse("2222-01-01") });
            list.Add(new TestObject(){ Number = 4, String = "d"  , NullableDate = DateTime.Parse("2000-01-01") });
            var q = list.AsQueryable();

            Assert.AreEqual(5, q.OrderBy(nameof(TestObject.String), true).First().Number);
            Assert.AreEqual(1, q.OrderBy(nameof(TestObject.String), false).First().Number);

            Assert.AreEqual(1, q.OrderBy(nameof(TestObject.NullableDate), true).ThenBy(nameof(TestObject.Number)).First().Number);
            Assert.AreEqual(3, q.OrderBy(nameof(TestObject.NullableDate), true).ThenBy(nameof(TestObject.Number),true).First().Number);

            Assert.AreEqual(2, q.OrderBy(nameof(TestObject.NullableDate), false).ThenBy(nameof(TestObject.Number),true).First().Number);



        }



    }
}