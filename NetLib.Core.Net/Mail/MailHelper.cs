using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using FrHello.NetLib.Core.Regex;

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

            CheckSmtpServer();

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding),
                Subject = subject,
                Body = body,
                SubjectEncoding = GlobalMailOptions.DefaultEncoding,
                BodyEncoding = GlobalMailOptions.DefaultEncoding
            };

            foreach (var address in toAddress.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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

            CheckSmtpServer();

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

            using var smtpClient = new SmtpClient
            {
                Host = GlobalMailOptions.SmtpServerInfo.SmtpHost, EnableSsl = GlobalMailOptions.EnableSsl
            };

            if (GlobalMailOptions.SmtpServerInfo.Port != null)
            {
                smtpClient.Port = GlobalMailOptions.SmtpServerInfo.Port.Value;
            }
            else if (smtpClient.EnableSsl)
            {
                smtpClient.Port = 587;
            }

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(GlobalMailOptions.SmtpServerInfo.MailUserName,
                GlobalMailOptions.SmtpServerInfo.MailPassword);

            smtpClient.Send(mailMessage);
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

            CheckSmtpServer();

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(GlobalMailOptions.SmtpServerInfo.MailUserName,
                    GlobalMailOptions.DefaultSenderDisplayerName, GlobalMailOptions.DefaultEncoding),
                Subject = subject,
                Body = body,
                SubjectEncoding = GlobalMailOptions.DefaultEncoding,
                BodyEncoding = GlobalMailOptions.DefaultEncoding
            };

            foreach (var address in toAddress.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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

            CheckSmtpServer();

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

            using var smtpClient = new SmtpClient
            {
                Host = GlobalMailOptions.SmtpServerInfo.SmtpHost,
                EnableSsl = GlobalMailOptions.EnableSsl
            };

            if (GlobalMailOptions.SmtpServerInfo.Port != null)
            {
                smtpClient.Port = GlobalMailOptions.SmtpServerInfo.Port.Value;
            }
            else if (smtpClient.EnableSsl)
            {
                smtpClient.Port = 587;
            }

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

                //超时异常
                throw new OperationCanceledException("Send mail timeout");
            }
            else
            {
                delayTaskCancel.Cancel();
            }
        }

        /// <summary>
        /// Create mail message
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static MailMessage CreateMailMessage(string[] to, string subject, string body = null)
        {
            return CreateMailMessage(to, null, subject, body);
        }

        /// <summary>
        /// Create mail message
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static MailMessage CreateMailMessage(string[] to, string[] cc, string subject, string body = null)
        {
            if (to == null || !to.Any())
            {
                throw new ArgumentNullException(nameof(to));
            }

            var mailMessage = new MailMessage
            {
                Subject = subject,
                Body = body
            };

            foreach (var address in to)
            {
                if (string.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException($"address in `to` is null");
                }

                if (RegexHelper.CheckEmail(address))
                {
                    mailMessage.To.Add(new MailAddress(address));
                }
                else
                {
                    throw new ArgumentException($"address[{address}] in `to` is error");
                }
            }

            if (cc != null)
            {
                foreach (var address in cc)
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        throw new ArgumentNullException($"address in `to` is null");
                    }

                    if (RegexHelper.CheckEmail(address))
                    {
                        mailMessage.CC.Add(new MailAddress(address));
                    }
                    else
                    {
                        throw new ArgumentException($"address[{address}] in `to` is error");
                    }
                }
            }

            return mailMessage;
        }

        /// <summary>
        /// Create mail attachment from file info
        /// </summary>
        /// <param name="filePath">file location path</param>
        /// <returns></returns>
        public static Attachment CreateAttachment(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);

                attachment.ContentDisposition.CreationDate = fileInfo.CreationTime;
                attachment.ContentDisposition.ModificationDate = fileInfo.LastWriteTime;
                attachment.ContentDisposition.ReadDate = fileInfo.LastAccessTime;

                return attachment;
            }

            return null;
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