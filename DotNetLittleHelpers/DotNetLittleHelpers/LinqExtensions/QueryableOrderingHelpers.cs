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