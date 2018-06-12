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
        /// Returns false if collection is null or empty<para/>
        /// Syntactic sugar for if (source != null &amp;&amp; source.Any())
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AnyAndNotNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return false;
            }
            else
            {
                return source.Any();
            }
        }

        /// <summary>
        /// Returns true if collection is null or empty<para/>
        /// Syntactic sugar for if (source == null || !source.Any())
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return !source.AnyAndNotNull();
        }

        /// <summary>
        /// Returns true if a value type is either null or a default for that type (default(T))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrDefault<T>(this T? source) where T : struct
        {
            return !source.HasValue || source.Value.Equals(default(T));
        }

        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <param name="source"></param>
        /// <param name="argumentName"></param>
        /// <param name="methodName"></param>
        public static void CheckArgumentNull(this object source, string argumentName, [CallerMemberName] string methodName = "")
        {
            if (source == null)
            {
                throw new ArgumentNullException(argumentName,$"Null parameter passed to method [{methodName}].");
            }
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