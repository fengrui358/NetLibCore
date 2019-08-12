using System.Text;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 默认的网络工具相关参数
    /// </summary>
    public static class GlobalMailOptions
    {
        /// <summary>
        /// 默认超时等待时间，毫秒
        /// </summary>
        public static int DefaultTimeOut { get; set; } = 5000;

        /// <summary>
        /// 默认编码格式，用于邮件主题和文本内容的编码
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 默认邮件服务器信息
        /// </summary>
        public static SmtpServerInfo SmtpServerInfo { get; set; }

        /// <summary>
        /// 是否使用安全套接字加密链接
        /// </summary>
        public static bool EnableSsl { get; set; }

        /// <summary>
        /// 默认发送者的名称
        /// </summary>
        internal static string DefaultSenderDisplayerName { get; } = "Auto mail";
    }
}
