using System;
using System.Collections.Generic;
using System.Text;
using FrHello.NetLib.Core.Serialization;
using Xunit;

namespace NetLib.Core.Test.Serialization.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class SerializationTest
    {
        /// <summary>
        /// ByteToStringTest
        /// </summary>
        [Fact]
        public void ByteToStringTest()
        {
            var chars = new[] {'a', '4'};
            var s = chars.ToStringEasy();
            
            Assert.Equal("a4", s);
        }
    }
}
