using System;
using System.Collections.Generic;
using System.Text;

namespace RoeiJeRot.Logic.Services
{
    public static class DateChecker
    {
        /// <summary>
        /// Function to check if 2 dates overlap
        /// </summary>
        /// <param name="a_start">Start date of A</param>
        /// <param name="a_duration">Duration of A</param>
        /// <param name="b_start">Start date of B</param>
        /// <param name="b_duration">Duration of B</param>
        /// <returns>If 2 dates overlap</returns>
        public static bool AvailableOn(DateTime a_start, TimeSpan a_duration, DateTime b_start, TimeSpan b_duration)
        {
            DateTime a_end = a_start + a_duration;
            DateTime b_end = b_start + b_duration;

            return a_end <= b_start || a_start >= b_end;
        }
    }
}
