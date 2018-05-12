using System;

namespace FrHello.NetLib.Core.Regex
{
    /// <summary>
    /// 常用正则表达式字符串
    /// </summary>
    public static class RegexString
    {
        #region 数字

        /// <summary>
        /// 自然数
        /// </summary>
        public static string NaturalNumber = "^[0-9]*$";

        /// <summary>
        /// 正整数
        /// </summary>
        public static string PositiveInteger = @"^[1-9]\d*$";

        private const string ConstNumber = @"^(\-|\+)?\d+(\.\d+)?$";
        /// <summary>
        /// 数字
        /// </summary>
        /// <returns></returns>
        public static string Number()
        {
            return ConstNumber;
        }

        #endregion

        #region 特殊需求

        /// <summary>
        /// Email
        /// </summary>
        public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// IpV4
        /// </summary>
        public const string IpV4 = "^((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))$";

        /// <summary>
        /// 汉字
        /// </summary>
        public const string Chinese = "[\u4e00-\u9fa5]+";

        #endregion
    }
}
