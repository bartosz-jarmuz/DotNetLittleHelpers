namespace DotNetLittleHelpers
{
    /// <summary>
    /// Miscellaneous extensions for numeric types
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        ///     Gets string like 1st, 2nd, 3rd, 5th for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetOrdinalNumber(this int number)
        {
            string suffix = number.GetOrdinalSuffix();
            return number + suffix;
        }

        /// <summary>
        ///     Gets string like st, nd, rd, th for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetOrdinalSuffix(this int number)
        {
            if (number <= 0) return "";

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}