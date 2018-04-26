using System;
using System.Collections.Generic;
using System.Text;
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
            var s = new char[] {'a', '4'};
            

            var x = new string(s);
            Assert.Equal("a4", x);
        }
    }
}
