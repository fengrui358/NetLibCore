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
        private readonly Lazy<IMvxNavigationService> _navigationService = new Lazy<IMvxNavigationService>(MvvmCross.Mvx.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        public override IMvxNavigationService NavigationService => _navigationService.Value;

        private IMvxLog _log;

        /// <summary>
        /// 日志
        /// </summary>
        protected override IMvxLog Log
        {
            get
            {
                if (_log == null)
                {
                    if (MvvmCross.Mvx.CanResolve<IMvxLogProvider>())
                    {
                        _log = MvvmCross.Mvx.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
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
        private readonly Lazy<IMvxNavigationService> _navigationService = new Lazy<IMvxNavigationService>(MvvmCross.Mvx.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        public override IMvxNavigationService NavigationService => _navigationService.Value;

        private IMvxLog _log;

        /// <summary>
        /// 日志
        /// </summary>
        protected override IMvxLog Log
        {
            get
            {
                if (_log == null)
                {
                    if (MvvmCross.Mvx.CanResolve<IMvxLogProvider>())
                    {
                        _log = MvvmCross.Mvx.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
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
        /// 关闭
        /// </summary>
        public void Close()
        {
            NavigationService.Close(this);
        }
    }
}
