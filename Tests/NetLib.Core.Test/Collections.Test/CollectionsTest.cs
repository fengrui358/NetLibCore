using System.Collections;
using System.Collections.Generic;
using FrHello.NetLib.Core.Collections;
using Xunit;

namespace NetLib.Core.Test.Collections.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class CollectionsTest
    {
        /// <summary>
        /// EnumerableIsNullOrEmpty
        /// </summary>
        [Fact]
        public void EnumerableIsNullOrEmpty()
        {
            IEnumerable target = new List<string>();
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

            IEnumerable target2 = target;
            Assert.False(target2.IsNullOrEmpty());
        }

        /// <summary>
        /// EnumerableCount
        /// </summary>
        [Fact]
        public void EnumerableCount()
        {
            IEnumerable target = new List<string> { "test", "test2" };
            Assert.Equal(2, target.Count());
        }
    }
}
