﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests.IO
{
    [TestClass]
    public class FileInfoExtensionsTests
    {
        [TestMethod]
        public void TestExists()
        {
            var path = @".\aybabtu.txt";
            File.WriteAllText(path, "All your base are belong to us!");
            var file = new FileInfo(path);

            Assert.IsTrue(file.ExistsAtTheMoment());
            Assert.IsTrue(File.Exists(file.FullName));

            File.Delete(file.FullName);

            Assert.IsFalse(File.Exists(file.FullName)); //yup, deleted for sure
            Assert.IsTrue(file.Exists); //yet .Exists is true

            Assert.IsFalse(file.ExistsAtTheMoment());
            Assert.IsFalse(file.Exists); //but not anymore

        }
    }
}
