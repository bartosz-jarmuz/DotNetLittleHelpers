namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;

    public static class MiscExtensions
    {
        /// <summary>
        ///     Gets string like st, nd, rd, th for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetOrdinalNumber(this int number)
        {
            string suffix = number.GetOrdinalSuffix();
            return number + suffix;
        }

        /// <summary>
        /// Select distinct elements based on the provided selector
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
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