using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FrHello.NetLib.Core.Collections
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 判断一个集合是否为空引用或零数量
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="this">使用扩展方法的对象</param>
        /// <returns>true 集合为空或数量为零，false 集合数量不为零</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this == null || !@this.Any();
        }

        /// <summary>
        /// 判断一个集合是否为空引用或零数量
        /// </summary>
        /// <param name="this">使用扩展方法的对象</param>
        /// <returns>true 集合为空或数量为零，false 集合数量不为零</returns>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            if (@this == null)
            {
                return true;
            }

            return !@this.GetEnumerator().MoveNext();
        }
    }
}
