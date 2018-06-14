using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// 程序启动控制
    /// </summary>
    public class BaseAppStart<TFirstViewModel> : MvxAppStart where TFirstViewModel : MvxViewModel
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="application"></param>
        /// <param name="navigationService"></param>
        public BaseAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        /// <summary>
        /// 打开第一个视图页面
        /// </summary>
        /// <param name="hint"></param>
        protected override void NavigateToFirstViewModel(object hint = null)
        {
            NavigationService.Navigate<TFirstViewModel>();
        }
    }
}
