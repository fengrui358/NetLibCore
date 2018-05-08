using System;
using System.Reflection;
using FrHello.NetLib.Core.Reflection;
using Xunit;

namespace NetLib.Core.Test.Reflection.Test
{
    /// <summary>
    /// 类型打印测试
    /// </summary>
    public class PrinterTest
    {
        /// <summary>
        /// PringTypeTest
        /// </summary>
        [Fact]
        public void PringTypeTest()
        {
            var output = Printer.Output(typeof(Environment));
            Assert.NotNull(output);
        }

        /// <summary>
        /// PringTypeTest
        /// </summary>
        [Fact]
        public void PringObjectTest()
        {
            var obj = new MockClass
            {
                PublicProperty = 34
            };

            var output = Printer.Output(obj, true);
            Assert.NotNull(output);
        }
    }

    internal class MockClass
    {
        private int _intFiled;
        private int? _nullabledIntFiled;

        private static int _intStaticFiled;
        private static int? _nullabledStaticIntFiled;

        private int PrivateProperty { get; set; }
        protected int? ProtectedNullabledProperty { get; set; }

        public int PublicProperty { get; set; }

        private static int PrivateStaticProperty { get; set; }

        protected static int? ProtectedNullabledStaticProperty { get; set; }

        public static int PublicStaticProperty { get; set; }

        internal static InnerMockClass InMockClass { get; set; }

        static MockClass()
        {
            PrivateStaticProperty = 25;
            PublicStaticProperty = 26;
        }

        internal MockClass()
        {
            _intFiled = 45;
            _nullabledIntFiled = 65;
        }

        private MockClass(string s)
        {
            
            
        }

        internal MockClass(int a, int? b)
        {
            _intFiled = a;
            _nullabledIntFiled = b;
        }

        internal class InnerMockClass
        {
#pragma warning disable 169
            private int _intFiled;
#pragma warning restore 169
#pragma warning disable 169
            private int? _nullabledIntFiled;
#pragma warning restore 169

            private int PrivateProperty { get; set; }
            protected int? ProtectedNullabledProperty { get; set; }
            public int PublicProperty { get; set; }

            private static int PrivateStaticProperty { get; set; }

            protected int? ProtectedNullabledStaticProperty { get; set; }

            public int PublicStaticProperty { get; set; }
        }
    }
}
