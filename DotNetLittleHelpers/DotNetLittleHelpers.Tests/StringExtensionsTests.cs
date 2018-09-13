using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetLittleHelpers.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class StringExtensionsTests
    {

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void ValidateEmailTest()
        {
            Assert.IsTrue("david.jones@proseware.com".IsValidEmail());
            Assert.IsTrue("d.j@server1.proseware.com".IsValidEmail());
            Assert.IsTrue("jones@ms1.proseware.com".IsValidEmail());
            Assert.IsTrue("js@proseware.com9".IsValidEmail());
            Assert.IsTrue("j.s@server1.proseware.com".IsValidEmail());
            Assert.IsTrue("\"j\\\"s\\\"\"@proseware.com".IsValidEmail()); 
            Assert.IsTrue("js@contoso.中国".IsValidEmail());
            Assert.IsTrue("j@proseware.com9".IsValidEmail());
            Assert.IsTrue("js#internal@proseware.com".IsValidEmail());
            Assert.IsTrue("j_9@[129.126.118.1]".IsValidEmail());

            Assert.IsFalse("j..s@proseware.com".IsValidEmail());
            Assert.IsFalse("js*@proseware.com".IsValidEmail());
            Assert.IsFalse("js@proseware..com".IsValidEmail());
            Assert.IsFalse("j.@server1.proseware.com".IsValidEmail());
            Assert.IsFalse("notemail".IsValidEmail());
            Assert.IsFalse("not.email".IsValidEmail());
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void ToLowerTest()
        {
            Assert.AreEqual("someThing", "SomeThing".ToLowerFirstChar());
            Assert.AreEqual("s", "S".ToLowerFirstChar());
            Assert.AreEqual("", "".ToLowerFirstChar());
            Assert.AreEqual(" ", " ".ToLowerFirstChar());
            string nullString = null;
            Assert.AreEqual(null, nullString.ToLowerFirstChar());
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void ToUppperTest()
        {
            Assert.AreEqual("SomeThing", "someThing".ToUpperFirstChar()); 
            Assert.AreEqual("S", "s".ToUpperFirstChar());
            Assert.AreEqual(" ", " ".ToUpperFirstChar());
            string nullString = null;
            Assert.AreEqual(null, nullString.ToUpperFirstChar());
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestVersionComparer_HappyPath()
        {
            Assert.IsTrue("1.0.0.0".IsNewerVersionThan("0.9.0.1"));
            Assert.IsTrue("1.0".IsNewerVersionThan("0.9.9"));

            Assert.IsFalse("1.0".IsNewerVersionThan("1.0.0.1"));

            Assert.IsFalse("1.0.1".IsNewerVersionThan("1.0.1"));
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestVersionComparer_Error()
        {
            NUnit.Framework.Assert.That(() => "0".IsNewerVersionThan(null), Throws.Exception.With.Message.Contain("Null parameter passed to method [IsNewerVersionThan]"));
            NUnit.Framework.Assert.That(() => "0".IsNewerVersionThan("0.9.0"), Throws.Exception.With.Message.Contain("Error while parsing [0] as Version"));

        }


        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void TestVersionComparer_List()
        {
            var list = new List<string>()
            {
                "1.0.0.0",
                "0.9.1",
                "2.0",
                "0.9.0",
            };
            var ordered = list.OrderBy(x => x, new VersionStringComparer()).ToList();

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
    }
}