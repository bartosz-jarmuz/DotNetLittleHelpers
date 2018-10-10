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
            public int Number2 { get; set; }
            public int Number3 { get; set; }

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
            Assert.AreEqual(5, q.OrderByMany(new OrderRule(nameof(TestObject.String), true)).First().Number);
            Assert.AreEqual(1, q.OrderBy(nameof(TestObject.String), false).First().Number);
            Assert.AreEqual(1, q.OrderByMany(new OrderRule(nameof(TestObject.String))).First().Number);


            Assert.AreEqual(1, q.OrderBy(nameof(TestObject.NullableDate), true).ThenBy(nameof(TestObject.Number)).First().Number);
            Assert.AreEqual(3, q.OrderBy(nameof(TestObject.NullableDate), true).ThenBy(nameof(TestObject.Number),true).First().Number);

            Assert.AreEqual(1, q.OrderByMany(new OrderRule(nameof(TestObject.NullableDate), true), new OrderRule(nameof(TestObject.Number))).First().Number);
            Assert.AreEqual(3, q.OrderByMany(new OrderRule(nameof(TestObject.NullableDate), true), new OrderRule(nameof(TestObject.Number), true)).First().Number);

            Assert.AreEqual(2, q.OrderBy(nameof(TestObject.NullableDate), false).ThenBy(nameof(TestObject.Number),true).First().Number);
            Assert.AreEqual(2, q.OrderByMany(new OrderRule(nameof(TestObject.NullableDate), false), new OrderRule(nameof(TestObject.Number), true)).First().Number);



        }

        [Test]
        public void TestOrderingByMany()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject() { Number = 1, Number2 = 5, Number3 = 11, String = "a"}   );
            list.Add(new TestObject() { Number = 2, Number2 = 4, Number3 = 22, String = "a"}   );
            list.Add(new TestObject() { Number = 3, Number2 = 3, Number3 = 22, String = "a"}   );
            list.Add(new TestObject() { Number = 4, Number2 = 2, Number3 = 11, String = "z"}   );
            list.Add(new TestObject() { Number = 5, Number2 = 1, Number3 = 22, String = "z"}   ); 
            var q = list.AsQueryable();

            Assert.AreEqual(3, q.OrderByMany(new OrderRule(nameof(TestObject.Number3), true), 
                                            new OrderRule(nameof(TestObject.String)), 
                                            new OrderRule(nameof(TestObject.Number2))
                ).First().Number);

            Assert.AreEqual(2, q.OrderByMany(new OrderRule(nameof(TestObject.Number3), true),
                new OrderRule(nameof(TestObject.String)),
                new OrderRule(nameof(TestObject.Number2), true)
            ).First().Number);

            Assert.AreEqual(1, q.OrderByMany(
                new Tuple<string, bool>(nameof(TestObject.Number3), false),
                new Tuple<string, bool>(nameof(TestObject.String), false),
                new Tuple<string, bool>(nameof(TestObject.Number2), true)
            ).First().Number);

            Assert.AreEqual(1, q.OrderByMany(new List<Tuple<string, bool>>()
            {
                new Tuple<string, bool>(nameof(TestObject.Number3), false),
                new Tuple<string, bool>(nameof(TestObject.String), false),
                new Tuple<string, bool>(nameof(TestObject.Number2), true)
            })
            .First().Number);

            Assert.AreEqual(4, q.OrderByMany(new OrderRule(nameof(TestObject.Number3)),
                new OrderRule(nameof(TestObject.String),true),
                new OrderRule(nameof(TestObject.Number2), true)
            ).First().Number);
        }


    }
    }