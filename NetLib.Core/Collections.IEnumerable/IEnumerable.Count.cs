using System.Collections;

namespace FrHello.NetLib.Core
{
    /// <summary>
    /// 扩展方法
    /// </summary>
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
