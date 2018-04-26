﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFirstDayOfWeek()
        {
            Assert.AreEqual(new DateTime(2018, 04,23), new DateTime(2018, 04, 26).FirstDayOfWeek());
            Assert.AreEqual(new DateTime(2018, 04,23), new DateTime(2018, 04, 23).FirstDayOfWeek());
            Assert.AreEqual(new DateTime(2018, 04,22), new DateTime(2018, 04, 26).FirstDayOfWeek(DayOfWeek.Sunday));
            
        }
    }
}
