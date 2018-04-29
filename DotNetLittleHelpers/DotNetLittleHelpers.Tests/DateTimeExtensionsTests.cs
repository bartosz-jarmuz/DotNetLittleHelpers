using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void TestFirstDayOfWeek()
        {
            Assert.AreEqual(new DateTime(2018, 04, 23), new DateTime(2018, 04, 26).FirstDayOfWeek());
            Assert.AreEqual(new DateTime(2018, 04, 23), new DateTime(2018, 04, 23).FirstDayOfWeek());
            Assert.AreEqual(new DateTime(2018, 04, 22), new DateTime(2018, 04, 26).FirstDayOfWeek(DayOfWeek.Sunday));
        }

        [TestMethod]
        public void TestLastDayOfWeek()
        {
            Assert.AreEqual(new DateTime(2018, 04, 29), new DateTime(2018, 04, 26).LastDayOfWeek());
        }

        [TestMethod()]
        public void FirstDayOfMonthTest()
        {
            Assert.AreEqual(new DateTime(2018, 04,01), new DateTime(2018, 04, 11).FirstDayOfMonth());
        }

        [TestMethod()]
        public void LastDayOfMonthTest()
        {
            Assert.AreEqual(new DateTime(2018, 02, 28), new DateTime(2018, 02, 11).LastDayOfMonth());
        }

        [TestMethod()]
        public void DaysInMonthTest()
        {
            Assert.AreEqual(28, new DateTime(2018, 02, 28).DaysInMonth());
        }

        [TestMethod]
        public void DatesAreInTheSameWeekAndMonthTest()
        {
            var date1 = new DateTime(2018, 03, 01);
            var date2 = new DateTime(2018, 02, 28);
            Assert.IsFalse(date1.DatesAreInTheSameWeekOfYear(date2));
            date2 = new DateTime(2018, 03, 05);
            Assert.IsFalse(date1.DatesAreInTheSameWeekOfYear(date2));

            date2 = new DateTime(2018, 03, 04);
            Assert.IsTrue(date1.DatesAreInTheSameWeekOfYear(date2));
        }

        [TestMethod]
        public void DateStringifierTests()
        {
            var text = DateTime.Today.ToProximityString();
            Assert.IsTrue(text.ToLower().Contains("today"));

            text = DateTime.Today.AddDays(1).ToProximityString();
            Assert.IsTrue(text.ToLower().Contains("tomorrow"));

            text = DateTime.Today.AddDays(5).ToProximityString();
            Assert.IsTrue(text.ToLower().Contains("5 days ahead"));

            text = DateTime.Today.Subtract(TimeSpan.FromDays(1)).ToProximityString();
            Assert.IsTrue(text.ToLower().Contains("yesterday"));

            text = DateTime.Today.Subtract(TimeSpan.FromDays(3)).ToProximityString();
            Assert.IsTrue(text.ToLower().Contains("3 days ago"));

        }
    }
}
