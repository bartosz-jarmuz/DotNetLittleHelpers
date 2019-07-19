using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Some extension methods which can be useful for logging/debugging
    /// </summary>
    public static class LoggingExtensions
    {
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
            AppendPropertyString(source, props, sb);



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

            Dictionary<string, PropertyInfo> props = type.GetProperties().OrderBy(x => x.Name).ToDictionary(p => p.Name, p => p);
            AppendPropertyString(source, props, sb);


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
            Dictionary<string, PropertyInfo> props = type.GetProperties().Where(x => x.Name.ToLowerInvariant().Contains("name") || x.Name.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.Name).ToDictionary(p => p.Name, p => p);

            AppendPropertyString(source, props, sb);

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
    }
}