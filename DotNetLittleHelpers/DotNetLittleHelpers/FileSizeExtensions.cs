using System;

namespace DotNetLittleHelpers
{
    using System.Globalization;

    /// <summary>
    /// Extensions for calculating file size
    /// </summary>
    public static class FileSizeExtensions
    {
        /// <summary>
        /// Converts bytes to kilobytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static double ConvertBytesToKilobytes(this long bytes, int decimalPlaces = 2)
        {
            return Math.Round(bytes / 1024f, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Converts bytes to megabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static double ConvertBytesToMegabytes(this long bytes, int decimalPlaces = 2)
        {
            return Math.Round(bytes / 1024f / 1024f, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Gets the file / folder size string in largest unit (KB, MB, GB etc)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>System.String.</returns>
        public static string GetSizeString(this Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + GetSizeString(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000 && i < 5)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}
