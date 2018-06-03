namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();

            foreach (string propertyName in propertyNames)
            {
                props.Add(propertyName, type.GetProperty(propertyName));
            }
            MiscExtensions.AppendPropertyString(source, props, sb);



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

            Dictionary<string, PropertyInfo> props = type.GetProperties().ToDictionary(p => p.Name, p => p);
            MiscExtensions.AppendPropertyString(source, props, sb);


            return sb.ToString();
        }

        /// <summary>
        /// Gets a string of name and values of properties which names contain 'Name' or end with 'Id' (case insensitive) <para/>
        /// "Name: [JimBeam], UserId: [34]"
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetNameAndIdString(this object source)
        {
            if (source == null)
            {
                return null;
            }
            Type type = source.GetType();
            StringBuilder sb = new StringBuilder();
           Dictionary<string, PropertyInfo> props = type.GetProperties().Where(x=>x.Name.ToLowerInvariant().Contains("name") || x.Name.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase)).ToDictionary(p=>p.Name, p=>p);

            MiscExtensions.AppendPropertyString(source, props, sb);

            return sb.ToString();
        }

        private static void AppendPropertyString(object source, Dictionary<string, PropertyInfo> props, StringBuilder sb)
        {
            for (int index = 0; index < props.Count; index++)
            {
                PropertyInfo propertyInfo = props.ElementAt(index).Value;
                if (propertyInfo != null)
                {
                    sb.Append($"{propertyInfo.Name}: [{propertyInfo.GetValue(source) ?? "*NULL*"}]");
                }
                else
                {
                    sb.Append($"{props.ElementAt(index).Key}: [*NO SUCH PROPERTY*]");
                }

                if (index < props.Count - 1)
                {
                    sb.Append(", ");
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