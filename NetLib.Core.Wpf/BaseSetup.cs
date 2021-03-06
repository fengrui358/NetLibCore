﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Navigation;
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
            return new BaseApp();
        }

        /// <summary>
        /// 初始化的最后一步，准备启动程序
        /// </summary>
        protected override void InitializeLastChance()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxAppStart>(new BaseAppStart<TViewModel>(
                Mvx.IoCProvider.Resolve<IMvxApplication>(),
                Mvx.IoCProvider.Resolve<IMvxNavigationService>()));
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
        /// <returns>视图所在的程序集集合</returns>
        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var mainWindowAssembly = typeof(TMainWindow).Assembly;

            if (_viewAssemblies != null)
            {
                var viewAssemblies = _viewAssemblies.ToList();

                if (!viewAssemblies.Contains(mainWindowAssembly))
                {
                    viewAssemblies.Add(mainWindowAssembly);
                }

                return viewAssemblies;
            }
            else
            {
                return new[]{ mainWindowAssembly };
            }
        }

        /// <summary>
        /// 获取ViewModel程序集
        /// </summary>
        /// <returns>ViewModel所在的程序集集合</returns>
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var firstViewModelAssembly = typeof(TViewModel).Assembly;

            if (_viewModelAssemblies != null)
            {
                var viewModelAssemblies = _viewModelAssemblies.ToList();

                if (!viewModelAssemblies.Contains(firstViewModelAssembly))
                {
                    viewModelAssemblies.Add(firstViewModelAssembly);
                }

                return viewModelAssemblies;
            }
            else
            {
                return new[] { firstViewModelAssembly };
            }
        }
    }
}
