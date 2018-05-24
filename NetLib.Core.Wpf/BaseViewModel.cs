using System;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    public class BaseViewModel : MvxViewModel
    {
        private readonly Lazy<IMvxNavigationService> _navigationService = new Lazy<IMvxNavigationService>(Mvx.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        protected IMvxNavigationService NavigationService => _navigationService.Value;
    }

    /// <summary>
    /// ViewModel基类
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public class BaseViewModel<TParameter> : MvxViewModel<TParameter>
    {
        private readonly Lazy<IMvxNavigationService> _navigationService = new Lazy<IMvxNavigationService>(Mvx.Resolve<IMvxNavigationService>);

        /// <summary>
        /// 导航服务
        /// </summary>
        protected IMvxNavigationService NavigationService => _navigationService.Value;

        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="parameter"></param>
        public override void Prepare(TParameter parameter)
        {
        }
    }
}
