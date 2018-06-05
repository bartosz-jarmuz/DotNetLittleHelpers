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
        public void CheckArgumentNullTest_NotNull()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject());
            list.CheckArgumentNull("list");
            new TestObject().CheckArgumentNull("obj");
        }

        [Test]
        public void CheckArgumentNullTest_Null()
        {
            List<TestObject> list = null;
            try
            {
                list.CheckArgumentNull("list");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Null parameter passed to method [CheckArgumentNullTest_Null].\r\nParameter name: list", ex.Message);
                return;
            }
            Assert.Fail("Exception expected");



        }

     
    }
}