namespace DotNetLittleHelpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     This list restricts the access to list manipulators (Add, Clear, Insert, Remove, RemoveAt) by marking
    ///     them as Obsolete
    ///     <para />
    ///     Unilike .NET built-in ReadOnlyCollection, this allows being used as list entity in Entity Framework,
    ///     and controlling the list modification via the Domain Model class which uses it.
    ///     <para />
    ///     In order to access the manipulators, unbox the Collection (only in the declaring class)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestrictedAccessList<T> : List<T>
    {
        private const string RestrictionComment = "\r\nRESTRICTED ACCESS.\r\nThis list must not be directly manipulated.\r\n" +
                                                  "Manipulation only allowed through declaring class members\r\n\r\n" +
                                                  "Unboxing this list is only allowed in the declaring class.";

        /// <summary>
        /// Creates new instance
        /// </summary>
        public RestrictedAccessList(IEnumerable<T> list) : base(list)
        {
        }

        /// <summary>
        /// Creates new instance
        /// </summary>
        public RestrictedAccessList()
        {
        }

        /// <summary>
        /// Creates new instance
        /// </summary>
        public RestrictedAccessList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        ///     Performs the unboxing
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator RestrictedAccessList<T>(Collection<T> source)
        {
            RestrictedAccessList<T> target = new RestrictedAccessList<T>();
            foreach (T item in source)
            {
                ((List<T>) target).Add(item);
            }

            return target;
        }

        /// <summary>
        ///     Restricts the access to Add
        /// </summary>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void Add(T item)
        {
        }

        /// <summary>
        ///     Restricts the access to AddRange
        /// </summary>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void AddRange(IEnumerable<T> collection)
        {
        }

        /// <summary>
        ///     Restricts the access to InsertRange
        /// </summary>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
        }

        /// <summary>
        ///     Restricts the access to Clear
        /// </summary>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void Clear()
        {
        }

        /// <summary>
        ///     Restricts the access to Insert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void Insert(int index, T item)
        {
        }

        /// <summary>
        ///     Restricts the access to Remove
        /// </summary>
        /// <param name="item"></param>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void Remove(T item)
        {
        }

        /// <summary>
        ///     Restricts the access to RemoveAt
        /// </summary>
        /// <param name="index"></param>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void RemoveAt(int index)
        {
        }

        /// <summary>
        ///     Restricts the access 
        /// </summary>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void RemoveAll(Predicate<T> match)
        {
        }

        /// <summary>
        ///     Restricts the access 
        /// </summary>
        [Obsolete(RestrictedAccessList<T>.RestrictionComment, true)]
        public new void RemoveRange(int index, int count)
        {
        }
    }
}