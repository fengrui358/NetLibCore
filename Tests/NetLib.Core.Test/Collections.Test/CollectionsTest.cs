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
            IEnumerable<string> target = new List<string>();
            Assert.True(target.IsNullOrEmpty());

            target = null;
            Assert.True(target.IsNullOrEmpty());
        }

        [Fact]
        public void EnumerableIsNotNullOrEmpty()
        {
            IEnumerable<string> target = new List<string> {"test"};
            Assert.False(target.IsNullOrEmpty());
        }
    }
}
