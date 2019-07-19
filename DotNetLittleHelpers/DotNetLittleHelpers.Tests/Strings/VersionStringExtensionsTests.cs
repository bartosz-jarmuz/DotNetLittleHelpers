using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace DotNetLittleHelpers.Tests
{
    [TestClass]
    public class VersionStringExtensionsTests
    {
        [TestMethod]
        public void TestVersionComparer_HappyPath()
        {
            Assert.IsTrue("1.0.0.0".IsNewerVersionThan("0.9.0.1"));
            Assert.IsTrue("1.0".IsNewerVersionThan("0.9.9"));
            Assert.IsTrue("1.0".IsNewerOrEqualVersion("0.9.9"));
            Assert.AreEqual(1, "1.0".CompareVersionStrings("0.9.9"));

            Assert.IsFalse("1.0".IsNewerVersionThan("1.0.0.1"));
            Assert.IsFalse("1.0".IsNewerOrEqualVersion("1.0.0.1"));
            Assert.AreEqual(-1, "1.0".CompareVersionStrings("1.0.0.1"));


            Assert.IsFalse("1.0.1".IsNewerVersionThan("1.0.1"));
            Assert.IsFalse("1.0.0".IsNewerVersionThan("1.0"));
            Assert.IsFalse("1.0".IsNewerVersionThan("1.0.0"));
            Assert.IsTrue("1.0.1".IsNewerOrEqualVersion("1.0.1"));
            Assert.IsTrue("1.0".IsNewerOrEqualVersion("1.0.0"));
            Assert.IsTrue("1.0.00.0".IsNewerOrEqualVersion("1.0.0"));
            Assert.AreEqual(0, "1.0.1".CompareVersionStrings("1.0.1"));
        }


        [TestMethod]
        public void TestDifferentStringLength()
        {
            Assert.AreEqual(-1, "1.0".CompareVersionStrings("1.0.0", false));
            Assert.AreEqual(1, "1.0.0.0".CompareVersionStrings("1.0.0", false));
            Assert.AreEqual(0, "1.0.0.0".CompareVersionStrings("1.0.0.0", false));

            Assert.AreEqual(0, "1.0".CompareVersionStrings("1.0.0"));
            Assert.AreEqual(0, "1.0.0".CompareVersionStrings("1.0.0"));
            Assert.AreEqual(0, "1.0.0.0".CompareVersionStrings("1.0.0"));

            Assert.IsFalse("1.0.0".IsNewerVersionThan("1.0"));
            Assert.IsFalse("1.0".IsNewerVersionThan("1.0.0.0"));
            Assert.IsTrue("1.0.0".IsNewerVersionThan("1.0", false));
            Assert.IsFalse("1.0.0".IsNewerVersionThan("1.0.0.0", false));

            Assert.IsTrue("1.0.0".IsNewerOrEqualVersion("1.0"));
            Assert.IsTrue("1.3.0".IsNewerOrEqualVersion("1.3.0.0"));
            Assert.IsTrue("1.3.0.0".IsNewerOrEqualVersion("1.3.0"));
            Assert.IsTrue("1.0".IsNewerOrEqualVersion("1.0.0.0"));

            Assert.IsTrue("1.0.0".IsNewerOrEqualVersion("1.0", false));
            Assert.IsFalse("1.0.0".IsNewerOrEqualVersion("1.0.0.0", false));
        }

        [TestMethod]
        public void TestVersionComparer_Error()
        {
            Assert.That(() => "1.0".IsNewerVersionThan(null), Throws.Exception.With.Message.Contain("Null parameter passed to method [IsNewerVersionThan]"));
            Assert.That(() => "0".IsNewerVersionThan("0.9.0"), Throws.Exception.With.Message.Contain("Error while parsing [0] as Version"));
            Assert.That(() => "2".NormalizeVersionString(), Throws.Exception.With.Message.Contain("Error while parsing [2] as Version"));
        }


        [TestMethod]
        public void TestVersionComparer_List()
        {
            List<string> list = new List<string>
            {
                "1.0.0.0",
                "0.9.1",
                "2.0",
                "0.9.0"
            };
            List<string> ordered = list.OrderBy(x => x, new VersionStringComparer()).ToList();

            Assert.AreEqual("0.9.0", ordered[0]);
            Assert.AreEqual("0.9.1", ordered[1]);
            Assert.AreEqual("1.0.0.0", ordered[2]);
            Assert.AreEqual("2.0", ordered[3]);

            ordered = list.OrderByDescending(x => x, new VersionStringComparer()).ToList();

            Assert.AreEqual("0.9.0", ordered[3]);
            Assert.AreEqual("0.9.1", ordered[2]);
            Assert.AreEqual("1.0.0.0", ordered[1]);
            Assert.AreEqual("2.0", ordered[0]);
        }

        [TestMethod]
        public void TestStringNormalization()
        {
            Assert.AreEqual("0.9.0.0", "0.9".NormalizeVersionString());
            Assert.AreEqual("0.9.0.0", "0.9.0".NormalizeVersionString());
            Assert.AreEqual("1.2.0.0", "1.2".NormalizeVersionString());
            Assert.AreEqual("1.2.0.0", "1.2.0.0".NormalizeVersionString());

        }
    }
}