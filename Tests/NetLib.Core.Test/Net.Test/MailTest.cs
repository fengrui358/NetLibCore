using System.Net.Mail;
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

            MailHelper.SendAsync(new MailMessage
            {
                To = {new MailAddress("")}, CC = {new MailAddress("")},
                Subject = "测试主题", Body = "测试内容"
            }).GetAwaiter().GetResult();
        }
    }
}
