using MvvmCross;
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
        public BaseAppStart(IMvxApplication application) : base(application)
        {
        }

        /// <summary>
        /// 程序启动导航到第一个页面
        /// </summary>
        /// <param name="hint"></param>
        protected override void Startup(object hint = null)
        {
            base.Startup(hint);

            //启动第一个页面
            Mvx.Resolve<IMvxNavigationService>().Navigate<TFirstViewModel>();
        }
    }
}
