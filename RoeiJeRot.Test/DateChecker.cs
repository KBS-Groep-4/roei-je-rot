using System;
using NUnit.Framework;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.Test
{
    [TestFixture]
    internal class DateCheckerTest
    {
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 14:00", 60, true)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 14:01", 20, true)]
        [TestCase("2020-4-9 13:01", 60, "2020-4-9 14:00", 60, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 13:10", 10, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 13:30", 30, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 13:00", 20, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 13:00", 60, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 12:40", 20, true)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 12:40", 21, false)]
        [TestCase("2020-4-9 13:00", 60, "2020-4-9 11:00", 20, true)]
        [TestCase("2020-4-9 13:20", 20, "2020-4-9 13:00", 60, false)]
        public void AvailibilityTest(string a_start, int a_duration, string b_start, int b_duration, bool expected)
        {
            Assert.AreEqual(expected, DateChecker.AvailableOn(DateTime.Parse(a_start),
                TimeSpan.FromMinutes(a_duration),
                DateTime.Parse(b_start),
                TimeSpan.FromMinutes(b_duration)
            ));
        }
    }
}