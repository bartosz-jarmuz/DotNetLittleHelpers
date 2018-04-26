using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Extension methods for the Date & Time 
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the first day of the week of the specified date time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek(DayOfWeek.Monday);
        }

        /// <summary>
        /// Gets the first day of the week of the specified date time (using the specified day as 'first day'
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeekDay"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime dt, DayOfWeek startOfWeekDay)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeekDay)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the last day of the week of the specified date time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek().AddDays(6);
        }
    }
}
