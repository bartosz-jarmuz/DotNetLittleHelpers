using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetLittleHelpers.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = NUnit.Framework.Assert;

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class FileSizeExtensionsTests
    {

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestGetSizeString()
        {
            Assert.AreEqual("0.0 bytes", ((long)0).GetSizeString());
            Assert.AreEqual("500.0 bytes", ((long)500).GetSizeString());
            Assert.AreEqual("425.1 KB", ((long)435343).GetSizeString());
            Assert.AreEqual("425.14 KB", ((long)435343).GetSizeString(2));
            Assert.AreEqual("8,192.0 PB", (long.MaxValue).GetSizeString());
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestConversionToKilo()
        {
            Assert.AreEqual(0, ((long)0).ConvertBytesToKilobytes());
            Assert.AreEqual(0.25, ((long)256).ConvertBytesToKilobytes());
            Assert.AreEqual(1, ((long)1024).ConvertBytesToKilobytes());
            Assert.AreEqual(425.14, ((long)435343).ConvertBytesToKilobytes());
            Assert.AreEqual(425.13965, ((long)435343).ConvertBytesToKilobytes(5));
        }
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestConversionToMega()
        {
            Assert.AreEqual(0, ((long)0).ConvertBytesToMegabytes());
            Assert.AreEqual(1, ((long)1024*1024).ConvertBytesToMegabytes());
            Assert.AreEqual(1.00195, ((long)1024*1024+2048).ConvertBytesToMegabytes(5));
        }

    }
}