using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests.IO
{
    [TestClass]
    public class DirectoryInfoExtensionsTests
    {
        [TestMethod]
        public void TestExists()
        {
            var path = @".\TestDir";
            Directory.CreateDirectory(path);
            var directoryInfo = new DirectoryInfo(path);
            Assert.IsTrue(Directory.Exists(directoryInfo.FullName));
            Assert.IsTrue(directoryInfo.Exists());
            Directory.Delete(directoryInfo.FullName);
            Assert.IsFalse(directoryInfo.Exists());
            Assert.IsFalse(Directory.Exists(directoryInfo.FullName));
        }
    }
}
