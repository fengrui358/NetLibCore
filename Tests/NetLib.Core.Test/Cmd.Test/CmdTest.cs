using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FrHello.NetLib.Core.Framework.Cmd;
using NetLib.Core.Test.ConstString;
using Xunit;

namespace NetLib.Core.Test.Cmd.Test
{
    /// <summary>
    /// CmdTest
    /// </summary>
    public class CmdTest
    {
        /// <summary>
        /// PingTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void PingTest()
        {
            CmdHelper.Excute($"ping {IPAddress.Parse("192.168.153.134")}").GetAwaiter().GetResult();
        }

        /// <summary>
        /// ChangeBaseDirTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void ChangeBaseDirTest()
        {
            CmdHelper.Excute($"git.exe commit", @"G:\Users\Administrator\Desktop\Test").GetAwaiter().GetResult();
        }
    }
}