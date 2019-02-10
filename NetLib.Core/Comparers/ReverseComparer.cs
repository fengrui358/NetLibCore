using System.Collections.Generic;

namespace FrHello.NetLib.Core.Comparers
{
    /// <summary>
    /// 用于进行反向比较
    /// </summary>
    /// <example>
    /// Array.Sort(list, new ReverseComparer());
    /// list.Sort(new ReverseComparer());
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class ReverseComparer<T> : IComparer<T>
    {
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>比较结果</returns>
        public int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(y, x);
        }
    }
}
