using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WowMacroEditorMVC.Models.Profile;

namespace WowMacroEditorMVCTests.Models.Profile
{
    [TestClass]
    public class IndexViewModelTests
    {
        [TestMethod]
        public void TimeToText_Seconds()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddSeconds(-5));
            Assert.AreEqual("5 seconds", text);
        }

        [TestMethod]
        public void TimeToText_Minutes()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddMinutes(-5));
            Assert.AreEqual("5 minutes", text);
        }

        [TestMethod]
        public void TimeToText_Hours()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddHours(-5));
            Assert.AreEqual("5 hours", text);
        }

        [TestMethod]
        public void TimeToText_Days()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddDays(-5));
            Assert.AreEqual("5 days", text);
        }

        [TestMethod]
        public void TimeToText_Months()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddMonths(-5));
            Assert.AreEqual("5 months", text);
        }

        [TestMethod]
        public void TimeToText_YearsAndMonths()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddMonths(-14));
            Assert.AreEqual("1 year, 2 months", text);
        }

        [TestMethod]
        public void TimeToText_NoPluralization()
        {
            string text = IndexViewModel.TimeToText(DateTime.Now.AddMonths(-1));
            Assert.AreEqual("1 month", text);
        }

        
    }
}
