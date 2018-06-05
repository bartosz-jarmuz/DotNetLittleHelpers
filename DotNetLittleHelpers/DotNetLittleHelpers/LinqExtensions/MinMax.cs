namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions for LinqRelatedMethods
    /// </summary>
    public static class MinMaxLinqExtensions
    {

        /// <summary>
        /// Returns the maximum element of the sequence according to the specified selector - or default value if the source collection is empty. <para/>
        /// Throw ArgumentNullException if source is null
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.CheckArgumentNull(nameof(source));
            return LinqExtensions.FunctionWrapper(source, src => src.Max(selector));
        }

        /// <summary>
        /// Returns the maximum element of the sequence according to the specified selector - or default value if the source collection is empty or null. <para/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxOrDefaultIfNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return LinqExtensions.FunctionWrapper(source, src => src.Max(selector)) ;
        }

        /// <summary>
        /// Returns the maximum element of the sequence according to the specified selector - or null if the source collection is empty. <para/>
        /// Throw ArgumentNullException if source is null
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MaxOrNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) where TResult : struct, IComparable
        {
            source.CheckArgumentNull(nameof(source));
            return LinqExtensions.NullableFunctionWrapper(source, src => src.Max(selector));
        }

        /// <summary>
        /// Returns the maximum element of the sequence according to the specified selector - or null if the source collection is empty or null. <para/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MaxOrNullIfNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) where TResult : struct, IComparable
        {
            return LinqExtensions.NullableFunctionWrapper(source, src => src.Max(selector));
        }

        /// <summary>
        /// Returns the minimum element of the sequence according to the specified selector - or default value if the source collection is empty.
        /// </summary>
        /// <remarks>
        /// Throw ArgumentNullException if source is null
        /// </remarks>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.CheckArgumentNull(nameof(source));
            return LinqExtensions.FunctionWrapper(source, src => src.Min(selector));
        }

        /// <summary>
        /// Returns the minimum element of the sequence according to the specified selector - or default value if the source collection is empty or null. <para/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MinOrDefaultIfNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return LinqExtensions.FunctionWrapper(source, src => src.Min(selector));
        }


        /// <summary>
        /// Returns the minimum element of the sequence according to the specified selector - or null if the source collection is empty.
        /// </summary>
        /// <remarks>
        /// Throw ArgumentNullException if source is null
        /// </remarks>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MinOrNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) where TResult : struct, IComparable
        {
            source.CheckArgumentNull(nameof(source));
            return LinqExtensions.NullableFunctionWrapper(source, src => src.Min(selector));
        }

        /// <summary>
        /// Returns the minimum element of the sequence according to the specified selector - or null if the source collection is empty or null.
        /// </summary>
        /// <remarks>
        /// Throw ArgumentNullException if source is null
        /// </remarks>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MinOrNullIfNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) where TResult : struct, IComparable
        {
            return LinqExtensions.NullableFunctionWrapper(source, src => src.Min(selector));
        }


    }
}