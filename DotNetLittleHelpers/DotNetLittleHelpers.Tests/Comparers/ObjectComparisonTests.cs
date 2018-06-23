// -----------------------------------------------------------------------
//  <copyright file="ObjectComparisonTests.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace DotNetLittleHelpers.Tests
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;
    #endregion

    [TestFixture()]
    public class ObjectComparisonTests
    {
        [Test]
        public void PropertiesAreEqualTest__OtherBindingFlags_AreDifferent()
        {
            var obj1 = new TestObject(33.2M, null)
            {
                Number = 2,
                Text = "Test",
                PublicDecimalField = 1.2M
            };
            var obj2 = new TestObject(0, "SomeValue")
            {
                Number = 2,
                Text = "Test",
            };
            var errorsList = new List<string>();
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2);

            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, validationErrorsList: errorsList));
            Assert.That(()=> obj1.ThrowIfPropertiesNotEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic), Throws.Exception.TypeOf<AggregateException>());

            Assert.AreEqual(1, errorsList.Count);
            Assert.AreEqual("Type: [TestObject]. Property: [PrivateStringProperty]. Source: [NULL]. Target: [SomeValue]", errorsList[0]);

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, throwIfNotEqual: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual("Type: [TestObject]. Property: [PrivateStringProperty]. Source: [NULL]. Target: [SomeValue]", ex.InnerExceptions.Single().Message);
            }
        }


        [Test]
        public void PropertiesAreEqualTest_IgnoreList()
        {
            var obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
            };
            var obj2 = new TestObject()
            {
                Number = 2,
                Text = "Test2",
                Date = new DateTime(2000, 1, 2)
            };
            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                ignoreProperties: new []{nameof(TestObject.Text), nameof(TestObject.Date)} ));

            obj1.ThrowIfPropertiesNotEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                ignoreProperties: new[] { nameof(TestObject.Text), nameof(TestObject.Date) });
        }

        [Test]
        public void PropertiesAreEqualTest_IncludeList()
        {
            var obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
                NullableDate = DateTime.MaxValue
            };
            var obj2 = new TestObject()
            {
                Number = 2333,
                Text = "Test",
                NullableDate = DateTime.MinValue
            };
            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                includeProperties: new[] { nameof(TestObject.Text), nameof(TestObject.Date) }));

            obj1.ThrowIfPropertiesNotEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                includeProperties: new[] { nameof(TestObject.Text), nameof(TestObject.Date) });
        }

        [Test]
        public void PropertiesAreEqualTest_IncludeAndIgnoreList()
        {
            var obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
                NullableDate = DateTime.MaxValue
            };
            var obj2 = new TestObject()
            {
                Number = 2333,
                Text = "Test",
                NullableDate = DateTime.MinValue
            };
            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, ignoreProperties: new[] { nameof(TestObject.NullableDate) }));

            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                includeProperties: new[] { nameof(TestObject.Text) }, ignoreProperties: new []{ nameof(TestObject.NullableDate) }));

            obj1.ThrowIfPropertiesNotEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                includeProperties: new[] { nameof(TestObject.Text) }, ignoreProperties: new[] { nameof(TestObject.NullableDate) });

        }

        [Test]
        public void PropertiesAreEqualTest_AreDifferent()
        {
            var obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
            };
            var obj2 = new TestObject()
            {
                Number = 2,
                Text = "Test2",
                Date = new DateTime(2000, 1, 2)
            };

            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance));
            Assert.IsFalse(obj1.PublicInstancePropertiesAreEqual(obj2));
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, throwIfNotEqual: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                Assert.AreEqual("Type: [TestObject]. Property: [Text]. Source: [Test]. Target: [Test2]", ex.InnerExceptions[0].Message);
                Assert.AreEqual("Type: [TestObject]. Property: [Date]. Source: [01.01.0001 00:00:00]. Target: [02.01.2000 00:00:00]", ex.InnerExceptions[1].Message);
            }
        }

        [Test]
        public void PropertiesAreEqualTest_AreSame()
        {
            var obj1 = new TestObject(33.2M, null)
            {
                Number = 2,
                Text = "Test",
                PublicDecimalField = 1.2M,
            };
            var obj2 = new TestObject(0, "SomeValue")
            {
                Number = 2,
                Text = "Test",
            };

            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance));
            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2);
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2));

            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: true));
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2, recursiveValidation: true));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2, recursiveValidation: true);

        }

        [Test]
        public void PropertiesAreEqualTest_NestedObject_Shared()
        {
            this.GetTestObject_SharedNestedObjects(out TestObject obj1, out TestObject obj2);

            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance));
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2);

        }

        [Test]
        public void PropertiesAreEqualTest_NestedObject_SimilarButSeparate_NoRecursion()
        {
            this.GetTestObject_SimilarNestedObjects(out TestObject obj1, obj2: out TestObject obj2);

            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance));
            Assert.IsFalse(obj1.PublicInstancePropertiesAreEqual(obj2));
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(
                    "Type: [TestObject]. Property: [NestedObject]. Source: [DotNetLittleHelpers.Tests.ObjectComparisonTests+SecondObject]. Target: [DotNetLittleHelpers.Tests.ObjectComparisonTests+SecondObject]",
                    ex.InnerExceptions.Single().Message);
            }
        }

        [Test]
        public void PropertiesAreEqualTest_NestedObject_Nulls()
        {
            this.GetTestObject_SimilarNestedObjects(out TestObject obj1, obj2: out TestObject obj2);
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            obj1.NestedObject = null;
            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true, recursiveValidation: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(
                    "Type: [TestObject]. Property: [NestedObject]. Source: [NULL]. Target: [DotNetLittleHelpers.Tests.ObjectComparisonTests+SecondObject]",
                    ex.InnerExceptions.Single().Message);
            }

            obj1.NestedObject = new SecondObject(){DecimalNumber = 2};
            obj2.NestedObject = null;

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true, recursiveValidation: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(
                    "Type: [TestObject]. Property: [NestedObject]. Source: [DotNetLittleHelpers.Tests.ObjectComparisonTests+SecondObject]. Target: [NULL]",
                    ex.InnerExceptions.Single().Message);
            }

            obj2.NestedObject = new SecondObject() { DecimalNumber = 2 };
            obj1.NestedObject.SubNestedObject = new SecondObject();
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true, recursiveValidation: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(
                    "Object: [NestedObject]. Type: [SecondObject]. Property: [SubNestedObject]. Source: [DotNetLittleHelpers.Tests.ObjectComparisonTests+SecondObject]. Target: [NULL]",
                    ex.InnerExceptions.Single().Message);
            }
        }


        [Test]
        public void PropertiesAreEqualTest_NestedObject_WithRecursion_DifferentValues()
        {
            this.GetTestObject_SimilarNestedObjects(out TestObject obj1, obj2: out TestObject obj2);
            obj1.NestedObject.DecimalNumber = 66.6M;
            obj1.NestedObject.SubNestedObject = new SecondObject() {DecimalNumber = 3};
            obj2.NestedObject.SubNestedObject = new SecondObject() {DecimalNumber = 3};
            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: true));
            Assert.IsFalse(obj1.PublicInstancePropertiesAreEqual(obj2, recursiveValidation: true));
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true, recursiveValidation: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(
                    "Object: [NestedObject]. Type: [SecondObject]. Property: [DecimalNumber]. Source: [66,6]. Target: [34,2]",
                    ex.InnerExceptions.Single().Message);
            }

            obj1.NestedObject.SubNestedObject = new SecondObject(){DecimalNumber = 2};
            obj1.NullableDate = DateTime.MinValue;
            
            Assert.IsFalse(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: true));
            Assert.IsFalse(obj1.PublicInstancePropertiesAreEqual(obj2, recursiveValidation: true));
            Assert.That(() => obj1.ThrowIfPublicPropertiesNotEqual(obj2), Throws.Exception.TypeOf<AggregateException>());

            try
            {
                obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, throwIfNotEqual: true, recursiveValidation: true);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(3, ex.InnerExceptions.Count);

                Assert.AreEqual(
                    "Type: [TestObject]. Property: [NullableDate]. Source: [01.01.0001 00:00:00]. Target: [NULL]",
                    ex.InnerExceptions[0].Message);
                Assert.AreEqual(
                    "Object: [NestedObject]. Type: [SecondObject]. Property: [DecimalNumber]. Source: [66,6]. Target: [34,2]",
                    ex.InnerExceptions[1].Message);
                Assert.AreEqual(
                    "Object: [SubNestedObject]. Type: [SecondObject]. Property: [DecimalNumber]. Source: [2]. Target: [3]",
                    ex.InnerExceptions[2].Message);
            }


        }

        [Test]
        public void PropertiesAreEqualTest_NestedObject_SameValues_WithRecursion()
        {
            this.GetTestObject_SimilarNestedObjects(out TestObject obj1, obj2: out TestObject obj2);
            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: true));
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2, recursiveValidation: true));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2, recursiveValidation: true);


            this.GetTestObject_SharedNestedObjects(out  obj1, obj2: out  obj2);
            Assert.IsTrue(obj1.PropertiesAreEqual(obj2, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: true));
            Assert.IsTrue(obj1.PublicInstancePropertiesAreEqual(obj2, recursiveValidation: true));
            obj1.ThrowIfPublicPropertiesNotEqual(obj2, recursiveValidation: true);
        }

        private void GetTestObject_SharedNestedObjects(out TestObject obj1, out TestObject obj2)
        {
            var nested = new SecondObject()
            {
                DecimalNumber = 34.2M
            };
            obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
                NestedObject = nested
            };
            obj2 = new TestObject(0, "SomeValue")
            {
                Number = 2,
                Text = "Test",
                NestedObject = nested
            };
        }

        private void GetTestObject_SimilarNestedObjects(out TestObject obj1, out TestObject obj2)
        {
            obj1 = new TestObject()
            {
                Number = 2,
                Text = "Test",
                NestedObject = new SecondObject()
                {
                    DecimalNumber = 34.2M
                }
            };
            obj2 = new TestObject(0, "SomeValue")
            {
                Number = 2,
                Text = "Test",
                NestedObject = new SecondObject()
                {
                    DecimalNumber = 34.2M
                }
            };
        }

        private class TestObject
        {
            public TestObject()
            {
            }

            public TestObject(decimal privateDecimalField, string privateStringProperty)
            {
                this._privateDecimalField = privateDecimalField;
                this.PrivateStringProperty = privateStringProperty;
            }

            private decimal _privateDecimalField;
#pragma warning disable 169
            public decimal PublicDecimalField;
#pragma warning restore 169
            public decimal Number { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }
            public SecondObject NestedObject { get; set; }
            private string PrivateStringProperty { get; set; }
        }

        public class SecondObject
        {
            public decimal DecimalNumber { get; set; }
            public SecondObject SubNestedObject { get; set; }
        }
    }
}