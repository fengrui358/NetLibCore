using System.Collections.Generic;
using FrHello.NetLib.Core;
using Xunit;

namespace NetLib.Core.Test.Collections.IEnumerable.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class EnumerableTest
    {
        /// <summary>
        /// EnumerableIsNullOrEmpty
        /// </summary>
        [Fact]
        public void EnumerableIsNullOrEmpty()
        {
            System.Collections.IEnumerable target = new List<string>();
            Assert.True(target.IsNullOrEmpty());

            target = null;
            Assert.True(target.IsNullOrEmpty());
        }

        /// <summary>
        /// EnumerableIsNotNullOrEmpty
        /// </summary>
        [Fact]
        public void EnumerableIsNotNullOrEmpty()
        {
            var target = new List<string> {"test"};
            Assert.False(target.IsNullOrEmpty());

            System.Collections.IEnumerable target2 = target;
            Assert.False(target2.IsNullOrEmpty());
        }

        /// <summary>
        /// EnumerableCount
        /// </summary>
        [Fact]
        public void EnumerableCount()
        {
            System.Collections.IEnumerable target = new List<string> { "test", "test2" };
            Assert.Equal(2, target.Count());

            target = null;
            Assert.Equal(0, target.Count());
        }
    }
}
