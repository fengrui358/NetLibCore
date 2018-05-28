using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// View基类
    /// </summary>
    public class BaseView : MvxWpfView
    {
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
                    if (Mvx.CanResolve<IMvxLogProvider>())
                    {
                        _log = Mvx.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }
    }

    /// <summary>
    /// View基类
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel</typeparam>
    public class BaseView<TViewModel> : MvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
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
                    if (Mvx.CanResolve<IMvxLogProvider>())
                    {
                        _log = Mvx.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }
    }
}
