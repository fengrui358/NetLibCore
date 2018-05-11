using System;

namespace FrHello.NetLib.Core.Regex
{
    /// <summary>
    /// 正则表达式辅助类
    /// </summary>
    public static class RegexHelper
    {
        #region 数字

        /// <summary>
        /// 校验自然数表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> NaturalNumberRegex = new Lazy<System.Text.RegularExpressions.Regex>(
            () => new System.Text.RegularExpressions.Regex(RegexString.NaturalNumber));

        /// <summary>
        /// 检查是否为自然数
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static bool CheckNaturalNumber(string number)
        {
            return !string.IsNullOrEmpty(number) && NaturalNumberRegex.Value.IsMatch(number);
        }

        #endregion

        #region 特殊需求

        /// <summary>
        /// 校验邮件正则表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> EmailRegex = new Lazy<System.Text.RegularExpressions.Regex>(
            () => new System.Text.RegularExpressions.Regex(RegexString.Email));

        /// <summary>
        /// 检查邮件地址是否正确
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <returns></returns>
        public static bool CheckEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && EmailRegex.Value.IsMatch(email);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 去掉字符串开始的^和末尾的$
        /// </summary>
        /// <param name="str">待去除的字符串</param>
        /// <returns></returns>
        public static string EscapeStarAndEndSymbol(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.StartsWith("^"))
            {
                str = str.Substring(1, str.Length - 1);
            }

            if (str.EndsWith("$"))
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }

        #endregion
    }
}
