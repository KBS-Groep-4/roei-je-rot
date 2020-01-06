using System;
using Innovative.SolarCalculator;

namespace RoeiJeRot.Logic.Helper
{
    public static class DayChecker
    {
        private static SolarTimes GetSolorTimes(DateTime date)
        {
            return new SolarTimes(date, 52.1326, 5.2913);
        }

        public static bool IsDay(DateTime date, TimeSpan duration)
        {
            var solar = GetSolorTimes(date);

            return  date < solar.Sunset && date > solar.Sunrise && date + duration < solar.Sunset;
        }
    }
}