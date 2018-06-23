namespace DotNetLittleHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Handles comparison of objects
    /// </summary>
    public static class ObjectComparison
    {
        /// <summary>
        /// Throws aggregate exception if public properties values are not equal between the source and target objects<para/>
        /// Inner exceptions collection contains info on each mismatched property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="propertyBindingFlags">Specifies which types of properties should be included</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">List of property names to be included in the comparison (anything not specified is ignored!)</param>
        /// <returns></returns>
        public static void ThrowIfPropertiesNotEqual<T>(this T source, T target, BindingFlags propertyBindingFlags, bool recursiveValidation = false, IEnumerable<string> ignoreProperties = null, IEnumerable<string> includeProperties = null) where T : class
        {
            source.PropertiesAreEqual(target, propertyBindingFlags, recursiveValidation: recursiveValidation, throwIfNotEqual: true, ignoreProperties: ignoreProperties, includeProperties : includeProperties);
        }

        /// <summary>
        /// Throws aggregate exception if public properties values are not equal between the source and target objects<para/>
        /// Inner exceptions collection contains info on each mismatched property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">List of property names to be included in the comparison (anything not specified is ignored!)</param>
        /// <returns></returns>
        public static void ThrowIfPublicPropertiesNotEqual<T>(this T source, T target, bool recursiveValidation = false, IEnumerable<string> ignoreProperties = null, IEnumerable<string> includeProperties = null) where T : class
        {
            source.PropertiesAreEqual(target, BindingFlags.Public | BindingFlags.Instance, recursiveValidation: recursiveValidation, throwIfNotEqual: true);
        }

        /// <summary>
        /// Returns true if public instance property values are equal between the source and target objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">First object to compare</param>
        /// <param name="target">Second object to compare</param>
        /// <param name="recursiveValidation">If true - Nested complex types will be processed recursively</param>
        /// <param name="ignoreProperties">List of property names to be ignored from the comparison</param>
        /// <param name="includeProperties">List of property names to be included in the comparison (anything not specified is ignored!)</param>
        /// <returns></returns>
        public static bool PublicInstancePropertiesAreEqual<T>(this T source, T target,  bool recursiveValidation = false, IEnumerable<string> ignoreProperties = null, IEnumerable<string> includeProperties = null) where T : class
        {
            return source.PropertiesAreEqual(target, BindingFlags.Public | BindingFlags.Instance, 
                recursiveValidation: recursiveValidation, 
                includeProperties: includeProperties, 
                ignoreProperties: ignoreProperties);
        }

        /// <summary>
        /// Returns true if property values are equal between the source and target objects
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
        /// <param name="includeProperties">List of property names to be included in the comparison (anything not specified is ignored!)</param>
        /// <returns></returns>
        public static bool PropertiesAreEqual<T>(this T source, T target, BindingFlags propertyBindingFlags, bool throwIfNotEqual =  false, bool recursiveValidation = false, IEnumerable<string> ignoreProperties = null, IEnumerable<string> includeProperties = null, List<string> validationErrorsList = null, string sourceObjectName = null) where T: class
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
                if (ignoreProperties != null)
                {
                    propertiesList = propertiesList.Where(x => !ignoreProperties.Contains(x.Name)).ToList();
                }
                if (includeProperties != null)
                {
                    propertiesList = propertiesList.Where(x => includeProperties.Contains(x.Name)).ToList();
                }
                foreach (PropertyInfo propertyInfo in propertiesList)
                {
                    object sourceVal = propertyInfo.GetValue(source);
                    object targetVal = propertyInfo.GetValue(target);
                    if (propertyInfo.GetUnderlyingType().IsSimpleType())
                    {
                        ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                    }
                    else
                    {
                        if (!recursiveValidation)
                        {
                            ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                        }
                        else
                        {
                            if (sourceVal == null && targetVal == null)
                            {
                                continue;
                            }

                            if (sourceVal != null)
                            {
                                if (targetVal == null)
                                {
                                    // ReSharper disable once ExpressionIsAlwaysNull - this is for proper error handling (using consistent method for all comparisons)
                                    ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                                }
                                else
                                {
                                    var innerMatched = sourceVal.PropertiesAreEqual(targetVal, propertyBindingFlags, 
                                        throwIfNotEqual: false, 
                                        validationErrorsList: validationErrorsList, 
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
                                // ReSharper disable once ExpressionIsAlwaysNull - this is for proper error handling (using consistent method for all comparisons)

                                ObjectComparison.CompareValues(sourceVal, targetVal, propertyInfo, sourceType, ref allMatched, validationErrorsList, sourceObjectName);
                            }

                        }
                    }

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
    }
}