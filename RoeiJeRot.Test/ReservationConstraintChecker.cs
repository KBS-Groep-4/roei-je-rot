using System;
using NUnit.Framework;
using RoeiJeRot.Logic.Helper;

namespace RoeiJeRot.Test
{
    [TestFixture]
    public class ReservationConstraintChecker
    {
        [TestCase("2020-11-18 13:00+01:00", 120, true)]
        [TestCase("2020-11-18 20:00+01:00", 120, false)]
        [TestCase("2020-11-18 23:00+01:00", 120, false)]
        [TestCase("2020-11-18 8:05+01:00", 120, true)]
        [TestCase("2020-11-18 8:00+01:00", 120, false)]
        [TestCase("2020-11-18 16:34+01:00", 120, false)]
        [Test]
        public void dayTimeCheckerTest(string date, int duration, bool expected)
        {
            Assert.AreEqual(DayChecker.IsDay(DateTime.Parse(date), TimeSpan.FromMinutes(duration)), expected);
        }
    }
}