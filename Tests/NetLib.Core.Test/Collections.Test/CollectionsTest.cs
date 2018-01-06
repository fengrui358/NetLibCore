using System.Collections;
using System.Collections.Generic;
using FrHello.NetLib.Core.Collections;
using Xunit;

namespace NetLib.Core.Test.Collections.Test
{
    public class CollectionsTest
    {
        [Fact]
        public void EnumerableIsNullOrEmpty()
        {
            IEnumerable target = new List<string>();
            Assert.True(target.IsNullOrEmpty());

            target = null;
            Assert.True(target.IsNullOrEmpty());
        }

        [Fact]
        public void EnumerableIsNotNullOrEmpty()
        {
            var target = new List<string> {"test"};
            Assert.False(target.IsNullOrEmpty());

            IEnumerable target2 = target;
            Assert.False(target2.IsNullOrEmpty());
        }

        [Fact]
        public void EnumerableCount()
        {
            IEnumerable target = new List<string> { "test", "test2" };
            Assert.Equal(3, target.Count());
        }
    }
}
