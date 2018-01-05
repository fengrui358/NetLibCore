using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FrHello.NetLib.Core.Collections
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取一个集合的数量
        /// </summary>
        /// <param name="this">使用扩展方法的对象</param>
        /// <returns>集合的数量，如果集合为空引用返回0</returns>
        public static int Count(this IEnumerable @this)
        {
            if (@this == null)
            {
                return 0;
            }

            var index = 0;
            foreach (var unused in @this)
            {
                index++;
            }

            return index;
        }
    }
}
