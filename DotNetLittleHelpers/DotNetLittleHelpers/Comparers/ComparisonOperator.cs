namespace DotNetLittleHelpers
{
    using System;

    /// <summary>
    /// Holds the delegates for comparison operators (&gt;, &lt; =, etc)
    /// </summary>
    public static class ComparisonOperator
    {
        /// <summary>
        /// Return true if first is greater than second
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, T, bool> Greater<T>() where T : IComparable<T>
        {
            return (first, second) => first.CompareTo(second) > 0;
        }

        /// <summary>
        /// Return true if first is greater or equal to second
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, T, bool> GreaterOrEqual<T>() where T : IComparable<T>
        {
            return (first, second) => first.CompareTo(second) >= 0;
        }

        /// <summary>
        /// Return true if first is equal to second
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, T, bool> Equal<T>() where T : IComparable<T>
        {
            return (first, second) => first.CompareTo(second) == 0;
        }

        /// <summary>
        /// Return true if first is smaller than second
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, T, bool> Smaller<T>() where T : IComparable<T>
        {
            return (first, second) => first.CompareTo(second) < 0;
        }

        /// <summary>
        /// Return true if first is smaller or equal to second
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, T, bool> SmallerOrEqual<T>() where T : IComparable<T>
        {
            return (first, second) => first.CompareTo(second) <= 0;
        }

    }
}