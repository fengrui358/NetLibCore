﻿using System.IO;
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
        /// CharsToStringTest
        /// </summary>
        [Fact]
        public void CharsToStringTest()
        {
            var chars = new[] {'a', '4'};
            var s = chars.ToStringEasy();
            
            Assert.Equal("a4", s);
        }

        /// <summary>
        /// BytesToStringTest
        /// </summary>
        [Fact]
        public void BytesToStringTest()
        {
            var s = "Hello你好";
            var bytes = Encoding.UTF8.GetBytes(s);
            var resultDefalutEncoding = bytes.ToStringEasy();
            var resultUtf8Encoding = bytes.ToStringEasy(Encoding.UTF8);
            var resultAsciiEncoding = bytes.ToStringEasy(Encoding.ASCII);

            Assert.Equal(s, resultDefalutEncoding);
            Assert.Equal(s, resultUtf8Encoding);
            Assert.NotEqual(s, resultAsciiEncoding);
        }

        /// <summary>
        /// StreamToStringTest
        /// </summary>
        [Fact]
        public void StreamToStringTest()
        {
            var s = "Hello你好";
            var bytes = Encoding.UTF8.GetBytes(s);
            var stream = new MemoryStream(bytes);

            var resultDefalutEncoding = stream.ToStringEasy();
            var resultUtf8Encoding = stream.ToStringEasy(Encoding.UTF8);
            var resultAsciiEncoding = stream.ToStringEasy(Encoding.ASCII);

            Assert.Equal(s, resultDefalutEncoding);
            Assert.Equal(s, resultUtf8Encoding);
            Assert.NotEqual(s, resultAsciiEncoding);
        }

        /// <summary>
        /// StringToBase64StringTest
        /// </summary>
        [Fact]
        public void StringToBase64StringTest()
        {
            var s =
                "你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64你好，Base64";
            var base64 = s.ToBase64String();

            Assert.Equal(
                "5L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY05L2g5aW977yMQmFzZTY0",
                base64);
        }
    }
}