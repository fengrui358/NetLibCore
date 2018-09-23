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
        /// Email(这个正则表达式校验Email的方式比微软的更加严格)
        /// </summary>
        public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// Ipv4
        /// </summary>
        public const string Ipv4 = @"^((25[0-5]|2[0-4]\d|[0-1]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[0-1]?\d\d?)$";

        /// <summary>
        /// Ipv6
        /// </summary>
        public const string Ipv6 = @"^\s*((([0-9A-Fa-f]{1,4}:){7}(([0-9A-Fa-f]{1,4})|:))|(([0-9A-Fa-f]{1,4}:){6}(:|((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})|(:[0-9A-Fa-f]{1,4})))|(([0-9A-Fa-f]{1,4}:){5}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(([0-9A-Fa-f]{1,4}:){4}(:[0-9A-Fa-f]{1,4}){0,1}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(([0-9A-Fa-f]{1,4}:){3}(:[0-9A-Fa-f]{1,4}){0,2}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(([0-9A-Fa-f]{1,4}:){2}(:[0-9A-Fa-f]{1,4}){0,3}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(([0-9A-Fa-f]{1,4}:)(:[0-9A-Fa-f]{1,4}){0,4}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(:(:[0-9A-Fa-f]{1,4}){0,5}((:((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})?)|((:[0-9A-Fa-f]{1,4}){1,2})))|(((25[0-5]|2[0-4]\d|[01]?\d{1,2})(\.(25[0-5]|2[0-4]\d|[01]?\d{1,2})){3})))(%.+)?\s*$";

        /// <summary>
        /// 汉字
        /// </summary>
        public const string Chinese = "[\u4e00-\u9fa5]+";

        /// <summary>
        /// Url
        /// </summary>
        public const string Url = @"^[a-zA-z]+://[^\s]*$";

        #endregion
    }
}
