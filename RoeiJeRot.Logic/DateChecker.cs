using System;
using System.Collections.Generic;
using System.Text;

namespace RoeiJeRot.Logic.Services
{
    public static class DateChecker
    {
        public static bool AvailableOn(DateTime a_start, TimeSpan a_duration, DateTime b_start, TimeSpan b_duration)
        {
            DateTime a_end = a_start + a_duration;
            DateTime b_end = b_start + b_duration;

            return a_end <= b_start || a_start >= b_end;
        }
    }
}
