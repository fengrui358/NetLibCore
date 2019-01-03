using System;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Mvx
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    public class BaseViewModel : MvxViewModel
    {
        /// <summary>
        /// 页面第一次呈现前标记
        /// </summary>
        private bool _viewAppearingFirstTime = false;

        /// <summary>
        /// 页面第一次呈现后标记
        /// </summary>
        private bool _viewAppearedFirstTime = false;

        /// <summary>
        /// 导航服务
        /// </summary>
        private readonly Lazy<IMvxNavigationService> _navigationService =
            new Lazy<IMvxNavigationService>(MvvmCross.Mvx.IoCProvider.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        public IMvxNavigationService NavigationService => _navigationService.Value;

        private IMvxLog _log;

        /// <summary>
        /// 日志
        /// </summary>
        protected IMvxLog Log
        {
            get
            {
                if (_log == null)
                {
                    if (MvvmCross.Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
                    {
                        _log = MvvmCross.Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }

        /// <summary>
        /// 页面第一次呈现前
        /// </summary>
        public virtual void ViewAppearingFirstTime()
        {
        }

        /// <summary>
        /// 页面第一次呈现后
        /// </summary>
        public virtual void ViewAppearedFirstTime()
        {
        }

        /// <summary>
        /// 页面呈现前
        /// </summary>
        public override void ViewAppearing()
        {
            if (!_viewAppearingFirstTime)
            {
                ViewAppearingFirstTime();
            }

            base.ViewAppearing();
        }

        /// <summary>
        /// 页面呈现后
        /// </summary>
        public override void ViewDisappeared()
        {
            if (!_viewAppearedFirstTime)
            {
                ViewAppearedFirstTime();
            }

            base.ViewDisappeared();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            NavigationService.Close(this);
        }
    }

    /// <summary>
    /// ViewModel基类
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public class BaseViewModel<TParameter> : MvxViewModel<TParameter>
    {
        /// <summary>
        /// 页面第一次呈现前标记
        /// </summary>
        private bool _viewAppearingFirstTime = false;

        /// <summary>
        /// 页面第一次呈现后标记
        /// </summary>
        private bool _viewAppearedFirstTime = false;

        /// <summary>
        /// 导航服务
        /// </summary>
        private readonly Lazy<IMvxNavigationService> _navigationService =
            new Lazy<IMvxNavigationService>(MvvmCross.Mvx.IoCProvider.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        public IMvxNavigationService NavigationService => _navigationService.Value;

        private IMvxLog _log;

        /// <summary>
        /// 日志
        /// </summary>
        protected IMvxLog Log
        {
            get
            {
                if (_log == null)
                {
                    if (MvvmCross.Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
                    {
                        _log = MvvmCross.Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }

        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="parameter"></param>
        public override void Prepare(TParameter parameter)
        {
        }

        /// <summary>
        /// 页面第一次呈现前
        /// </summary>
        public virtual void ViewAppearingFirstTime()
        {
        }

        /// <summary>
        /// 页面第一次呈现后
        /// </summary>
        public virtual void ViewAppearedFirstTime()
        {
        }

        /// <summary>
        /// 页面呈现前
        /// </summary>
        public override void ViewAppearing()
        {
            if (!_viewAppearingFirstTime)
            {
                ViewAppearingFirstTime();
            }

            base.ViewAppearing();
        }

        /// <summary>
        /// 页面呈现后
        /// </summary>
        public override void ViewDisappeared()
        {
            if (!_viewAppearedFirstTime)
            {
                ViewAppearedFirstTime();
            }

            base.ViewDisappeared();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            NavigationService.Close(this);
        }
    }
}
