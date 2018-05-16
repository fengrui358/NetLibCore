using FrHello.NetLib.Core.Net;
using Xunit;

namespace NetLib.Core.Test.Net.Test
{
    /// <summary>
    /// NetTest
    /// </summary>
    public class NetTest
    {
        /// <summary>
        /// PingTest
        /// </summary>
        [Fact]
        public void PingTest()
        {
            Assert.True(NetHelper.PingAsync("baidu.com").GetAwaiter().GetResult());
            Assert.True(NetHelper.PingAsync("127.0.0.1").GetAwaiter().GetResult());
        }

        /// <summary>
        /// CheckRemotePortTest
        /// </summary>
        [Fact]
        public void CheckRemotePortTest()
        {
            Assert.True(NetHelper.CheckRemotePortAsync("baidu.com", 80).GetAwaiter().GetResult());
        }

        /// <summary>
        /// LocalPortInUse
        /// </summary>
        [Fact(Skip = "需要手动执行")]
        public void LocalPortInUse()
        {
            Assert.True(NetHelper.CheckLocalPort(80));
        }
    }
}
