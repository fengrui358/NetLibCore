using FrHello.NetLib.Core.Helpers;
using NetLib.Core.Test.ConstString;
using Xunit;

namespace NetLib.Core.Test.Path.Test
{
    /// <summary>
    /// PathHelperTest
    /// </summary>
    public class PathHelperTest
    {
        /// <summary>
        /// PathHelperTest
        /// </summary>
        [Fact]
        public void UrlCombineTest()
        {
            var actual1 = "10.45.32.56:4353/dfsfsa/gfgd";
            var excepted1 = PathHelper.UrlCombine(string.Empty, "/10.45.32.56:4353", "/dfSfsa/gfGd");
            Assert.Equal(excepted1, actual1);

            var actual2 = "https://cd.bendibao.com/live/201784/91973.shtm";
            var excepted2 = PathHelper.UrlCombine("https", "cd.bendibao.com\\live/", "/201784/91973.shtm");
            Assert.Equal(excepted2, actual2);
        }

        /// <summary>
        /// GetDesktopFileName
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void GetDesktopFileName()
        {
            var excepted1 = PathHelper.GetDesktopFileName("Test.ppt");

            var excepted2 = PathHelper.GetDesktopFileName("Test.ppt", true);
        }
    }
}
