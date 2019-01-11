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
        private bool _isLoadedFirstTime;
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
                    if (Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
                    {
                        _log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public BaseView()
        {
            Loaded += (sender, args) =>
            {
                if (!_isLoadedFirstTime)
                {
                    _isLoadedFirstTime = true;

                    LoadedFirstTime();
                }
            };
        }

        /// <summary>
        /// 第一次加载
        /// </summary>
        protected virtual void LoadedFirstTime()
        {
        }
    }

    /// <summary>
    /// View基类
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel</typeparam>
    public class BaseView<TViewModel> : MvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        private bool _isLoadedFirstTime;
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
                    if (Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
                    {
                        _log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(GetType().FullName);
                    }
                }

                return _log;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public BaseView()
        {
            Loaded += (sender, args) =>
            {
                if (!_isLoadedFirstTime)
                {
                    _isLoadedFirstTime = true;

                    LoadedFirstTime();
                }
            };
        }

        /// <summary>
        /// 第一次加载
        /// </summary>
        protected virtual void LoadedFirstTime()
        {
        }
    }
}