using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass()]
    public class NumbersTests
    {

        [TestMethod()]
        public void GetOrdinalTest()
        {
            Assert.AreEqual("1st", 1.GetOrdinalNumber());

            Assert.AreEqual("21st", 21.GetOrdinalNumber());

            Assert.AreEqual("2nd", 2.GetOrdinalNumber());

            Assert.AreEqual("323rd", 323.GetOrdinalNumber());

            Assert.AreEqual("56776th", 56776.GetOrdinalNumber());
            Assert.AreEqual("0", 0.GetOrdinalNumber());

        }

        [TestMethod()]
        public void GetOrdinalSuffixTest()
        {
            Assert.AreEqual("", 0.GetOrdinalSuffix());

            Assert.AreEqual("st", 1.GetOrdinalSuffix());
            Assert.AreEqual("th", 11.GetOrdinalSuffix());
            Assert.AreEqual("st", 21.GetOrdinalSuffix());

            Assert.AreEqual("nd", 2.GetOrdinalSuffix());
            Assert.AreEqual("th", 12.GetOrdinalSuffix());
            Assert.AreEqual("nd", 22.GetOrdinalSuffix());
            Assert.AreEqual("nd", 262.GetOrdinalSuffix());

            Assert.AreEqual("rd", 3.GetOrdinalSuffix());
            Assert.AreEqual("th", 13.GetOrdinalSuffix());
            Assert.AreEqual("rd", 23.GetOrdinalSuffix());
            Assert.AreEqual("rd", 323.GetOrdinalSuffix());


            Assert.AreEqual("th", 4.GetOrdinalSuffix());
            Assert.AreEqual("th", 45.GetOrdinalSuffix());
            Assert.AreEqual("th", 16.GetOrdinalSuffix());
            Assert.AreEqual("th", 56776.GetOrdinalSuffix());
        }


    }
}