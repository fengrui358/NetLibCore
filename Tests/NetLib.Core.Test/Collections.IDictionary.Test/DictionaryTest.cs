using System.Collections.Generic;
using FrHello.NetLib.Core;
using Xunit;

namespace NetLib.Core.Test.Collections.IDictionary.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class DictionaryTest
    {
        /// <summary>
        /// DictionaryAddIfNotContainsKey
        /// </summary>
        [Fact]
        public void DictionaryAddIfNotContainsKey()
        {
            var dic = new Dictionary<string, string>();

            var addResult1 = dic.AddIfNotContainsKey("key1", "value1");
            Assert.True(addResult1);
            Assert.Single(dic);

            var addResult2 = dic.AddIfNotContainsKey("key1", "key1");
            Assert.False(addResult2);
            Assert.Single(dic);

            var addResult3 = dic.AddIfNotContainsKey("key2", () => "value2");
            Assert.True(addResult3);
            Assert.Equal(2, dic.Count);

            var addResult4 = dic.AddIfNotContainsKey("key2", () => "value2");
            Assert.False(addResult4);
            Assert.Equal(2, dic.Count);

            var addResult5 = dic.AddIfNotContainsKey("key3", s => "value3");
            Assert.True(addResult5);
            Assert.Equal(3, dic.Count);

            var addResult6 = dic.AddIfNotContainsKey("key3", s => "value3");
            Assert.False(addResult6);
            Assert.Equal(3, dic.Count);
        }
    }
}
