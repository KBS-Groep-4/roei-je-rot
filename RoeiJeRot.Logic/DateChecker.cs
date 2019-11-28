﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RoeiJeRot.Logic.Services
{
    public static class DateChecker
    {
        /// <summary>
        /// Checks if two dates overlap.
        /// </summary>
        /// <param name="aStart">Start date of A</param>
        /// <param name="aDuration">Duration of A</param>
        /// <param name="bStart">Start date of B</param>
        /// <param name="bDuration">Duration of B</param>
        /// <returns>If 2 dates overlap</returns>
        public static bool AvailableOn(DateTime aStart, TimeSpan aDuration, DateTime bStart, TimeSpan bDuration)
        {
            DateTime aEnd = aStart + aDuration;
            DateTime bEnd = bStart + bDuration;

            return aEnd <= bStart || aStart >= bEnd;
        }
    }
}
