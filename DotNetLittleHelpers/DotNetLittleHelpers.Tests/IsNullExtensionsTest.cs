namespace DotNetLittleHelpers.Tests
{
    #region Using
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    #endregion

    [TestFixture]
    public class IsNullExtensionsTest
    {
        [Test]
        public void GuidTests()
        {
            Guid g = new Guid();

            Assert.IsTrue(g.IsEmpty());
            Assert.IsFalse(g.IsNotEmpty());

            g = Guid.NewGuid();

            Assert.IsFalse(g.IsEmpty());
            Assert.IsTrue(g.IsNotEmpty());
        }



        [Test]
        public void AnyAndNotNull()
        {
            List<TestObject> list = null;
            Assert.IsFalse(list.AnyAndNotNull());

            list = new List<TestObject>();
            Assert.IsFalse(list.AnyAndNotNull());

            list = new List<TestObject> { null };
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


        [Test]
        public void CheckArgumentNullTest_NotNull()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject());
            list.CheckArgumentNull("list");
            list.ThrowIfNull("list");
            new TestObject().CheckArgumentNull("obj");
            new TestObject().ThrowIfNull("obj");
        }

        [Test]
        public void CheckArgumentNullTest_Null()
        {
            List<TestObject> list = null;
            try
            {
                list.CheckArgumentNull("list");
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Null parameter passed to method [CheckArgumentNullTest_Null].\r\nParameter name: list", ex.Message);
            }

            try
            {
                list.ThrowIfNull("list");
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Null parameter passed to method [CheckArgumentNullTest_Null].\r\nParameter name: list", ex.Message);
            }


        }

    
        private class TestObject
        {
        }
    }
}