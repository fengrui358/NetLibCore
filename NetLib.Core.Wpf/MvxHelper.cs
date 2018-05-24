using System;
using System.Reflection;
using System.Windows;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Presenters;
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
        /// <param name="mainWindow">主窗体</param>
        /// <param name="firstViewModel">第一个视图</param>
        /// <param name="mvxViewPresenter">呈现控制器</param>
        /// <param name="mvxSetup">启动流程控制器</param>
        /// <param name="mvxApplication">ViewModelCore当中的</param>
        /// <param name="mvxAppStart">程序启动控制</param>
        /// <param name="viewAssemblies">视图程序集</param>
        /// <param name="viewModelAssemblies">ViewModel程序集</param>
        public static void Init(Window mainWindow, MvxViewModel firstViewModel, IMvxViewPresenter mvxViewPresenter = null,
            IMvxWpfSetup mvxSetup = null, IMvxApplication mvxApplication = null, IMvxAppStart mvxAppStart = null, Assembly[] viewAssemblies = null,
            Assembly[] viewModelAssemblies = null)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException(nameof(mainWindow));
            }

            if (firstViewModel == null)
            {
                throw new ArgumentNullException(nameof(firstViewModel));
            }

            if (mvxViewPresenter == null)
            {
                mvxViewPresenter = new MvxWpfViewPresenter(mainWindow);
            }

            if (mvxSetup == null)
            {
                mvxSetup = new BaseSetup();
            }

            mvxSetup.PlatformInitialize(Application.Current.Dispatcher, mainWindow);
            mvxSetup.InitializePrimary();
            mvxSetup.InitializeSecondary();


        }
    }
}
