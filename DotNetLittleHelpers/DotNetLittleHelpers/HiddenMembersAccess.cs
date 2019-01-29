using System;
using System.Linq;
using System.Reflection;

namespace DotNetLittleHelpers
{
    /// <summary>
    ///     Allows access to hidden (private, internal) members through Reflection.
    ///     <para />
    ///     This should be used in tests and hacks rather than 'proper' code
    /// </summary>
    public static class HiddenMembersAccess
    {
        /// <summary>
        ///     Gets the field value.
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <param name="source">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">fieldName - Couldn't find field {fieldName} in type {sourceType.FullName}</exception>
        public static T GetFieldValue<T>(this object source, string fieldName)
        {
            source.ThrowIfNull(nameof(source));

            var sourceType = source.GetType();
            var fieldInfo = GetFieldInfo(sourceType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(nameof(fieldName)
                    , $"Couldn't find field {fieldName} in type {sourceType.FullName}");
            return (T) fieldInfo.GetValue(source);
        }

        /// <summary>
        ///     Gets the property value.
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     propertyName - Couldn't find property {propertyName} in type
        ///     {sourceType.FullName}
        /// </exception>
        public static T GetPropertyValue<T>(this object source, string propertyName)
        {
            source.ThrowIfNull(nameof(source));

            var sourceType = source.GetType();

            var propInfo = GetPropertyInfo(sourceType, propertyName);

            if (propInfo == null)
                throw new ArgumentOutOfRangeException(nameof(propertyName)
                    , $"Couldn't find property {propertyName} in type {sourceType.FullName}");

            return (T) propInfo.GetValue(source, null);
        }

        /// <summary>
        ///     Sets the field value.
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">fieldName - Couldn't find field {fieldName} in type {sourceType.FullName}</exception>
        public static void SetFieldValue(this object source, string fieldName, object val)
        {
            source.ThrowIfNull(nameof(source));

            var sourceType = source.GetType();
            var fieldInfo = GetFieldInfo(sourceType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(nameof(fieldName)
                    , $"Couldn't find field {fieldName} in type {sourceType.FullName}");
            fieldInfo.SetValue(source, val);
        }

        /// <summary>
        ///     Sets the property value.
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <param name="source">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     propertyName - Couldn't find property {propertyName} in type
        ///     {sourceType.FullName}
        /// </exception>
        public static void SetPropertyValue(this object source, string propertyName, object val)
        {
            source.ThrowIfNull(nameof(source));

            var sourceType = source.GetType();
            var propInfo = GetPropertyInfo(sourceType, propertyName);
            if (propInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName)
                    , $"Couldn't find property {propertyName} in type {sourceType.FullName}");
            }
               
            propInfo.SetValue(source, val, null);
        }

        /// <summary>
        ///     Calls a void method
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <param name="source">The object.</param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     propertyName - Couldn't find property {propertyName} in type
        ///     {sourceType.FullName}
        /// </exception>
        public static void CallMethod(this object source, string methodName, params object[] args)
        {
            source.ThrowIfNull(nameof(source));

            Type sourceType = source.GetType();
            MethodInfo methodInfo = sourceType.GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(methodName)
                    , $"Couldn't find method [{methodName}] in type {sourceType.FullName}");
            }
            try
            {
                methodInfo.Invoke(source, args);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw;
            }
            catch (TargetParameterCountException)
            {
                throw new TargetParameterCountException($"Method [{methodName}] in type {sourceType.FullName} does not have an overload which takes [{args.Length}] parameters: [{String.Join(",", args.Select(x => x.GetType().Name))}]");
            }
        }

        /// <summary>
        ///     Calls a method which returns a T. For void methods use non-generic overload
        ///     <para />
        ///     This should be used in tests and hacks rather than 'proper' code
        /// </summary>
        /// <param name="source">The object.</param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     propertyName - Couldn't find property {propertyName} in type
        ///     {sourceType.FullName}
        /// </exception>
        public static T CallMethod<T>(this object source, string methodName, params object[] args)
        {
            source.ThrowIfNull(nameof(source));

            Type sourceType = source.GetType();
            MethodInfo methodInfo = sourceType.GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(methodName)
                    , $"Couldn't find method {methodName} in type {sourceType.FullName}");
            }
            try
            {
                return (T)methodInfo.Invoke(source, args);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw;
            }
            catch (TargetParameterCountException)
            {
                throw new TargetParameterCountException($"Method [{methodName}] in type {sourceType.FullName} does not have an overload which takes [{args.Length}] parameters: [{String.Join(",", args.Select(x => x.GetType().Name))}]");
            }
        }

        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName
                    , BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fieldInfo == null && type != null);

            return fieldInfo;
        }

        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propInfo;
            do
            {
                propInfo = type.GetProperty(propertyName
                    , BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (propInfo == null && type != null);

            return propInfo;
        }
    }
}