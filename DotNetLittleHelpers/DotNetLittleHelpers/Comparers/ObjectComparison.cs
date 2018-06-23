// -----------------------------------------------------------------------
//  <copyright file="ObjectComparison.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace DotNetLittleHelpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    /// <summary>
    ///     Handles comparison of objects
    /// </summary>
    public static class ObjectComparison
    {
        /// <summary>
        ///     Returns true if property values are equal between the source and target objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="propertyBindingFlags">Specifies which types of properties should be included</param>
        /// <param name="throwIfNotEqual">If true - Throw Aggregate exception containing each incorrect property</param>
        /// <param name="validationErrorsList">List of validation errors</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="sourceObjectName">The name of the source object (for error logging purpose)</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="ignoreComplexTypes">If true - Complex types comparison will be skipped</param>
        /// <param name="includeProperties">
        ///     List of property names to be included in the comparison (anything not specified is
        ///     ignored!)
        /// </param>
        /// <returns></returns>
        public static bool PropertiesAreEqual<T>(this T source, T target, BindingFlags propertyBindingFlags, bool throwIfNotEqual = false, bool recursiveValidation = false,
                                                 IEnumerable<string> ignoreProperties = null, bool ignoreComplexTypes = false, IEnumerable<string> includeProperties = null, List<string> validationErrorsList = null,
                                                 string sourceObjectName = null) where T : class
        {
            source.CheckArgumentNull("source");
            if (validationErrorsList == null)
            {
                validationErrorsList = new List<string>();
            }

            bool allMatched = true;

            if (target == null)
            {
                allMatched = false;
                validationErrorsList.Add("Target object is NULL");
            }
            else
            {
                Type sourceType = source.GetType();
                List<PropertyInfo> propertiesList = sourceType.GetProperties(propertyBindingFlags).ToList();
                propertiesList = ObjectComparison.FilterPropertiesList(ignoreProperties, includeProperties, propertiesList);

                foreach (PropertyInfo propertyInfo in propertiesList)
                {
                    ObjectComparison.CompareProperties(source, target, propertyBindingFlags, recursiveValidation, ignoreComplexTypes, validationErrorsList, sourceObjectName, propertyInfo, sourceType,
                        ref allMatched);
                }
            }

            if (throwIfNotEqual && !allMatched)
            {
                List<Exception> exList = new List<Exception>();
                foreach (string error in validationErrorsList)
                {
                    exList.Add(new Exception(error));
                }

                throw new AggregateException(exList);
            }

            return allMatched;
        }

        /// <summary>
        ///     Returns true if public instance property values are equal between the source and target objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreComplexTypes">If true - Complex types comparison will be skipped</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">
        ///     List of property names to be included in the comparison (anything not specified is
        ///     ignored!)
        /// </param>
        /// <returns></returns>
        public static bool PublicInstancePropertiesAreEqual<T>(this T source, T target, bool recursiveValidation = false, bool ignoreComplexTypes = false, IEnumerable<string> ignoreProperties = null,
                                                               IEnumerable<string> includeProperties = null) where T : class
        {
            return source.PropertiesAreEqual(target, BindingFlags.Public | BindingFlags.Instance,
                recursiveValidation: recursiveValidation,
                ignoreComplexTypes: ignoreComplexTypes,
                includeProperties: includeProperties,
                ignoreProperties: ignoreProperties);
        }

        /// <summary>
        ///     Throws aggregate exception if public properties values are not equal between the source and target objects
        ///     <para />
        ///     Inner exceptions collection contains info on each mismatched property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="propertyBindingFlags">Specifies which types of properties should be included</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreComplexTypes">If true - Complex types comparison will be skipped</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">
        ///     List of property names to be included in the comparison (anything not specified is
        ///     ignored!)
        /// </param>
        /// <returns></returns>
        public static void ThrowIfPropertiesNotEqual<T>(this T source, T target, BindingFlags propertyBindingFlags, bool recursiveValidation = false, bool ignoreComplexTypes = false,
                                                        IEnumerable<string> ignoreProperties = null, IEnumerable<string> includeProperties = null) where T : class
        {
            source.PropertiesAreEqual(target, propertyBindingFlags, recursiveValidation: recursiveValidation, ignoreComplexTypes: ignoreComplexTypes, throwIfNotEqual: true, ignoreProperties: ignoreProperties,
                includeProperties: includeProperties);
        }

        /// <summary>
        ///     Throws aggregate exception if public properties values are not equal between the source and target objects
        ///     <para />
        ///     Inner exceptions collection contains info on each mismatched property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreComplexTypes">If true - Complex types comparison will be skipped</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">
        ///     List of property names to be included in the comparison (anything not specified is
        ///     ignored!)
        /// </param>
        /// <returns></returns>
        public static void ThrowIfPublicPropertiesNotEqual<T>(this T source, T target, bool recursiveValidation = false, bool ignoreComplexTypes = false, IEnumerable<string> ignoreProperties = null,
                                                              IEnumerable<string> includeProperties = null) where T : class
        {
            source.PropertiesAreEqual(target, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: recursiveValidation, ignoreComplexTypes: ignoreComplexTypes, throwIfNotEqual: true);
        }

        private static void CompareProperties<T>(T source, T target, BindingFlags propertyBindingFlags, bool recursiveValidation, bool ignoreComplexTypes, List<string> validationErrorsList, string sourceObjectName,
                                                 PropertyInfo propertyInfo, Type sourceType, ref bool allMatched) where T : class
        {
            if (propertyInfo.IsNonStringEnumerable())
            {
                ObjectComparison.HandleListComparison(source, target, propertyBindingFlags, validationErrorsList, ignoreComplexTypes,propertyInfo, sourceType, ref allMatched);
            }
            else
            {
                object sourceVal = propertyInfo.GetValue(source, null);
                object targetVal = propertyInfo.GetValue(target, null);

                if (propertyInfo.GetUnderlyingType().IsSimpleType())
                {
                    ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                }
                else
                {
                    if (ignoreComplexTypes)
                    {
                        return;
                    }
                    if (!recursiveValidation)
                    {
                        ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                    }
                    else
                    {
                        ObjectComparison.HandleRecursiveComparison(propertyBindingFlags, validationErrorsList, false, sourceObjectName, propertyInfo, sourceType, ref allMatched, sourceVal,
                            targetVal);
                    }
                }
            }
        }

        private static void CompareValues(object sourceVal, object targetVal, PropertyInfo propertyInfo, Type type, ref bool allMatched, List<string> validationErrorsList,
                                          string sourceObjectName)
        {
            string sourceObjectInfo = "";
            if (!string.IsNullOrEmpty(sourceObjectName))
            {
                sourceObjectInfo = $"Object: [{sourceObjectName}]. ";
            }

            if (sourceVal != targetVal && (sourceVal == null || !sourceVal.Equals(targetVal)))
            {
                allMatched = false;
                validationErrorsList.Add($"{sourceObjectInfo}Type: [{type.Name}]. Property: [{propertyInfo.Name}]. Source: [{sourceVal ?? "NULL"}]. Target: [{targetVal ?? "NULL"}]");
            }
        }

        private static List<PropertyInfo> FilterPropertiesList(IEnumerable<string> ignoreProperties, IEnumerable<string> includeProperties, List<PropertyInfo> propertiesList)
        {
            if (ignoreProperties != null)
            {
                propertiesList = propertiesList.Where(x => !ignoreProperties.Contains(x.Name)).ToList();
            }

            if (includeProperties != null)
            {
                propertiesList = propertiesList.Where(x => includeProperties.Contains(x.Name)).ToList();
            }

            return propertiesList;
        }

        private static void HandleListComparison<T>(T source, T target, BindingFlags propertyBindingFlags, List<string> validationErrorsList, bool ignoreComplexTypes, PropertyInfo propertyInfo, Type sourceType,
                                                    ref bool allMatched)
            where T : class
        {
            IEnumerable<object> sourceVal = propertyInfo.GetValue(source, null) as IEnumerable<object>;
            IEnumerable<object> targetVal = propertyInfo.GetValue(target, null) as IEnumerable<object>;
            if (sourceVal != null || targetVal != null)
            {
                List<object> sourceList = new List<object>(sourceVal ?? new List<object>());
                List<object> targetList = new List<object>(targetVal ?? new List<object>());
                if (sourceList.Count == targetList.Count)
                {
                    for (int i = 0; i < sourceList.Count; i++)
                    {
                        object obj1 = sourceList[i];
                        if (ignoreComplexTypes && !obj1.GetType().IsSimpleType())
                        {
                            break;
                        }

                        object obj2 = targetList[i];
                        bool innerMatched = obj1.PropertiesAreEqual(obj2, propertyBindingFlags,
                            throwIfNotEqual: false,
                            validationErrorsList: validationErrorsList,
                            ignoreComplexTypes: ignoreComplexTypes,
                            recursiveValidation: true,
                            sourceObjectName: propertyInfo.Name);
                        if (!innerMatched)
                        {
                            allMatched = false;
                        }
                    }
                }
                else
                {
                    allMatched = false;
                    validationErrorsList.Add(
                        $"Type: [{sourceType.Name}]. Property: [{propertyInfo.Name}]. SourceListCount: [{(sourceVal != null ? sourceList.Count.ToString() : "NULL")}]. TargetListCount: [{(targetVal != null ? targetList.Count.ToString() : "NULL")}]");
                }
            }
        }

        private static void HandleRecursiveComparison(BindingFlags propertyBindingFlags, List<string> validationErrorsList, bool ignoreComplexTypes, string sourceObjectName, PropertyInfo propertyInfo,
                                                      Type sourceType,
                                                      ref bool allMatched, object sourceVal, object targetVal)
        {
            if (sourceVal != null || targetVal != null)
            {
                if (sourceVal != null)
                {
                    if (targetVal == null)
                    {
                        // ReSharper disable once ExpressionIsAlwaysNull - this is for proper error handling (using consistent method for all comparisons)
                        ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                    }
                    else
                    {
                        bool innerMatched = sourceVal.PropertiesAreEqual(targetVal, propertyBindingFlags,
                            throwIfNotEqual: false,
                            validationErrorsList: validationErrorsList,
                            ignoreComplexTypes: ignoreComplexTypes,
                            recursiveValidation: true,
                            sourceObjectName: propertyInfo.Name);
                        if (!innerMatched)
                        {
                            allMatched = false;
                        }
                    }
                }
            }

            else
            {
                // ReSharper disable once ExpressionIsAlwaysNull - this is for proper error handling (using consistent method for all comparisons)
                ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
            }
        }
    }
}