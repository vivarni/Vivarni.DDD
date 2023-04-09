﻿using System;
using System.Collections.Generic;

namespace Vivarni.DDD.Core
{
    /// <inheritdoc/>
    public abstract class DomainType<T> : DomainType
        where T : DomainType
    {
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        protected DomainType(string name) : base(name) { }

        /// <summary>
        /// Searches all static fields of <typeparamref name="T"/> and returns the one whose name
        /// corresponds with <paramref name="name"/> or <see langword="null"/> otherwise.
        /// </summary>
        public static T? FromString(string? name, bool ignoreCase = false, bool throwException = false)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var stringComparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            var typeproperties = typeof(T).GetFields();

            foreach (var p in typeproperties)
            {
                if (p.GetValue(null) is T item && string.Equals(name, item.Name, stringComparison))
                    return item;
            }

            if (throwException)
                throw new Exception($"Invalid value for domain type {typeof(T).Name}: {name}");

            return null;
        }

        /// <summary>
        /// Returns all object instances of the generic <see cref="DomainType"/>.
        /// </summary>
        public static IEnumerable<T> GetAll()
        {
            var result = new List<T>();
            var typeproperties = typeof(T).GetFields();

            foreach (var p in typeproperties)
            {
                if (p.GetValue(null) is T item)
                    result.Add(item);
            }

            return result;
        }
    }
}
