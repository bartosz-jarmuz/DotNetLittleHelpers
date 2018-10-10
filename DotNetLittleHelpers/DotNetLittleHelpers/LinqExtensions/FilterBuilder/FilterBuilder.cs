using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{

    /// <summary>
    /// Class WhereExpressionBuilder.
    /// https://www.codeproject.com/Tips/582450/Build-Where-Clause-Dynamically-in-Linq
    /// </summary>
    public static class FilterBuilder
    {
        /// <summary>
        /// The contains method
        /// </summary>
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");
        /// <summary>
        /// The starts with method
        /// </summary>
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        /// <summary>
        /// The ends with method
        /// </summary>
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        /// <summary>
        /// Gets the compiled expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters">The filters.</param>
        /// <returns>Func&lt;T, System.Boolean&gt;.</returns>
        public static Func<T, bool> GetCompiled<T>(IList<Filter> filters)
        {
            return GetExpression<T>(filters).Compile();
        }
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters">The filters.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(param, filters[0], filters[1]);
            }
            else
            {
                while (filters.Count > 0)
                {
                    Filter f1 = filters[0];
                    Filter f2 = filters[1];

                    if (exp == null)
                    {
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    }
                    else
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Expression.</returns>
        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Comparison)
            {
                case ComparisonRule.Equals:
                    return Expression.Equal(member, constant);

                case ComparisonRule.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case ComparisonRule.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case ComparisonRule.LessThan:
                    return Expression.LessThan(member, constant);

                case ComparisonRule.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case ComparisonRule.Contains:
                    return Expression.Call(member, ContainsMethod, constant);

                case ComparisonRule.StartsWith:
                    return Expression.Call(member, StartsWithMethod, constant);

                case ComparisonRule.EndsWith:
                    return Expression.Call(member, EndsWithMethod, constant);
            }

            return null;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="filter1">The filter1.</param>
        /// <param name="filter2">The filter2.</param>
        /// <returns>BinaryExpression.</returns>
        private static BinaryExpression GetExpression<T> (ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }
    }
}
