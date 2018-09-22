using System.Diagnostics;
using System.Globalization;
using System.Net;
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
        public void CheckIpv4Test()
        {
            Assert.True(RegexHelper.CheckIpv4("0.0.0.0"));
            Assert.True(RegexHelper.CheckIpv4("255.255.255.255"));
            Assert.True(RegexHelper.CheckIpv4("192.168.1.1"));
            Assert.True(RegexHelper.CheckIpv4("0.0.0.1"));
            Assert.True(RegexHelper.CheckIpv4("01.1.1.1"));
            Assert.True(RegexHelper.CheckIpv4("5.025.25.25"));

            Assert.False(RegexHelper.CheckIpv4("2.43.546.2"));
            Assert.False(RegexHelper.CheckIpv4("2..54.2"));
            Assert.False(RegexHelper.CheckIpv4("111.111.111.256"));
        }

        /// <summary>
        /// CheckPositiveIntegerTest
        /// </summary>
        [Fact]
        public void CheckIpv6Test()
        {
            Assert.True(RegexHelper.CheckIpv6("5e:0:0:0:0:0:5668:eeee"));
            Assert.True(RegexHelper.CheckIpv6("5e:0:0:023:0:0:5668:eeee"));
            Assert.True(RegexHelper.CheckIpv6("5e::5668:eeee"));
            Assert.True(RegexHelper.CheckIpv6("::1:8:8888:0:0:8"));
            Assert.True(RegexHelper.CheckIpv6("1::"));
            Assert.True(RegexHelper.CheckIpv6("::1:2:2:2"));
            Assert.True(RegexHelper.CheckIpv6("::"));

            Assert.False(RegexHelper.CheckIpv6("55555:5e:0:0:0:0:0:5668:eeee"));
            Assert.False(RegexHelper.CheckIpv6("5e::5668::eeee"));
            Assert.False(RegexHelper.CheckIpv6("5e::5668::eeee"));
        }

        /// <summary>
        /// CheckChineseTest
        /// </summary>
        [Fact]
        public void CheckChineseTest()
        {
            Assert.True(RegexHelper.CheckChinese("sdsdf34grtg_)_#@$$@#1`@!#$!@$d赢fv dsfERGRETGRT$%65d78598*-+"));

            Assert.False(RegexHelper.CheckChinese("sdsdf34grtg_)_#@$$@#1`@!#$!@$dfv dsfERGRETGRT$%65d78598*-+"));
        }

        /// <summary>
        /// CheckDomainTest
        /// </summary>
        [Fact]
        public void CheckUrlTest()
        {
            Assert.True(RegexHelper.CheckUrl("http://wenku.baidu.com"));
            Assert.True(RegexHelper.CheckUrl("h://32.545"));
        }

        /// <summary>
        /// CheckFileNameTest
        /// </summary>
        [Fact]
        public void CheckFileNameTest()
        {
            Assert.True(RegexHelper.CheckFileNameNotContainsInvalidChar("sa.dfa232"));
            Assert.False(RegexHelper.CheckFileNameNotContainsInvalidChar(".dad\\sa"));
            Assert.False(RegexHelper.CheckFileNameNotContainsInvalidChar("dad?sa"));
        }

        /// <summary>
        /// CheckFilePathTest
        /// </summary>
        [Fact]
        public void CheckFilePathTest()
        {
            Assert.True(RegexHelper.CheckFilePathNotContainsInvalidChar("asfas/"));
        }
    }
}