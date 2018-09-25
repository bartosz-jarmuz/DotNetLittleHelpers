using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests.Zip
{
    [TestClass]
    [DeploymentItem("Zip", "Zip")]
    public class ZipHelpersTests
    {
        [TestMethod]
        [DeploymentItem("NotAZip.zip", "Zip")]
        public void TestNoZipFile()
        {
            using (StreamReader sr = new StreamReader("Zip/NotAZip.zip"))
            {
                Assert.IsFalse(ZipHelpers.IsZipCompressedData(sr.BaseStream));
                Assert.IsFalse(ZipHelpers.IsCompressedData(sr.BaseStream));
                Assert.AreEqual(0, sr.BaseStream.Position);
            }
        }


        [TestMethod]
        [DeploymentItem("NotAZip.zip", "Zip")]
        public void TestZipFile()
        {
            using (StreamReader sr = new StreamReader("Zip/ThisIsAZip.zip"))
            {
                Assert.IsTrue(ZipHelpers.IsZipCompressedData(sr.BaseStream));
                Assert.IsTrue(ZipHelpers.IsCompressedData(sr.BaseStream));
                Assert.AreEqual(0, sr.BaseStream.Position);
            }
        }
    }
}
