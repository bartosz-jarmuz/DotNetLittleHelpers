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
    public class FilterBuilderTests
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
        public void TestWhere()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject(){ Number = 2,Number2 = 11, String = "baa" });
            list.Add(new TestObject(){ Number = 3,Number2 = 22, String = "eaaa", NullableDate =   DateTime.Parse("2000-01-01") });
            list.Add(new TestObject(){ Number = 3,Number2 = 33, String = "baaa"  });
            list.Add(new TestObject() {Number = 2,Number2 = 44, String = "baaa" , NullableDate =  DateTime.Parse("2222-01-01") });
            list.Add(new TestObject(){ Number = 2,Number2 = 55, String = "daa"   });
            var q = list.AsQueryable();

            List<Filter> filters = new List<Filter>()
            {
                new Filter { PropertyName = nameof(TestObject.Number) ,
                    Comparison = ComparisonRule.Equals, Value = 2  },
                new Filter { PropertyName = nameof(TestObject.String) ,
                    Comparison = ComparisonRule.StartsWith, Value = "b"  },
                new Filter { PropertyName =nameof(TestObject.NullableDate),
                    Comparison = ComparisonRule.Equals, Value = null }
            };

            var deleg = FilterBuilder.GetExpression<TestObject>(filters).Compile();
            var filteredCollection = q.Where(deleg).ToList();
            Assert.AreEqual(11, filteredCollection.Single().Number2);

            filters = new List<Filter>()
            {
                new Filter { PropertyName = nameof(TestObject.Number) ,
                    Comparison = ComparisonRule.Equals, Value = 2  },
                new Filter { PropertyName = nameof(TestObject.String) ,
                    Comparison = ComparisonRule.StartsWith, Value = "b"  },
            };

            deleg = FilterBuilder.GetCompiled<TestObject>(filters);
            filteredCollection = q.Where(deleg).ToList();
            Assert.AreEqual(11, filteredCollection.First().Number2);
            Assert.AreEqual(44, filteredCollection.Last().Number2);

        }

        [Test]
        public void TestOrderingWithParam()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject() {Number = 2, String = "b"});
            list.Add(new TestObject() {Number = 5, String = "e", NullableDate = DateTime.Parse("2000-01-01")});
            list.Add(new TestObject() {Number = 3, String = "c", NullableDate = DateTime.Parse("2222-01-01")});
            list.Add(new TestObject() {Number = 1, String = "a", NullableDate = DateTime.Parse("2222-01-01")});
            list.Add(new TestObject() {Number = 4, String = "d", NullableDate = DateTime.Parse("2000-01-01")});
            var q = list.AsQueryable();

            Assert.AreEqual(5, q.OrderBy(x=>x.String, true).First().Number);
            Assert.AreEqual(1, q.OrderBy(x=>x.String, false).First().Number);


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