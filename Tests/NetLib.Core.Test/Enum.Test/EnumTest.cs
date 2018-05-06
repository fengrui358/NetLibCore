using FrHello.NetLib.Core.Reflection.Enum;
using Xunit;

namespace NetLib.Core.Test.Enum.Test
{
    /// <summary>
    /// EnumTest
    /// </summary>
    public class EnumTest
    {
        /// <summary>
        /// EnumDescriptionTest
        /// </summary>
        [Fact]
        public void EnumDescriptionTest()
        {
            var ma = new MockClass{EnumTest = TestEnum.A};
            var mb = new MockClass{EnumTest = TestEnum.B};

            var a = ma.EnumTest.GetDescription();
            var b = mb.EnumTest.GetDescription();

            Assert.Equal("ADescription", a);
            Assert.Equal(mb.EnumTest.ToString(), b);
        }

        /// <summary>
        /// MockClass
        /// </summary>
        public class MockClass
        {
            /// <summary>
            /// EnumTest
            /// </summary>
            public TestEnum EnumTest { get; set; }
        }

        /// <summary>
        /// TestEnum
        /// </summary>
        public enum TestEnum
        {
            /// <summary>
            /// A
            /// </summary>
            [EnumDescription("ADescription")]
            A,

            /// <summary>
            /// B
            /// </summary>
            B
        }
    }
}
