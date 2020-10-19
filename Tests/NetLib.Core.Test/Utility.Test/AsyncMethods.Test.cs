using System.Threading.Tasks;
using FrHello.NetLib.Core.Utility;
using NetLib.Core.Test.ConstString;
using Xunit;

namespace NetLib.Core.Test.Utility.Test
{
    /// <summary>
    /// AsyncMethodsTest
    /// </summary>
    public class AsyncMethodsTest
    {
        private int _deBounceCount = 10;
        private int _throttleCount = 10;

        /// <summary>
        /// DeBounceTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void DeBounceTest()
        {
            var t = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    DeBounceInnerTest();
                }
            });

            t.Wait(105);
            Assert.NotEqual(10, _deBounceCount);
        }

        private void DeBounceInnerTest()
        {
            AsyncMethods.DeBounce(() => _deBounceCount--, 10, true);
        }

        /// <summary>
        /// DeBounceTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void ThrottleTest()
        {
            var t = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    ThrottleInnerTest();
                }
            });

            t.Wait(105);
            Assert.NotEqual(10, _throttleCount);
        }

        private void ThrottleInnerTest()
        {
            AsyncMethods.Throttle(() => _throttleCount--, 10);
        }
    }
}
