using FrHello.NetLib.Core.Reflection;
using Xunit;

namespace NetLib.Core.Test.Reflection.Test
{
    /// <summary>
    /// ReflectionHelperTest
    /// </summary>
    public class ReflectionHelperTest
    {
        /// <summary>
        /// GetInvokerInfoTest
        /// </summary>
        [Fact]
        public void GetInvokerInfoTest()
        {
            Assert.Equal(nameof(ReflectionHelperTest), GetClassName());
            Assert.Equal(nameof(ReflectionHelperTest.GetInvokerInfoTest), GetMethodName());
        }

        private string GetClassName()
        {
            ReflectionHelper.GetInvokerInfo(out var className, out var methodName);
            return className;
        }

        private string GetMethodName()
        {
            ReflectionHelper.GetInvokerInfo(out var className, out var methodName);
            return methodName;
        }
    }
}
