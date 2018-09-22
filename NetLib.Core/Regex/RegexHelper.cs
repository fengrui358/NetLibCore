using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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
        private static readonly Lazy<System.Text.RegularExpressions.Regex> EmailRegex =
            new Lazy<System.Text.RegularExpressions.Regex>(
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
        /// 检查是否为Ipv4地址格式
        /// </summary>
        /// <param name="address">Ip地址</param>
        /// <returns></returns>
        public static bool CheckIpv4(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            var ipSections = address.Split('.');
            if (ipSections.Length != 4)
            {
                return false;
            }

            foreach (var ipSection in ipSections)
            {
                if (!int.TryParse(ipSection, out var ip) || ip < 0 || ip > 255)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否为Ipv6地址格式
        /// </summary>
        /// <param name="address">Ip地址</param>
        /// <returns></returns>
        public static bool CheckIpv6(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            //检查是否有压缩符号
            var compressionSymbol = address.IndexOf("::", StringComparison.Ordinal);
            if (compressionSymbol >= 0)
            {
                var right = address.Substring(compressionSymbol + 2);
                if (right.IndexOf("::", StringComparison.Ordinal) >= 0)
                {
                    //Ipv6内部只能出现一次"::"压缩符号
                    return false;
                }

                var left = address.Substring(0, compressionSymbol);
                var otherIpSections = new List<string>();

                if (!string.IsNullOrWhiteSpace(left))
                {
                    otherIpSections.AddRange(left.Split(':'));
                }

                if (!string.IsNullOrWhiteSpace(right))
                {
                    otherIpSections.AddRange(right.Split(':'));
                }

                //Ipv6分为8段，出去"::"压缩符号后最多还剩下5个":"符号，6段
                if (otherIpSections.Count > 6)
                {
                    return false;
                }

                foreach (var ipSection in otherIpSections)
                {
                    if (!int.TryParse(ipSection, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out var ip) ||
                        ip < 0 || ip > 0xFFFF)
                    {
                        return false;
                    }
                }
            }
            else
            {
                var ipSections = address.Split(':');
                if (ipSections.Length != 8)
                {
                    return false;
                }

                foreach (var ipSection in ipSections)
                {
                    if (!int.TryParse(ipSection, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out var ip) ||
                        ip < 0 || ip > 0xFFFF)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 中文正则表达式
        /// </summary>
        private static readonly Lazy<System.Text.RegularExpressions.Regex> ChineseRegex =
            new Lazy<System.Text.RegularExpressions.Regex>(
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
        /// 检查域名
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool CheckUrl(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }


            var symbolIndex = str.IndexOf("://", StringComparison.Ordinal);
            if (symbolIndex > 0)
            {
                var urlContent = str.Substring(symbolIndex, 3);
                if (urlContent.Length > 0)
                {
                    var protocol = str.Substring(0, symbolIndex);
                    
                    if (protocol.Length <= 0)
                    {
                        return false;
                    }

                    //协议必须为字母
                    foreach (var c in protocol)
                    {
                        if (!char.IsLetter(c))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检查文件名不包含无效字符
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool CheckFileNameNotContainsInvalidChar(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var intersects = fileName.Intersect(Path.GetInvalidFileNameChars());

            return !intersects.Any();
        }

        /// <summary>
        /// 检查文件路径不包含无效字符
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool CheckFilePathNotContainsInvalidChar(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            var intersects = filePath.Intersect(Path.GetInvalidPathChars());

            return !intersects.Any();
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