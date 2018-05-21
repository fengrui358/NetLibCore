using System;
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
        // ReSharper disable once NotAccessedField.Local
        private int _intFiled;
        // ReSharper disable once NotAccessedField.Local
        private int? _nullabledIntFiled;

#pragma warning disable 169
        private static int _intStaticFiled;
#pragma warning restore 169
#pragma warning disable 169
        private static int? _nullabledStaticIntFiled;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
        private int PrivateProperty { get; set; }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
        protected int? ProtectedNullabledProperty { get; set; }

        public int PublicProperty { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        private static int PrivateStaticProperty { get; set; }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
        protected static int? ProtectedNullabledStaticProperty { get; set; }

        public static int PublicStaticProperty { get; set; }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
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

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedParameter.Local
        private MockClass(string s)
        {
            
            
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once MemberHidesStaticFromOuterClass
        // ReSharper disable once UnusedMember.Global
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

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            private int PrivateProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            protected int? ProtectedNullabledProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            public int PublicProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            private static int PrivateStaticProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            protected int? ProtectedNullabledStaticProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedMember.Global
            public int PublicStaticProperty { get; set; }
        }
    }
}
