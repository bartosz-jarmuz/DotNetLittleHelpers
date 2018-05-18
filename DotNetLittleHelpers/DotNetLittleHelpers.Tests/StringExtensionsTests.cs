﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {

        [TestMethod()]
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

        [TestMethod()]
        public void ToLowerTest()
        {
            Assert.AreEqual("someThing", "SomeThing".ToLowerFirstChar());
            Assert.AreEqual("s", "S".ToLowerFirstChar());
            Assert.AreEqual("", "".ToLowerFirstChar());
            Assert.AreEqual(" ", " ".ToLowerFirstChar());
            string nullString = null;
            Assert.AreEqual(null, nullString.ToLowerFirstChar());
        }

        [TestMethod()]
        public void ToUppperTest()
        {
            Assert.AreEqual("SomeThing", "someThing".ToUpperFirstChar()); 
            Assert.AreEqual("S", "s".ToUpperFirstChar());
            Assert.AreEqual(" ", " ".ToUpperFirstChar());
            string nullString = null;
            Assert.AreEqual(null, nullString.ToUpperFirstChar());
        }
    }
}