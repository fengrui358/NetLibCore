using System;
using System.Reflection;
using System.Windows;
using MvvmCross;
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
        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mvxViewPresenter">呈现控制器</param>
        /// <param name="mvxSetup">启动流程控制器</param>
        /// <param name="mvxApplication">ViewModelCore当中的</param>
        /// <param name="mvxAppStart">程序启动控制</param>
        /// <param name="viewAssemblies">视图程序集</param>
        /// <param name="viewModelAssemblies">ViewModel程序集</param>
        public static void Init<TMainWindow, TViewModel>(MvxWpfViewPresenter mvxViewPresenter = null,
            IMvxWpfSetup mvxSetup = null, IMvxApplication mvxApplication = null, IMvxAppStart mvxAppStart = null, Assembly[] viewAssemblies = null,
            Assembly[] viewModelAssemblies = null) where TMainWindow : Window where TViewModel : BaseViewModel
        {
            var mainWindow = Activator.CreateInstance<TMainWindow>();

            if (mvxViewPresenter == null)
            {
                mvxViewPresenter = new MvxWpfViewPresenter(mainWindow);
            }

            if (mvxSetup == null)
            {
                mvxSetup = new BaseSetup<TMainWindow, TViewModel>();
            }

            mvxSetup.PlatformInitialize(Application.Current.Dispatcher, mvxViewPresenter);
            mvxSetup.InitializePrimary();
            mvxSetup.InitializeSecondary();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            mainWindow.Show();
        }
    }
}
