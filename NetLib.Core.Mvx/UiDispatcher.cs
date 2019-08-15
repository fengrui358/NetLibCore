using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;

namespace FrHello.NetLib.Core.Mvx
{
    /// <summary>
    /// 主线程辅助类
    /// </summary>
    public class UiDispatcherHelper
    {
        private static IMvxMainThreadAsyncDispatcher _mainThreadAsyncDispatcher;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Ui主线程调用
        /// </summary>
        /// <param name="action">执行方法</param>
        public static void Invoke(Action action)
        {
            if (action == null) return;
            if (_mainThreadAsyncDispatcher == null)
            {
                Task.Run(async () =>
                {
                    await SemaphoreSlim.WaitAsync();

                    try
                    {
                        if (_mainThreadAsyncDispatcher == null)
                        {
                            if (MvvmCross.Mvx.IoCProvider != null &&
                                MvvmCross.Mvx.IoCProvider.CanResolve(typeof(IMvxMainThreadAsyncDispatcher)))
                            {
                                _mainThreadAsyncDispatcher =
                                    MvvmCross.Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
                            }
                            else
                            {
                                do
                                {
                                    await Task.Delay(5);

                                    if (MvvmCross.Mvx.IoCProvider != null &&
                                        MvvmCross.Mvx.IoCProvider.CanResolve(typeof(IMvxMainThreadAsyncDispatcher)))
                                    {
                                        _mainThreadAsyncDispatcher = MvvmCross.Mvx.IoCProvider
                                            .Resolve<IMvxMainThreadAsyncDispatcher>();
                                        break;
                                    }
                                } while (_mainThreadAsyncDispatcher == null);
                            }
                        }
                    }
                    finally
                    {
                        SemaphoreSlim.Release();
#pragma warning disable 4014
                        _mainThreadAsyncDispatcher?.ExecuteOnMainThreadAsync(action);
#pragma warning restore 4014
                    }
                });
            }
            else
            {
                _mainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(action);
            }
        }

        /// <summary>
        /// Ui主线程调用并等待返回结果
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <returns>等待任务</returns>
        public static async Task InvokeAsync(Action action)
        {
            if (action == null) return;
            if (_mainThreadAsyncDispatcher == null)
            {
                await SemaphoreSlim.WaitAsync();

                try
                {
                    if (_mainThreadAsyncDispatcher == null)
                    {
                        if (MvvmCross.Mvx.IoCProvider != null &&
                            MvvmCross.Mvx.IoCProvider.CanResolve(typeof(IMvxMainThreadAsyncDispatcher)))
                        {
                            _mainThreadAsyncDispatcher =
                                MvvmCross.Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
                        }
                        else
                        {

                            do
                            {
                                await Task.Delay(5);

                                if (MvvmCross.Mvx.IoCProvider != null &&
                                    MvvmCross.Mvx.IoCProvider.CanResolve(typeof(IMvxMainThreadAsyncDispatcher)))
                                {
                                    _mainThreadAsyncDispatcher = MvvmCross.Mvx.IoCProvider
                                        .Resolve<IMvxMainThreadAsyncDispatcher>();
                                    break;
                                }
                            } while (_mainThreadAsyncDispatcher == null);
                        }
                    }
                }
                finally
                {
                    SemaphoreSlim.Release();
                }
            }

            await _mainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(action);
        }
    }
}