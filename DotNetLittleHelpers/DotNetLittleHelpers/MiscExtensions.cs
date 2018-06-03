namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

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
        /// <para>Get a string of names and values of properties (for logging/display purpose). The format is as follows:</para>
        /// "NameProperty: [JimBeam], SomethingNUll: [*NULL*], NonExistentProperty: [*NO SUCH PROPERTY*], SomeBooleanProperty: [True]"  <para/>
        /// This uses reflection.
        /// Performance-wise - executing it 100000 (hundred thousand) times on an object with 5 properties takes on average 430ms
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string GetPropertyInfoString(this object source, params string[] propertyNames)
        {
            if (source == null)
            {
                return null;
            }
            Type type = source.GetType();
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < propertyNames.Length; index++)
            {
                string propertyName = propertyNames[index];
                PropertyInfo propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    sb.Append($"{propertyName}: [{propertyInfo.GetValue(source)??"*NULL*"}]");
                }
                else
                {
                    sb.Append($"{propertyName}: [*NO SUCH PROPERTY*]");
                }

                if (index < propertyNames.Length-1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// <para>Get a string of names and values of properties (for logging/display purpose). The format is as follows:</para>
        /// "NameProperty: [JimBeam], SomethingNUll: [*NULL*], NonExistentProperty: [*NO SUCH PROPERTY*], SomeBooleanProperty: [True]"  <para/>
        /// This uses reflection.
        /// Performance-wise - executing it 100000 (hundred thousand) times on an object with 5 properties takes on average 430ms
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetPropertyInfoString(this object source)
        {
            if (source == null)
            {
                return null;
            }
            Type type = source.GetType();
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = type.GetProperties();
            for (int index = 0; index < props.Length; index++)
            {
                PropertyInfo propertyInfo = props[index];
                sb.Append($"{propertyInfo.Name}: [{propertyInfo.GetValue(source) ?? "*NULL*"}]");

                if (index < props.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
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