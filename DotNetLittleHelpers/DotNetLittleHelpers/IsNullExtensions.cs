namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extensions for simpler 'is null' checks
    /// </summary>
    public static class IsNullExtensions
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
        /// Determines if guid a is Guid.Empty (which is equivalent to default(Guid) and to new Guid())
        /// </summary>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Determines if guid a is not Guid.Empty (which is equivalent to default(Guid) and to new Guid())
        /// </summary>
        public static bool IsNotEmpty(this Guid guid)
        {
            return guid != Guid.Empty;
        }

        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <param name="source"></param>
        /// <param name="argumentName"></param>
        /// <param name="methodName"></param>
        public static void ThrowIfNull(this object source, string argumentName, [CallerMemberName] string methodName = "")
        {
            if (source == null)
            {
                throw new ArgumentNullException(argumentName, $"Null parameter passed to method [{methodName}].");
            }
        }


        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <param name="source"></param>
        /// <param name="argumentName"></param>
        /// <param name="methodName"></param>
        public static void CheckArgumentNull(this object source, string argumentName, [CallerMemberName] string methodName = "")
        {
            source.ThrowIfNull(argumentName, methodName);
        }
    }
}