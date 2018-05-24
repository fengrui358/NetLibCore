using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// 基础的启动程序
    /// </summary>
    public class BaseApp : MvxApplication
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
        }
    }
}
