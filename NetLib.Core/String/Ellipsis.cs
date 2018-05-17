// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 省略字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ellipsisLength">字符串长度（超过这个长度则显示省略号）</param>
        /// <returns>处理后的字符串</returns>
        public static string Ellipsis(this string str, int ellipsisLength = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (ellipsisLength <= 0)
            {
                ellipsisLength = GlobalCoreOptions.DefaultStringEllipsisLength;
            }

            return str.Length > ellipsisLength ? $"{str.Substring(0, ellipsisLength)}..." : str;
        }
    }
}
