﻿using System.Collections.Generic;
using System.Linq;

namespace FrHello.NetLib.Core.Collections
{
    public static partial class Extensions
    {
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that queries if a null or is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The collection to act on.</param>
        /// <returns>true if a null or is t>, false if not.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this == null || !@this.Any();
        }
    }
}
