using System;
using System.Threading;

namespace FrHello.NetLib.Core.ClassFoundation
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        /// <summary>
        /// 锁定标识
        /// </summary>
        private static object AsyncObj = new object();

        /// <summary>
        /// 自定义的构造方法
        /// </summary>
        private static Func<T> _createInstanceFunc;

        /// <summary>
        /// 通过自定义构造方法创建的实例
        /// </summary>
        private static T _createInstanceFuncValue;

        /// <summary>
        /// 是否已经通过自定义构造方法返回过实例
        /// </summary>
        private static bool _hasSetCustomInstance;

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
                    if (!_hasSetCustomInstance)
                    {
                        lock (AsyncObj)
                        {
                            if (!_hasSetCustomInstance)
                            {
                                _createInstanceFuncValue = _createInstanceFunc();
                                _hasSetCustomInstance = true;
                            }
                        }
                    }

                    return _createInstanceFuncValue;
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
            if (createInstanceFunc == null)
            {
                throw new ArgumentNullException(nameof(createInstanceFunc));
            }

            lock (AsyncObj)
            {
                _hasSetCustomInstance = false;
                _createInstanceFunc = createInstanceFunc;
            }
        }

        /// <summary>
        /// Singleton
        /// </summary>
        protected Singleton()
        {
        }
    }
}
