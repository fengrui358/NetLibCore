using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// MvxWpfSetup重写部分启动流程
    /// </summary>
    public class BaseSetup<TMainWindow, TViewModel> : MvxWpfSetup where TMainWindow : Window where TViewModel : MvxViewModel
    {
        /// <summary>
        /// 视图对应的程序集
        /// </summary>
        private readonly Assembly[] _viewAssemblies;

        /// <summary>
        /// ViewModel对应的程序集
        /// </summary>
        private readonly Assembly[] _viewModelAssemblies;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="viewAssemblies">视图程序集</param>
        /// <param name="viewModelAssemblies">ViewModel程序集</param>
        public BaseSetup(Assembly[] viewAssemblies = null, Assembly[] viewModelAssemblies = null)
        {
            _viewAssemblies = viewAssemblies;
            _viewModelAssemblies = viewModelAssemblies;
        }

        /// <summary>
        /// 创建App
        /// </summary>
        /// <returns></returns>
        protected override IMvxApplication CreateApp()
        {
            var app = new BaseApp();
            Mvx.RegisterSingleton<IMvxAppStart>(new BaseAppStart<TViewModel>(app));

            return app;
        }

        /// <summary>
        /// 设置默认日志系统
        /// </summary>
        /// <returns></returns>
        public override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.NLog;
        }

        /// <summary>
        /// 获取视图程序集
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies != null)
            {
                var mainWindowAssembly = typeof(TMainWindow).Assembly;
                var viewAssemblies = _viewAssemblies.ToList();

                if (!viewAssemblies.Contains(mainWindowAssembly))
                {
                    viewAssemblies.Add(mainWindowAssembly);
                }

                return viewAssemblies;
            }
            else
            {
                return base.GetViewAssemblies();
            }
        }

        /// <summary>
        /// 获取ViewModel程序集
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            if (_viewModelAssemblies != null)
            {
                var firstViewModelAssembly = typeof(TViewModel).Assembly;
                var viewModelAssemblies = _viewModelAssemblies.ToList();

                if (!viewModelAssemblies.Contains(firstViewModelAssembly))
                {
                    viewModelAssemblies.Add(firstViewModelAssembly);
                }

                return viewModelAssemblies;
            }
            else
            {
                return base.GetViewModelAssemblies();
            }
        }
    }
}
