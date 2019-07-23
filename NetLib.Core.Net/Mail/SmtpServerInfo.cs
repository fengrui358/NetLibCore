

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// Smtp的服务端信息
    /// </summary>
    public class SmtpServerInfo
    {
        /// <summary>
        /// 邮件服务地址
        /// </summary>
        public string SmtpHost { get; }

        /// <summary>
        /// 邮件服务端口
        /// </summary>
        public int? Port { get; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string MailUserName { get; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string MailPassword { get; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="smtpHost">邮件服务器地址</param>
        /// <param name="mailUserName">邮件用户名</param>
        /// <param name="mailPassword">邮件密码</param>
        public SmtpServerInfo(string smtpHost, string mailUserName = null, string mailPassword = null)
        {
            SmtpHost = smtpHost;
            MailUserName = mailUserName;
            MailPassword = mailPassword;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="smtpHost">邮件服务器地址</param>
        /// <param name="port">邮件服务器端口</param>
        /// <param name="mailUserName">邮件用户名</param>
        /// <param name="mailPassword">邮件密码</param>
        public SmtpServerInfo(string smtpHost, int port, string mailUserName = null, string mailPassword = null) : this(smtpHost, mailUserName, mailPassword)
        {
            Port = port;
        }
    }
}
