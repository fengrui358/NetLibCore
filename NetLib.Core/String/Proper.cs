using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    /// <summary>
    /// 字符串辅助
    /// </summary>
    public static partial class StringHelper
    {
        /// <summary>
        /// 整理字符串，修改为首字母大写，清理单词间的多余空格
        /// </summary>
        /// <param name="str">需要整理的字符串</param>
        /// <returns>整理后的字符串</returns>
        public static string Proper(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            var strList = str.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", strList.Select(s =>
            {
                var chars = s.ToLower().ToCharArray();
                chars[0] = (char) (chars[0] - 32);

                return new string(chars);
            }));
        }
    }
}