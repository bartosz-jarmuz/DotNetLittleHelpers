namespace DotNetLittleHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// Extensions for LinqRelatedMethods
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Returns true if the sequence contains one and only one element which meets the predicate. Otherwise return false
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool OneExists<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            source.CheckArgumentNull(nameof(source));
            bool oneFound = false;
            foreach (TSource element in source)
            {
                if (predicate.Invoke(element))
                {
                    if (oneFound)
                    {
                        return false;
                    }

                    oneFound = true;
                }
            }

            return oneFound;
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
            source.CheckArgumentNull(nameof(source));
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        internal static TResult FunctionWrapper<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, TResult> function)
        {
            if (source == null)
            {
                return default(TResult);
            }
            IList<TSource> list = source as IList<TSource>;
            if (!list.Any())
            {
                return default(TResult);
            }

            return function(list);
        }

        internal static TResult? NullableFunctionWrapper<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, TResult> function) where TResult: struct, IComparable
        {
            if (source == null)
            {
                return default(TResult?);
            }
            IList<TSource> list = source as IList<TSource>;
            if (!list.Any())
            {
                return default(TResult?);
            }

            return function(list);

        }

    }
}