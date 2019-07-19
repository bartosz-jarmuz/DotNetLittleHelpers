using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    using System;
    using System.Reflection.Emit;

    [TestClass()]
    public class MiscLinqExtensionsTests
    {

        [TestMethod()]
        public void TestOneExists()
        {
            var input = new int[] {1, 2, 3, 4, 4};

            Assert.IsTrue(input.UniqueExists(x=>x == 3));
            Assert.IsFalse(input.UniqueExists(x=>x == 4));
            Assert.IsFalse(input.UniqueExists(x=>x == 6));

            input = new int[] { };

            Assert.IsFalse(input.UniqueExists(x => x == 3));

        }
    }
}