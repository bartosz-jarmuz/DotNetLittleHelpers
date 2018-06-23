namespace DotNetLittleHelpers
{
    using System;
    using System.Collections;
    using System.Reflection;

    /// <summary>
    /// Extension methods for the Type class
    /// https://stackoverflow.com/a/844855/2892378
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns true if a property is an IEnumerable (notice that string is Enumerable as well, but is excluded from this logic)
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsNonStringEnumerable(this PropertyInfo pi)
        {
            return pi != null && pi.PropertyType.IsNonStringEnumerable();
        }

        /// <summary>
        /// Returns true if a object is an IEnumerable (notice that string is Enumerable as well, but is excluded from this logic)
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNonStringEnumerable(this object instance)
        {
            return instance != null && instance.GetType().IsNonStringEnumerable();
        }

        /// <summary>
        /// Returns true if a type is an IEnumerable (notice that string is Enumerable as well, but is excluded from this logic)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonStringEnumerable(this Type type)
        {
            if (type == null || type == typeof(string))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive"/>
        public static bool IsSimpleType(
            this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                ((IList) new[]
                {
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }).Contains(type) ||
                (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        /// <summary>
        /// Gets the underlying type from the member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}