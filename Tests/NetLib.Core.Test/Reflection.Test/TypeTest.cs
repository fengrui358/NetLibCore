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
            Assert.Equal(typeof(MockAbstractClass), type1.GetRealType());

            Assert.Equal(typeof(double), typeof(double?).GetRealType());
        }
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
    public abstract class MockAbstractClass
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
}
