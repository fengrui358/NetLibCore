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
            //todo
        }

        /// <summary>
        /// PingPortTest
        /// </summary>
        [Fact]
        public void PingPortTest()
        {
            //todo
        }

        /// <summary>
        /// LocalPortInUse
        /// </summary>
        [Fact]
        public void LocalPortInUse()
        {
            Assert.True(NetHelper.CheckPortInUse(80));
        }
    }
}
