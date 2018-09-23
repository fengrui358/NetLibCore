using System.Threading.Tasks;
using MvvmCross.Commands;

namespace FrHello.NetLib.Core.Mvx
{
    /// <summary>
    /// 可编辑的ViewModel
    /// </summary>
    public abstract class BaseEditViewModel : BaseViewModel
    {
        private bool _canConfirm = true;

        /// <summary>
        /// 确认命令
        /// </summary>
        public MvxCommand ConfirmCommand { get; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public MvxCommand CancelCommand { get; }

        /// <summary>
        /// 是否可确认
        /// </summary>
        public virtual bool CanConfirm
        {
            get => _canConfirm && CanConfirmFun();
            set => SetProperty(ref _canConfirm, value);
        }

        /// <summary>
        /// 构造
        /// </summary>
        protected BaseEditViewModel()
        {
            ConfirmCommand = new MvxCommand(ConfirmCommandHandler);
            CancelCommand = new MvxCommand(Close);
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> Confirm();

        /// <summary>
        /// 是否可确认(由CanConfirm的通知触发)
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanConfirmFun()
        {
            return true;
        }

        private async void ConfirmCommandHandler()
        {
            if (await Confirm())
            {
                Close();
            }
        }
    }

    /// <summary>
    /// 可编辑的ViewModel基类
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public abstract class BaseEditViewModel<TParameter> : BaseViewModel<TParameter>
    {
        private bool _canConfirm = true;

        /// <summary>
        /// 确认命令
        /// </summary>
        public MvxCommand ConfirmCommand { get; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public MvxCommand CancelCommand { get; }

        /// <summary>
        /// 是否可确认
        /// </summary>
        public virtual bool CanConfirm
        {
            get => _canConfirm && CanConfirmFun();
            set => SetProperty(ref _canConfirm, value);
        }

        /// <summary>
        /// 构造
        /// </summary>
        protected BaseEditViewModel()
        {
            ConfirmCommand = new MvxCommand(ConfirmCommandHandler);
            CancelCommand = new MvxCommand(Close);
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> Confirm();

        /// <summary>
        /// 是否可确认(由CanConfirm的通知触发)
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanConfirmFun()
        {
            return true;
        }

        private async void ConfirmCommandHandler()
        {
            if (await Confirm())
            {
                Close();
            }
        }
    }
}
