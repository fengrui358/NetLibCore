using System;
using System.Collections.Generic;
using System.Text;
using FrHello.NetLib.Core.Reflection;
using Xunit;

namespace NetLib.Core.Test.Reflection.Test
{
    /// <summary>
    /// TypeTest
    /// </summary>
    public class TypeTest
    {
        /// <summary>
        /// HasDefaultConstructorTest
        /// </summary>
        [Fact]
        public void HasDefaultConstructorTest()
        {
            Assert.False(TypeHelper.HasDefaultConstructor(typeof(MockInterface)));
            Assert.False(TypeHelper.HasDefaultConstructor(typeof(MockAbstractClass)));
            Assert.False(TypeHelper.HasDefaultConstructor(typeof(MockPrivateClass)));
            Assert.True(TypeHelper.HasDefaultConstructor(typeof(MockParamsClass)));
        }

        /// <summary>
        /// GetRealTypeTest
        /// </summary>
        [Fact]
        public void GetRealTypeTest()
        {
            var type1 = typeof(MockAbstractClass);
            Assert.Equal(typeof(MockAbstractClass), type1.GetNullableInnerType());

            Assert.Equal(typeof(double), typeof(double?).GetNullableInnerType());
        }

#pragma warning disable xUnit2004 // Do not use equality check to test for boolean conditions

        /// <summary>
        /// IsSubclassOfTest
        /// </summary>
        [Fact]
        public void IsInheritedFromTest()
        {
            Assert.Equal(true, typeof(MockAbstractClass).IsInheritedFrom(typeof(MockInterface)));
            Assert.Equal(false, typeof(MockInterface).IsInheritedFrom(typeof(MockAbstractClass)));

            Assert.Equal(true, typeof(MockParamsClass).IsInheritedFrom(typeof(MockInterface)));
            Assert.Equal(false, typeof(MockInterface).IsInheritedFrom(typeof(MockParamsClass)));

            Assert.Equal(true, typeof(MockParamsClass).IsInheritedFrom(typeof(MockAbstractClass)));
            Assert.Equal(false, typeof(MockAbstractClass).IsInheritedFrom(typeof(MockParamsClass)));

            Assert.Equal(false, typeof(MockParamsClass).IsInheritedFrom(typeof(MockParamsClass)));

            Assert.Equal(true, typeof(MockGenericType).IsInheritedFrom(typeof(MockGenericType<>)));

            Assert.Equal(false, typeof(MockGenericTypeSubType2).IsInheritedFrom(typeof(MockGenericType<>)));
            Assert.Equal(true, typeof(MockGenericTypeSubType2).IsInheritedFrom(typeof(MockGenericType<>), true));
        }

#pragma warning restore xUnit2004 // Do not use equality check to test for boolean conditions
    }

    /// <summary>
    /// MockInterface
    /// </summary>
    public interface MockInterface
    {
    }

    /// <summary>
    /// MockAbstractClass
    /// </summary>
    public abstract class MockAbstractClass : MockInterface
    {
    }

    /// <summary>
    /// MockPrivateClass
    /// </summary>
    public class MockPrivateClass
    {
        private MockPrivateClass()
        {
        }
    }

    /// <summary>
    /// MockParamsClass
    /// </summary>
    public class MockParamsClass : MockAbstractClass
    {
        /// <summary>
        /// MockParamsClass
        /// </summary>
        /// <param name="i"></param>
        public MockParamsClass(int i)
        {
        }

        /// <summary>
        /// MockParamsClass
        /// </summary>
        public MockParamsClass()
        {
        }

        static MockParamsClass()
        {
        }
    }

    /// <summary>
    /// MockGenericType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MockGenericType<T> : MockInterface
    {
    }

    /// <summary>
    /// MockGenericType
    /// </summary>
    public class MockGenericType : MockGenericType<object> { }

    /// <summary>
    /// MockGenericTypeSubType
    /// </summary>
    public class MockGenericTypeSubType : MockGenericType { }

    /// <summary>
    /// MockGenericTypeSubType2
    /// </summary>
    public class MockGenericTypeSubType2 : MockGenericType { }
}
