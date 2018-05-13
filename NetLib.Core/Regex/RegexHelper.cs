using System;
using System.Net;

namespace FrHello.NetLib.Core.Regex
{
    /// <summary>
    /// 正则表达式辅助类
    /// </summary>
    public static class RegexHelper
    {
        #region 数字

        /// <summary>
        /// 检查是否为自然数
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static bool CheckNaturalNumber(string number)
        {
            return !string.IsNullOrEmpty(number) && int.TryParse(number, out var outNumber) && outNumber >= 0;
        }

        /// <summary>
        /// 检查是否为数字（所有正负整数和浮点数）
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static bool CheckNumber(string number)
        {
            return !string.IsNullOrEmpty(number) && double.TryParse(number, out var _);
        }

        /// <summary>
        /// 检查是否为指定位数的数字（如果是浮点数只考虑整数部分）
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="length">位数</param>
        /// <returns></returns>
        public static bool CheckNumber(string number, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Number of digits cannot be less than 0", nameof(length));
            }

            if (string.IsNullOrEmpty(number))
            {
                return false;
            }

            double.TryParse(number, out var outNumber);
            var outInt = (int) Math.Abs(outNumber);
            return outInt.ToString().Length == length;
        }

        /// <summary>
        /// 检查是否为正整数
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static bool CheckPositiveInteger(string number)
        {
            return !string.IsNullOrEmpty(number) && int.TryParse(number, out var outNumber) && outNumber > 0;
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

        /// <summary>
        /// 校验IpV4正则表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> IpV4Regex = new Lazy<System.Text.RegularExpressions.Regex>(
            () => new System.Text.RegularExpressions.Regex(RegexString.Ipv4));

        /// <summary>
        /// 检查是否为Ipv4地址格式
        /// </summary>
        /// <param name="address">Ip地址</param>
        /// <returns></returns>
        public static bool CheckIpv4(string address)
        {
            return !string.IsNullOrEmpty(address) && IpV4Regex.Value.IsMatch(address);
        }

        /// <summary>
        /// 检查地址是否有效
        /// </summary>
        /// <param name="address">网络地址</param>
        /// <returns></returns>
        public static bool CheckHostNameOrAddress(string address)
        {
            var s = IPAddress.TryParse(address, out var ip);

            return !string.IsNullOrEmpty(address) && IPAddress.TryParse(address, out _);
        }

        /// <summary>
        /// 中文正则表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> ChineseRegex = new Lazy<System.Text.RegularExpressions.Regex>(
            () => new System.Text.RegularExpressions.Regex(RegexString.Chinese));

        /// <summary>
        /// 检查字符串是否包含中文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool CheckChinese(string str)
        {
            return !string.IsNullOrEmpty(str) && ChineseRegex.Value.IsMatch(str);
        }

        /// <summary>
        /// 域名正则表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> DomainRegex = new Lazy<System.Text.RegularExpressions.Regex>(
            () => new System.Text.RegularExpressions.Regex(RegexString.Domain));

        /// <summary>
        /// 检查域名
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool CheckDomain(string str)
        {
            return !string.IsNullOrEmpty(str) && DomainRegex.Value.IsMatch(str);
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
