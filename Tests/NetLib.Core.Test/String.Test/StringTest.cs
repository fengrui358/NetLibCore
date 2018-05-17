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
    }
}
