using System;
using FrHello.NetLib.Core.Reflection;
using Xunit;

namespace NetLib.Core.Test.Reflection.Test
{
    /// <summary>
    /// ChangeTypeTest
    /// </summary>
    public class ChangeTypeTest
    {
        /// <summary>
        /// 是否为可空类型判断
        /// </summary>
        [Fact]
        public void IsNullableTypeTest()
        {
            Assert.False(TypeHelper.IsNullableType(typeof(int)));
            Assert.True(TypeHelper.IsNullableType(typeof(double?)));
            Assert.False(TypeHelper.IsNullableType(typeof(MockClass)));
            Assert.False(TypeHelper.IsNullableType(typeof(MockClass<>)));
        }

        /// <summary>
        /// 类型转换测试
        /// </summary>
        [Fact]
        public void TypeChangeTest()
        {
            //可空类型转不可空类型
            double? intNullable = 45.36;

            var actual = (int?) TypeHelper.TryChangeType(intNullable, typeof(int));
            Assert.Equal(45, actual);
        }

        /// <summary>
        /// 获取TryParse方法
        /// </summary>
        [Fact]
        public void GetTryParseMethodTest()
        {
            var m = TypeHelper.GetTryParseMethod(typeof(int));
            Assert.NotNull(m);

            var args = new object[] { "12", null };
            var r = m.Invoke(null, args);

            Assert.True((bool) r);
            Assert.Equal(12, args[1]);
        }

        /// <summary>
        /// 获取Parse方法
        /// </summary>
        [Fact]
        public void GetParseMethodTest()
        {
            var m = TypeHelper.GetParseMethod(typeof(Version));
            Assert.NotNull(m);

            var args = new object[] { "12.3" };
            var r = m.Invoke(null, args);

            Assert.Equal(new Version("12.3"), r);
        }

        /// <summary>
        /// 测试类
        /// </summary>
        public class MockClass
        {
            
        }

        /// <summary>
        /// 测试泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MockClass<T>
        {
            
        }
    }
}
