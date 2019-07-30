namespace DotNetLittleHelpers
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Various extension methods functioning on string
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Verifies whether a string is a valid email address (Microsoft way)
        /// <para>https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format</para>
        /// </summary>
        /// <param name="maybeEmail"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string maybeEmail)
        {
            bool invalid = false;
            if (String.IsNullOrEmpty(maybeEmail))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                maybeEmail = Regex.Replace(maybeEmail, @"(@)(.+)$", x=> DomainMapper(x, ref invalid),
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(maybeEmail,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts the first character to lower case - CultureInvariant
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToLowerFirstChar(this string input)
        {
            if (input == null)
                return null;
            if (input == "")
                return "";
           return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        /// Converts the first character to upper case - CultureInvariant
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToUpperFirstChar(this string input)
        {
            if (input == null)
                return null;
            if (input == "")
                return "";
            return char.ToUpperInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        /// Trims the substring from the end of the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="suffixToRemove">The suffix to remove.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>System.String.</returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.Ordinal)
        {

            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

        private static string DomainMapper(Match match, ref bool invalid)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}