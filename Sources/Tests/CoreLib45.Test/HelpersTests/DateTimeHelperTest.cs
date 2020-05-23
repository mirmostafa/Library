#region Code Identifications

// Created on     2018/04/29
// Last update on 2018/04/29 by Mohammad Mir mostafa 

#endregion

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Helpers;

namespace CoreLib45.Test.HelpersTests
{
    [TestClass]
    public class DateTimeHelperTest
    {
        [TestMethod]
        public void TestCatch1()
        {
            var now = DateTime.Now;
            var tomorrow = now.AddDays(1);
            var yesterday = now.AddDays(-1);
            Assert.IsTrue(now.IsBetween(yesterday, tomorrow));
            Assert.IsTrue(now.ToTimeSpan().IsBetween(yesterday.ToTimeSpan(), tomorrow.ToTimeSpan()));
            Assert.IsTrue(now.ToTimeSpan().IsBetween(yesterday.ToTimeSpan().ToString(), tomorrow.ToTimeSpan().ToString()));
            Assert.AreEqual(now.ToTimeSpan().ToDateTime(), now);
            Assert.IsTrue(DateTimeHelper.IsValid(now.ToString(CultureInfo.InvariantCulture)));
        }
    }
}