using FrHello.NetLib.Core;
using Xunit;

namespace NetLib.Core.Test.String.Test
{
    /// <summary>
    /// StringTest
    /// </summary>
    public class StringTest
    {
        /// <summary>
        /// StringEllipsisTest
        /// </summary>
        [Fact]
        public void StringEllipsisTest()
        {
            var defaultSetting = GlobalCoreOptions.DefaultStringEllipsisLength;
            GlobalCoreOptions.DefaultStringEllipsisLength = 3;

            var str = "123";
            Assert.Equal("123", str.Ellipsis());

            str = $"{str}4";
            Assert.Equal("123...", str.Ellipsis());

            GlobalCoreOptions.DefaultStringEllipsisLength = defaultSetting;
        }

        /// <summary>
        /// EqualAbsoluteTest
        /// </summary>
        [Fact]
        public void EqualAbsoluteTest()
        {
            string strA = null;
            Assert.False(strA.EqualAbsolute(null));

            string strB = "abc";
            string strC = "abc";
            Assert.True(strB.EqualAbsolute(strC));
        }
    }
}
