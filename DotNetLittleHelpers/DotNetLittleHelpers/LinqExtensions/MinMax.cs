// -----------------------------------------------------------------------
//  <copyright file="MinMax.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace DotNetLittleHelpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    /// <summary>
    ///     Extensions for LinqRelatedMethods
    /// </summary>
    public static class MinMaxLinqExtensions
    {
        /// <summary>
        ///     Returns the elements of the list which property specified by the selector meets the extreme (e.g. min or max)
        ///     criteria specified by the operator
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExtremeBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector, Func<int, int, bool> @operator)
            where TDeterminer : IComparable<TDeterminer>
        {
            source.CheckArgumentNull(nameof(source));

            bool listIsEmpty = true;
            bool evaluatingFirstItem = true;
            List<TSource> returnList = new List<TSource>();
            TDeterminer maxDeterminer = default(TDeterminer);
            foreach (TSource item in source)
            {
                listIsEmpty = false;
                if (evaluatingFirstItem)
                {
                    returnList.Add(item);
                    maxDeterminer = selector(item);
                    evaluatingFirstItem = false;
                }
                else
                {
                    TDeterminer determiner = selector(item);
                    int comparisonResult = determiner.CompareTo(maxDeterminer);
                    if (@operator(comparisonResult, 0))
                    {
                        returnList.Clear();
                        maxDeterminer = determiner;
                        returnList.Add(item);
                    }
                    else if (comparisonResult == 0)
                    {
                        returnList.Add(item);
                    }
                }
            }

            if (listIsEmpty)
            {
                throw new ArgumentException("Source collection is empty.");
            }

            return returnList;
        }

        /// <summary>
        ///     Returns the first element of the list which property specified by the selector meets the extreme (e.g. min or max)
        ///     criteria specified by the operator
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static TSource ExtremeFirstBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector, Func<int, int, bool> @operator)
            where TDeterminer : IComparable<TDeterminer>
        {
            source.CheckArgumentNull(nameof(source));

            bool listIsEmpty = true;
            bool evaluatingFirstItem = true;
            TSource returnObject = default(TSource);
            TDeterminer extremeDeterminer = default(TDeterminer);
            foreach (TSource item in source)
            {
                listIsEmpty = false;
                if (evaluatingFirstItem)
                {
                    returnObject = item;
                    extremeDeterminer = selector(returnObject);
                    evaluatingFirstItem = false;
                }
                else
                {
                    TDeterminer determiner = selector(item);
                    if (@operator(determiner.CompareTo(extremeDeterminer), 0))
                    {
                        extremeDeterminer = determiner;
                        returnObject = item;
                    }
                }
            }

            if (listIsEmpty)
            {
                throw new ArgumentException("Source collection is empty.");
            }

            return returnObject;
        }

        /// <summary>
        ///     Returns the elements from the sequence which property are a Max compared to other elements properties
        ///     <para />
        ///     If more elements have the same value, the first one is returned
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> MaxBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector) where TDeterminer : IComparable<TDeterminer>
        {
            return source.ExtremeBy(selector, ComparisonOperator.Greater<int>());
        }

        /// <summary>
        ///     Returns the element from the sequence which property is a Max compared to other elements properties
        ///     <para />
        ///     If more elements have the same value, the first one is returned
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TSource MaxFirstBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector) where TDeterminer : IComparable<TDeterminer>
        {
            return source.ExtremeFirstBy(selector, ComparisonOperator.Greater<int>());
        }

        /// <summary>
        ///     Returns the maximum element of the sequence according to the specified selector - or default value if the source
        ///     collection is empty.
        ///     <para />
        ///     Throw ArgumentNullException if source is null
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
        ///     Returns the maximum element of the sequence according to the specified selector - or default value if the source
        ///     collection is empty or null.
        ///     <para />
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxOrDefaultIfNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return LinqExtensions.FunctionWrapper(source, src => src.Max(selector));
        }

        /// <summary>
        ///     Returns the maximum element of the sequence according to the specified selector - or null if the source collection
        ///     is empty.
        ///     <para />
        ///     Throw ArgumentNullException if source is null
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
        ///     Returns the maximum element of the sequence according to the specified selector - or null if the source collection
        ///     is empty or null.
        ///     <para />
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
        ///     Returns the elements from the sequence which property are a Min compared to other elements properties
        ///     <para />
        ///     If more elements have the same value, the first one is returned
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> MinBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector) where TDeterminer : IComparable<TDeterminer>
        {
            return source.ExtremeBy(selector, ComparisonOperator.Smaller<int>());
        }

        /// <summary>
        ///     Returns the element from the sequence which property is a Min compared to other elements properties
        ///     <para />
        ///     If more elements have the same value, the first one is returned
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDeterminer"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TSource MinFirstBy<TSource, TDeterminer>(this IEnumerable<TSource> source, Func<TSource, TDeterminer> selector) where TDeterminer : IComparable<TDeterminer>
        {
            return source.ExtremeFirstBy(selector, ComparisonOperator.Smaller<int>());
        }

        /// <summary>
        ///     Returns the minimum element of the sequence according to the specified selector - or default value if the source
        ///     collection is empty.
        /// </summary>
        /// <remarks>
        ///     Throw ArgumentNullException if source is null
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
        ///     Returns the minimum element of the sequence according to the specified selector - or default value if the source
        ///     collection is empty or null.
        ///     <para />
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
        ///     Returns the minimum element of the sequence according to the specified selector - or null if the source collection
        ///     is empty.
        /// </summary>
        /// <remarks>
        ///     Throw ArgumentNullException if source is null
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
        ///     Returns the minimum element of the sequence according to the specified selector - or null if the source collection
        ///     is empty or null.
        /// </summary>
        /// <remarks>
        ///     Throw ArgumentNullException if source is null
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