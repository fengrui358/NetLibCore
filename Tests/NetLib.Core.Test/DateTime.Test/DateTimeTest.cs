using System;
using FrHello.NetLib.Core.Serialization;
using FrHello.NetLib.Core.Serialization.DateTime;
using Xunit;

namespace NetLib.Core.Test.DateTime.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class DateTimeTest
    {
        /// <summary>
        /// ToUnixTimeTest
        /// </summary>
        [Fact]
        public void ToUnixTimeTest()
        {
            var d = new System.DateTime(1975, 1, 3, 5, 3, 2, DateTimeKind.Utc).ToUnixTime();
            Assert.Equal(157957382000L, d);
        }
    }
}
