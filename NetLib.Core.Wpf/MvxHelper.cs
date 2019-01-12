using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// MvvmCross框架封装辅助方法
    /// </summary>
    public class MvxHelper
    {
        private static readonly object LockObj = new object();
        private static bool _isCaptureGlobalExceptions;

        private static IMvxLog _log;

        /// <summary>
        /// 日志
        /// </summary>
        private static IMvxLog Log
        {
            get
            {
                if (_log == null)
                {
                    if (Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
                    {
                        _log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(typeof(MvxHelper).FullName);
                    }
                }

                return _log;
            }
        }

        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mvxViewPresenter">呈现控制器</param>
        /// <param name="mvxSetup">启动流程控制器</param>
        /// <param name="mvxApplication">ViewModelCore当中的</param>
        /// <param name="mvxAppStart">程序启动控制</param>
        /// <param name="viewAssemblies">视图程序集</param>
        /// <param name="viewModelAssemblies">ViewModel程序集</param>
        /// <param name="serviceAssemblies">提供服务工具的程序集，更具服务工具的接口注册为单例（服务的名称必须以Service结尾）</param>
        public static void Init<TMainWindow, TViewModel>(MvxWpfViewPresenter mvxViewPresenter = null,
            IMvxWpfSetup mvxSetup = null, IMvxApplication mvxApplication = null, IMvxAppStart mvxAppStart = null, Assembly[] viewAssemblies = null,
            Assembly[] viewModelAssemblies = null, Assembly[] serviceAssemblies = null) where TMainWindow : Window where TViewModel : MvxViewModel
        {
            var mainWindow = Activator.CreateInstance<TMainWindow>();

            if (mvxViewPresenter == null)
            {
                mvxViewPresenter = new MvxWpfViewPresenter(mainWindow);
            }

            if (mvxSetup == null)
            {
                mvxSetup = new BaseSetup<TMainWindow, TViewModel>(viewAssemblies, viewModelAssemblies);
            }

            mvxSetup.PlatformInitialize(Application.Current.Dispatcher, mvxViewPresenter);
            mvxSetup.InitializePrimary();
            mvxSetup.InitializeSecondary();

            if (serviceAssemblies != null)
            {
                foreach (var serviceAssembly in serviceAssemblies)
                {
                    serviceAssembly.CreatableTypes()
                        .EndingWith("Service")
                        .AsInterfaces()
                        .RegisterAsLazySingleton();
                }
            }

            var start = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            start.Start();

            mainWindow.Show();
        }

        /// <summary>
        /// 捕获全局异常
        /// </summary>
        public static void CaptureGlobalExceptions()
        {
            lock (LockObj)
            {
                if (!_isCaptureGlobalExceptions)
                {
                    Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

                    _isCaptureGlobalExceptions = true;
                }
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                Log.Fatal(String.Empty, exception);
            }
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(String.Empty, e.Exception);
            e.Handled = true;
        }
    }
}
