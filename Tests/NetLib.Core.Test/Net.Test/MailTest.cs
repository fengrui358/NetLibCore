using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using FrHello.NetLib.Core.Net;
using NetLib.Core.Test.ConstString;
using Xunit;

namespace NetLib.Core.Test.Net.Test
{
    /// <summary>
    /// MailTest
    /// </summary>
    public class MailTest
    {
        /// <summary>
        /// SendTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void SendTest()
        {
            GlobalMailOptions.SmtpServerInfo = new SmtpServerInfo("smtp.189.cn", "logger@189.cn", "");
            GlobalMailOptions.DefaultTimeOut = 1500;

            var mailMessage =
                MailHelper.CreateMailMessage(new string[]{}, "测试主题");
            MailHelper.SendAsync(mailMessage).GetAwaiter().GetResult();
        }

        /// <summary>
        /// SendAttachmentTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void SendAttachmentTest()
        {
            GlobalMailOptions.SmtpServerInfo = new SmtpServerInfo("smtp.189.cn", "logger@189.cn", "");
            GlobalMailOptions.DefaultTimeOut = 1500;

            var mailMessage = new MailMessage
            {
                To = {new MailAddress("")},
                CC = {new MailAddress("")},
                Subject = "测试主题",
                Body = "测试内容"
            };

            var filePath1 = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"{Guid.NewGuid():N}.txt");
            File.WriteAllText(filePath1, "test attachment 1", Encoding.UTF8);

            var filePath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"{Guid.NewGuid():N}.txt");
            File.WriteAllText(filePath2, "测试 附件 1", Encoding.UTF8);

            mailMessage.Attachments.Add(MailHelper.CreateAttachment(filePath1));
            mailMessage.Attachments.Add(MailHelper.CreateAttachment(filePath2));

            MailHelper.SendAsync(mailMessage).GetAwaiter().GetResult();
        }
    }
}