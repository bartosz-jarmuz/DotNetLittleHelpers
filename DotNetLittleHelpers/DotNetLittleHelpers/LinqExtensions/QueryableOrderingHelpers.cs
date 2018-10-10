using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// QueryableOrderingHelpers
    /// </summary>
    public static class QueryableOrderingHelpers
    {

        /// <summary>
        /// Orderings the helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="descending">if set to <c>true</c> [descending].</param>
        /// <param name="anotherLevel">if set to <c>true</c> [another level].</param>
        /// <returns>IOrderedQueryable&lt;T&gt;.</returns>
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        /// <summary>
        /// Orders the collection based on number of criteria in sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyFilters">The property filters.</param>
        /// <returns>IOrderedQueryable&lt;T&gt;.</returns>
        public static IOrderedQueryable<T> OrderByMany<T>(this IQueryable<T> source, params Tuple<string, bool>[] propertyFilters)
        {
            return source.OrderByMany(propertyFilters.Select(x => new OrderRule(x.Item1, x.Item2)).ToArray());
        }

        /// <summary>
            /// Orders the collection based on number of criteria in sequence
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="source">The source.</param>
            /// <param name="propertyFilters">The property filters.</param>
            /// <returns>IOrderedQueryable&lt;T&gt;.</returns>
            public static IOrderedQueryable<T> OrderByMany<T>(this IQueryable<T> source, params OrderRule[] propertyFilters)
        {
            var filters = propertyFilters.ToList();

            if (filters.Count == 1)
            {
                return source.OrderBy(filters[0].Property, filters[0].Descending);
            }
            else if (filters.Count == 2)
            {
                return source.OrderBy(filters[0].Property, filters[0].Descending).ThenBy(filters[1].Property, filters[1].Descending);
            }
            else
            {
                var queryable = OrderingHelper(source, filters[0].Property, filters[0].Descending, false);
                filters.RemoveAt(0);

                while (filters.Count > 0)
                {
                    queryable = OrderingHelper(queryable, filters[0].Property, filters[0].Descending, true);

                    filters.RemoveAt(0);

                    if (filters.Count == 1)
                    {
                        queryable = OrderingHelper(queryable, filters[0].Property, filters[0].Descending, true);
                        break;
                    }
                }

                return queryable ?? source as IOrderedQueryable<T>;
            }
        }

        /// <summary>
        /// Orders the collection based on a property with specified name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="descending">Ordering order</param>
        /// <returns>IOrderedQueryable&lt;T&gt;.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool descending = false)
        {
            return OrderingHelper(source, propertyName, descending, false);
        }

        /// <summary>
        /// Another level of ordering by a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="descending">if set to <c>true</c> [descending].</param>
        /// <returns>IOrderedQueryable&lt;T&gt;.</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, bool descending = false)
        {
            return OrderingHelper(source, propertyName, descending, true);
        }
    }
}