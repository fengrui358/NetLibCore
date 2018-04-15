using System;
using System.Threading;

namespace FrHello.NetLib.Core.ClassFoundation
{
    public class Singleton<T> where T : new()
    {
        /// <summary>
        /// 自定义的构造方法
        /// </summary>
        private static Func<T> _createInstanceFunc;
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(() => new T(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// 实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_createInstanceFunc != null)
                {
                    var s = _createInstanceFunc();
                    return s;
                }

                return LazyInstance.Value;
            }
        }

        /// <summary>
        /// 手动返回实例，自定义构造(用于可能有参数或单元测试的接缝)
        /// </summary>
        /// <param name="createInstanceFunc"></param>
        public static void CreateInstance(Func<T> createInstanceFunc)
        {
            _createInstanceFunc = createInstanceFunc;
        }

        private Singleton()
        {
        }
    }
}
