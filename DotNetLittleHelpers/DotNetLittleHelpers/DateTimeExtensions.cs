namespace DotNetLittleHelpers
{
    #region Using
    using System;
    using System.Globalization;
    #endregion

    /// <summary>
    ///     Extension methods for the Date &amp; Time
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Checks whether two dates are in the same week of year
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool DatesAreInTheSameWeekOfYear(this DateTime date1, DateTime date2)
        {
            if (date1.Month != date2.Month)
            {
                return false;
            }

            var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = cal.GetWeekOfYear(date1, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var d2 = cal.GetWeekOfYear(date2, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return d1 == d2;
        }

        /// <summary>
        ///     Gets the number of days in the month of the specified date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        /// <summary>
        ///     Gets the first day for a month of the specified date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, day: 1);
        }

        /// <summary>
        ///     Gets the first day of the week of the specified date time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek(DayOfWeek.Monday);
        }

        /// <summary>
        ///     Gets the first day of the week of the specified date time (using the specified day as 'first day'
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeekDay"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime dt, DayOfWeek startOfWeekDay)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeekDay)) % 7;
            return dt.AddDays(value: -1 * diff).Date;
        }

        /// <summary>
        ///     Gets string like 1st, 2nd, 3rd, 4th etc
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetDayNumberWithsSuffix(this DateTime day)
        {
            var daySuffix = day.GetDaySuffix();
            return day.Day + daySuffix;
        }

        /// <summary>
        ///     Gets string like st, nd, rd, th etc
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetDaySuffix(this DateTime day)
        {
            switch (day.Day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        /// <summary>
        ///     Gets the number of the week in given month
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, day: 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        /// <summary>
        ///     Gets the week number in the Gregorian Calendar for the specified date
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(this DateTime time)
        {
            GregorianCalendar _gc = new GregorianCalendar();

            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        /// <summary>
        ///     Gets the last day for a month of the specified date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, day: value.DaysInMonth());
        }

        /// <summary>
        ///     Gets the last day of the week of the specified date time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek().AddDays(value: 6);
        }

        /// <summary>
        /// Return string like yesteday, today, tomorrow or 22.02.2018 (3 days ago)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToProximityString(this DateTime date)
        {
            if (date.Date == DateTime.Today.Date)
            {
                return "TODAY";
            }
            else if (date.Date == DateTime.Today.Date.AddDays(1))
            {
                return "TOMORROW";
            }
            else if (date.Date == DateTime.Today.Date.Subtract(TimeSpan.FromDays(1)))
            {
                return "YESTERDAY";
            }
            else
            {
                if (date.Date > DateTime.Today.Date)
                {
                    return $" ({(date.Date - DateTime.Today.Date).TotalDays} days ahead)";
                }
                else
                {
                    return $" ({(DateTime.Today.Date - date.Date).TotalDays} days ago)";
                }
            }
        }
    }
}