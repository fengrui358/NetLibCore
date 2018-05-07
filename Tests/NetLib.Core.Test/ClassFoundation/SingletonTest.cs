using FrHello.NetLib.Core.ClassFoundation;
using Xunit;

namespace NetLib.Core.Test.ClassFoundation
{
    /// <summary>
    /// SingletonTest
    /// </summary>
    public class SingletonTest
    {
        /// <summary>
        /// CreateSingletonTest
        /// </summary>
        [Fact]
        public void CreateSingletonTest()
        {
            MockSingleton.CreateInstance(() => new MockSingleton { Id = 45 });
            var m1 = MockSingleton.Instance;
            MockSingleton.Instance.Id = 5;

            Assert.Equal(m1, MockSingleton.Instance);
            Assert.NotEqual(45, m1.Id);
        }

        /// <summary>
        /// 测试单例
        /// </summary>
        public class MockSingleton : Singleton<MockSingleton>
        {
            /// <summary>
            /// Id
            /// </summary>
            public int Id { get; set; }
        }
    }
}
