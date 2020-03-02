using System.Linq;
using System.Net.NetworkInformation;
using FrHello.NetLib.Core.Net;
using NetLib.Core.Test.ConstString;
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
        [Fact()]
        public void PingTest()
        {
            Assert.True(NetHelper.PingAsync("http://baidu.com").GetAwaiter().GetResult());
            Assert.True(NetHelper.PingAsync("127.0.0.1").GetAwaiter().GetResult());
        }

        /// <summary>
        /// PingEasyTest
        /// </summary>
        [Fact]
        public void PingEasyTest()
        {
            Assert.True(NetHelper.PingEasy("http://baidu.com/index.html").GetAwaiter().GetResult());
            Assert.True(NetHelper.PingEasy("http://baidu.com:80/index.html").GetAwaiter().GetResult());
            Assert.True(NetHelper.PingEasy("127.0.0.1").GetAwaiter().GetResult());
        }

        /// <summary>
        /// CheckRemotePortTest
        /// </summary>
        [Fact()]
        public void CheckRemotePortTest()
        {
            Assert.True(NetHelper.CheckRemotePortAsync("http://baidu.com", 80).GetAwaiter().GetResult());
        }

        /// <summary>
        /// CheckRemotePortEasyTest
        /// </summary>
        [Fact()]
        public void CheckRemotePortEasyTest()
        {
            Assert.True(NetHelper.CheckRemotePortEasy("http://baidu.com/index.html").GetAwaiter().GetResult());
            Assert.True(NetHelper.CheckRemotePortEasy("http://baidu.com:443/index.html").GetAwaiter().GetResult());
            Assert.False(NetHelper.CheckRemotePortEasy("http://baidu.com:88/index.html").GetAwaiter().GetResult());
        }

        /// <summary>
        /// LocalPortInUse
        /// </summary>
        [Fact()]
        public void LocalPortInUse()
        {
            Assert.True(NetHelper.CheckLocalPort(80));
        }

        /// <summary>
        /// GetLocalIpAddressTest
        /// </summary>
        [Fact()]
        public void GetLocalIpAddressTest()
        {
            var ethernetaAddresses = NetHelper.GetAllLocalIPv4(NetworkInterfaceType.Ethernet);
            var wirelessAddresses = NetHelper.GetAllLocalIPv4(NetworkInterfaceType.Wireless80211);

            Assert.True(ethernetaAddresses.Any());
            Assert.True(wirelessAddresses.Any());
        }
    }
}
