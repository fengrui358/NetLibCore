using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FrHello.NetLib.Core.Framework.Cmd;
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
        [Fact(Skip = "手动执行")]
        public void PingTest()
        {
            CmdHelper.Excute($"ping {IPAddress.Parse("192.168.153.134")}").GetAwaiter().GetResult();
        }
    }
}