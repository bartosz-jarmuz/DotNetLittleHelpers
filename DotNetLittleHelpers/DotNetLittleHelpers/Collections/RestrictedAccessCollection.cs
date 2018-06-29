namespace DotNetLittleHelpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     This collection restricts the access to collection manipulators (Add, Clear, Insert, Remove, RemoveAt) by marking
    ///     them as Obsolete
    ///     <para />
    ///     Unilike .NET built-in ReadOnlyCollection, this allows being used as collection entity in Entity Framework,
    ///     and controlling the collection modification via the Domain Model class which uses it.
    ///     <para />
    ///     In order to access the manipulators, unbox the Collection (only in the declaring class)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestrictedAccessCollection<T> : Collection<T>
    {
        private const string RestrictionComment = "\r\nRESTRICTED ACCESS.\r\nThis collection must not be directly manipulated.\r\n" +
                                                  "Manipulation only allowed through declaring class members\r\n\r\n" +
                                                  "Unboxing this collection is only allowed in the declaring class.";

        /// <summary>
        ///     Performs the unboxing
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator RestrictedAccessCollection<T>(List<T> source)
        {
            RestrictedAccessCollection<T> target = new RestrictedAccessCollection<T>();
            foreach (T item in source)
            {
                ((Collection<T>) target).Add(item);
            }

            return target;
        }

        /// <summary>
        ///     Restricts the access to Add
        /// </summary>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessCollection<T>.RestrictionComment, true)]
        public new void Add(T item)
        {
        }

        /// <summary>
        ///     Restricts the access to Clear
        /// </summary>
        [Obsolete(RestrictedAccessCollection<T>.RestrictionComment, true)]
        public new void Clear()
        {
        }

        /// <summary>
        ///     Restricts the access to Insert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessCollection<T>.RestrictionComment, true)]
        public new void Insert(int index, T item)
        {
        }

        /// <summary>
        ///     Restricts the access to Remove
        /// </summary>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessCollection<T>.RestrictionComment, true)]
        public new void Remove(T item)
        {
        }

        /// <summary>
        ///     Restricts the access to RemoveAt
        /// </summary>
        /// <param name="index"></param>
        [Obsolete(RestrictedAccessCollection<T>.RestrictionComment, true)]
        public new void RemoveAt(int index)
        {
        }
    }
}