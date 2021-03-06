﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    using System;
    using System.Reflection.Emit;

    [TestClass()]
    public class EnumHelpersTests
    {
        private enum WeekDays
        {
            [System.ComponentModel.Description("The happy day")]
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        [TestMethod]
        public void TestIncrease()
        {
            Assert.AreEqual(WeekDays.Friday, WeekDays.Tuesday.IncreaseValue(3));
            Assert.AreEqual(WeekDays.Tuesday, EnumHelper.IncreaseValue(WeekDays.Tuesday, 0));
            Assert.AreEqual(WeekDays.Sunday, WeekDays.Tuesday.IncreaseValue(22));
            try
            {
                EnumHelper.IncreaseValue(WeekDays.Tuesday, 22, true);
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "The modifier 22 exceeds the maximum possible value for WeekDays when applied on Tuesday");
            }
        }



        [TestMethod]
        public void TestDecrease()
        {
            Assert.AreEqual(WeekDays.Tuesday, EnumHelper.DecreaseValue(WeekDays.Friday, 3));
            Assert.AreEqual(WeekDays.Tuesday, EnumHelper.DecreaseValue(WeekDays.Tuesday, 0));
            Assert.AreEqual(WeekDays.Monday, EnumHelper.DecreaseValue(WeekDays.Tuesday, 22));
            try
            {
                EnumHelper.DecreaseValue(WeekDays.Tuesday, 22, true);
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "The modifier 22 exceeds the minimum possible value for WeekDays when applied on Tuesday");
            }
        }

        [TestMethod]
        public void TestGetDescription()
        {
            Assert.AreEqual("The happy day", WeekDays.Monday.GetDescription());
            Assert.AreEqual("Tuesday", WeekDays.Tuesday.GetDescription());

        }
    }
}