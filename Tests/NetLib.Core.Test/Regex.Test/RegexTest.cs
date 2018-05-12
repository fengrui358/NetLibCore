using System.Globalization;
using FrHello.NetLib.Core.Regex;
using Xunit;

namespace NetLib.Core.Test.Regex.Test
{
    /// <summary>
    /// RegexTest
    /// </summary>
    public class RegexTest
    {
        /// <summary>
        /// RegexEmailTest
        /// </summary>
        [Fact]
        public void RegexCheckEmailTest()
        {
            var isEmail = "asd@163.com";
            var isNotEmail = "asd2433@163";

            Assert.True(RegexHelper.CheckEmail(isEmail));
            Assert.False(RegexHelper.CheckEmail(isNotEmail));
        }

        /// <summary>
        /// RegexCheckNaturalNumberTest
        /// </summary>
        [Fact]
        public void RegexCheckNaturalNumberTest()
        {
            Assert.True(RegexHelper.CheckNaturalNumber(2.ToString()));
            Assert.True(RegexHelper.CheckNaturalNumber(0.ToString()));
            Assert.False(RegexHelper.CheckNaturalNumber((-2).ToString()));
            Assert.False(RegexHelper.CheckNaturalNumber(2.1.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// RegexCheckNumberTest
        /// </summary>
        [Fact]
        public void RegexCheckNumberTest()
        {
            Assert.True(RegexHelper.CheckNumber(2.ToString()));
            Assert.True(RegexHelper.CheckNumber(0.ToString()));
            Assert.True(RegexHelper.CheckNumber((-2.56).ToString(CultureInfo.InvariantCulture)));
            Assert.False(RegexHelper.CheckNumber("2.3.6"));
        }

        /// <summary>
        /// RegexCheckNumberLengthTest
        /// </summary>
        [Fact]
        public void RegexCheckNumberLengthTest()
        {
            Assert.True(RegexHelper.CheckNumber(2.ToString(), 1));
            Assert.True(RegexHelper.CheckNumber(0.ToString(), 1));
            Assert.True(RegexHelper.CheckNumber((-243.56).ToString(CultureInfo.InvariantCulture), 3));
            Assert.False(RegexHelper.CheckNumber(234.ToString(), 2));
        }

        /// <summary>
        /// CheckPositiveIntegerTest
        /// </summary>
        [Fact]
        public void CheckPositiveIntegerTest()
        {
            Assert.True(RegexHelper.CheckPositiveInteger(2.ToString()));
            Assert.False(RegexHelper.CheckPositiveInteger(0.ToString()));
            Assert.False(RegexHelper.CheckPositiveInteger((-2.56).ToString(CultureInfo.InvariantCulture)));
            Assert.False(RegexHelper.CheckPositiveInteger(2.56.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// CheckPositiveIntegerTest
        /// </summary>
        [Fact]
        public void CheckIpV4Test()
        {
            Assert.False(RegexHelper.CheckIpV4("2.43.546.2"));
            Assert.True(RegexHelper.CheckIpV4("2.43.54.2"));
            Assert.False(RegexHelper.CheckIpV4("2..54.2"));
        }

        /// <summary>
        /// CheckChineseTest
        /// </summary>
        [Fact]
        public void CheckChineseTest()
        {
            Assert.False(RegexHelper.CheckChinese("sdsdf34grtg_)_#@$$@#1`@!#$!@$dfv dsfERGRETGRT$%65d78598*-+"));
            Assert.True(RegexHelper.CheckChinese("sdsdf34grtg_)_#@$$@#1`@!#$!@$d赢fv dsfERGRETGRT$%65d78598*-+"));
        }
    }
}
