using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 邮件辅助类
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="toAddress">发送的地址，如果有多个地址用,分隔</param>
        public static void Send(string subject, string body, string toAddress)
        {
            if (string.IsNullOrEmpty(toAddress))
            {
                throw new ArgumentNullException(nameof(toAddress));
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding),
                Subject = subject,
                Body = body,
                SubjectEncoding = GlobalMailOptions.DefaultEncoding,
                BodyEncoding = GlobalMailOptions.DefaultEncoding
            };

            foreach (var address in toAddress.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(new MailAddress(address));
            }

            Send(mailMessage);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件内容</param>
        public static void Send(MailMessage mailMessage)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException(nameof(mailMessage));
            }

            //检查有无发件人信息
            if (mailMessage.From != null)
            {
                if (mailMessage.From.Address != GlobalMailOptions.SmtpServerInfo.MailUserName)
                {
                    throw new ArgumentException("Can't support overwrite default sender");
                }
            }
            else
            {
                mailMessage.From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding);
            }

            //继续检查
            CheckSmtpServer();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = GlobalMailOptions.SmtpServerInfo.SmtpHost;
                if (GlobalMailOptions.SmtpServerInfo.Port != null)
                {
                    smtpClient.Port = GlobalMailOptions.SmtpServerInfo.Port.Value;
                }

                smtpClient.EnableSsl = GlobalMailOptions.EnableSsl;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.SmtpServerInfo.MailPassword);

                smtpClient.Send(mailMessage);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="toAddress">发送的地址，如果有多个地址用,分隔</param>
        public static async Task SendAsync(string subject, string body, string toAddress)
        {
            if (string.IsNullOrEmpty(toAddress))
            {
                throw new ArgumentNullException(nameof(toAddress));
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding),
                Subject = subject,
                Body = body,
                SubjectEncoding = GlobalMailOptions.DefaultEncoding,
                BodyEncoding = GlobalMailOptions.DefaultEncoding
            };

            foreach (var address in toAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(new MailAddress(address));
            }

            await SendAsync(mailMessage);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件内容</param>
        public static async Task SendAsync(MailMessage mailMessage)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException(nameof(mailMessage));
            }

            //检查有无发件人信息
            if (mailMessage.From != null)
            {
                if (mailMessage.From.Address != GlobalMailOptions.SmtpServerInfo.MailUserName)
                {
                    throw new ArgumentException("Can't support overwrite default sender");
                }
            }
            else
            {
                mailMessage.From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding);
            }

            //继续检查
            CheckSmtpServer();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = GlobalMailOptions.SmtpServerInfo.SmtpHost;
                if (GlobalMailOptions.SmtpServerInfo.Port != null)
                {
                    smtpClient.Port = GlobalMailOptions.SmtpServerInfo.Port.Value;
                }

                smtpClient.EnableSsl = GlobalMailOptions.EnableSsl;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.SmtpServerInfo.MailPassword);

                var sendMailAsync = smtpClient.SendMailAsync(mailMessage);
                var delayTaskCancel = new CancellationTokenSource();
                var delayTask = Task.Delay(GlobalMailOptions.DefaultTimeOut, delayTaskCancel.Token);

                if (await Task.WhenAny(sendMailAsync, delayTask) == delayTask)
                {
                    //任务超时，取消发送
                    smtpClient.SendAsyncCancel();
                }
                else
                {
                    delayTaskCancel.Cancel();
                }
            }
        }

        /// <summary>
        /// 检查Smtp服务设置
        /// </summary>
        private static void CheckSmtpServer()
        {
            if (GlobalMailOptions.SmtpServerInfo == null)
            {
                throw new ArgumentNullException(nameof(GlobalMailOptions.SmtpServerInfo));
            }

            if (string.IsNullOrEmpty(GlobalMailOptions.SmtpServerInfo.SmtpHost))
            {
                throw new ArgumentNullException(nameof(GlobalMailOptions.SmtpServerInfo.SmtpHost));
            }

            if (string.IsNullOrEmpty(GlobalMailOptions.SmtpServerInfo.MailUserName))
            {
                throw new ArgumentNullException(nameof(GlobalMailOptions.SmtpServerInfo.MailUserName));
            }

            if (string.IsNullOrEmpty(GlobalMailOptions.SmtpServerInfo.MailPassword))
            {
                throw new ArgumentNullException(nameof(GlobalMailOptions.SmtpServerInfo.MailPassword));
            }
        }
    }
}
