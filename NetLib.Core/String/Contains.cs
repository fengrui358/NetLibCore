using System;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="source">string source</param>
        /// <param name="value">search value</param>
        /// <param name="comparisonType">comparison type</param>
        /// <returns></returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}
