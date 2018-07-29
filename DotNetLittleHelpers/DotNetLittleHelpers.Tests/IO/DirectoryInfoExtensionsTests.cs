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

        [TestMethod]
        public void TestAppendFilePath()
        {
            var absoluteDir = new DirectoryInfo(@"c:\dir");

            Assert.AreEqual(@"c:\dir\file", absoluteDir.CreateFileInfo("file").FullName);
        }

        [TestMethod]
        public void TestAppendSubDirPath()
        {
            var absoluteDir = new DirectoryInfo(@"c:\dir");

            Assert.AreEqual(@"c:\dir\subdir", absoluteDir.CreateSubDirectoryInfo("subdir").FullName);
            Assert.AreEqual(@"c:\dir\subdir\nested", absoluteDir.CreateSubDirectoryInfo(@"subdir\nested").FullName);
        }
    }
}
