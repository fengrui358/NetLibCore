using System;
using MvvmCross.Base;

namespace FrHello.NetLib.Core.Mvx
{
    /// <summary>
    /// 主线程辅助类
    /// </summary>
    public class UiDispatcherHelper
    {
        private static IMvxMainThreadAsyncDispatcher _mainThreadAsyncDispatcher;
        private static readonly object Async = new object();

        /// <summary>
        /// Ui主线程调用
        /// </summary>
        /// <param name="action">执行方法</param>
        public static void Invoke(Action action)
        {
            if (action == null) return;
            if (_mainThreadAsyncDispatcher == null)
            {
                lock (Async)
                {
                    if (_mainThreadAsyncDispatcher == null)
                    {
                        _mainThreadAsyncDispatcher = MvvmCross.Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
                    }
                }
            }

            _mainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(action);
        }
    }
}