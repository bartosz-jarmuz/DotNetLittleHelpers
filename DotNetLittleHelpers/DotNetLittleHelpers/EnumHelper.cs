using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Class EnumHelper.
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Decreases the value of an enum by the specified number of steps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEnum">The enum.</param>
        /// <param name="modifier">The modifier.</param>
        /// <param name="throwIfMinValueExceeded">If set to true, if the modified value exceeds the maximum value of the enum, an error is thrown. Otherwise max is returned</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentException">The requested type [{typeof(T).Name}</exception>
        public static T DecreaseValue<T>(T theEnum, uint modifier, bool throwIfMinValueExceeded = false) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"The requested type [{typeof(T).Name}] is not an enum!");
            }

            T last = Enum.GetValues(typeof(T)).Cast<T>().Min();
            int lastValue = Convert.ToInt32(last);
            uint originalModifier = modifier;
            int value = (Convert.ToInt32(theEnum) - (int)modifier);

            while (!Enum.IsDefined(typeof(T), value))
            {
                if (throwIfMinValueExceeded)
                {
                    if (value < lastValue)
                    {
                        throw new Exception($"The modifier {originalModifier} exceeds the minimum possible value for {typeof(T).Name} when applied on {theEnum}");
                    }
                }
                else
                {
                    if (value <= lastValue)
                    {
                        return last;
                    }
                }
                modifier--;
                value = (Convert.ToInt32(theEnum) + (int)modifier);
            }

            return (T)Enum.Parse(typeof(T), value.ToString());
        }

        /// <summary>
        /// Increases the value of an enum by the specified number of steps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEnum">The enum.</param>
        /// <param name="modifier">The modifier.</param>
        /// <param name="throwIfMaxValueExceeded">If set to true, if the modified value exceeds the maximum value of the enum, an error is thrown. Otherwise max is returned</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentException">The requested type [{typeof(T).Name}</exception>
        public static T IncreaseValue<T>(T theEnum, uint modifier, bool throwIfMaxValueExceeded = false) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"The requested type [{typeof(T).Name}] is not an enum!");
            }

            int value = (Convert.ToInt32(theEnum) + (int)modifier);
            uint originalModifier = modifier;
            T last = Enum.GetValues(typeof(T)).Cast<T>().Max();
            int lastValue = Convert.ToInt32(last);
            while (!Enum.IsDefined(typeof(T), value))
            {
                if (modifier == 0)
                {
                    return theEnum;
                }

                if (throwIfMaxValueExceeded)
                {
                    if (value > lastValue)
                    {
                        throw new Exception($"The modifier {originalModifier} exceeds the maximum possible value for {typeof(T).Name} when applied on {theEnum}");
                    }
                }
                else
                {
                    if (value >= lastValue)
                    {
                        return last;
                    }
                }

                modifier--;
                value = (Convert.ToInt32(theEnum) + (int)modifier);
            }

            return (T)Enum.Parse(typeof(T), value.ToString());
        }
    }
}
