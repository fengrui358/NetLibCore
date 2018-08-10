using System;
// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 比较字符串绝对相等，均不为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="other">待比较的字符串</param>
        /// <param name="stringComparison">比较方式</param>
        /// <returns>处理后的字符串</returns>
        public static bool EqualAbsolute(this string str, string other, StringComparison stringComparison = StringComparison.Ordinal)
        {
            return str != null && other != null && str.Equals(other, stringComparison);
        }
    }
}
