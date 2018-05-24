using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Presenters;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// MvvmCross框架封装辅助方法 //todo
    /// </summary>
    public class MvxHelper
    {
        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        public static void Init(Window mainWindow)
        {
            Init(mainWindow, null, null);
        }

        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        /// <param name="mvxViewPresenter">呈现控制器</param>
        public static void Init(Window mainWindow, IMvxViewPresenter mvxViewPresenter)
        {

        }

        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        /// <param name="iMvxSetup">启动控制器</param>
        public static void Init(Window mainWindow, IMvxSetup iMvxSetup)
        {

        }

        /// <summary>
        /// 在程序的Application的OnStartup方法中使用
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        /// <param name="mvxViewPresenter">呈现控制器</param>
        /// <param name="iMvxSetup">启动控制器</param>
        public static void Init(Window mainWindow, IMvxViewPresenter mvxViewPresenter, IMvxSetup iMvxSetup)
        {
            if (mvxViewPresenter == null)
            {
                var mvxWpfViewPresenter = new MvxWpfViewPresenter(mainWindow);
                mvxViewPresenter = (IMvxViewPresenter)mvxWpfViewPresenter;
            }

            if (iMvxSetup == null)
            {

            }
        }
    }
}
