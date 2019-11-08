using System.IO;
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

        /// <summary>
        /// UnicodeToStringTest
        /// </summary>
        [Fact]
        public void UnicodeToStringTest()
        {
            var s = "freeTest";

            var unicode = @"\u0066\u0072\u0065\u0065\u0054\u0065\u0073\u0074";

            Assert.Equal(unicode, s.ToUnicode());
            Assert.Equal(s, unicode.ToStringFromUnicode());
        }

        /// <summary>
        /// XmlSerializationTest
        /// </summary>
        [Fact]
        public void XmlSerializationTest()
        {
            var c = new MockClass {A = "a"};
            var xml = c.SerializeXml();

            var c2 = xml.DeserializeXml<MockClass>();

            Assert.NotEqual(c, c2);
            Assert.Equal(c.A, c2.A);
        }

        /// <summary>
        /// JsonSerializationTest
        /// </summary>
        [Fact]
        public void JsonSerializationTest()
        {
            var c = new MockClass { A = "a" };
            var json = c.SerializeJson();
            var c2 = json.DeserializeJson<MockClass>();
            Assert.NotEqual(c, c2);
            Assert.Equal(c.A, c2.A);

            json = c.SerializeJsonAsync().GetAwaiter().GetResult();
            c2 = json.DeserializeJsonAsync<MockClass>().GetAwaiter().GetResult();
            Assert.NotEqual(c, c2);
            Assert.Equal(c.A, c2.A);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public class MockClass
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            public string A { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }
    }
}
