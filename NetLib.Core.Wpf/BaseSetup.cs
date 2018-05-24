using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// todo:
    /// </summary>
    public class BaseSetup : MvxWpfSetup
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="viewAssemblies">视图程序集</param>
        /// <param name="viewModelAssemblies">ViewModel程序集</param>
        public BaseSetup(Assembly[] viewAssemblies = null, Assembly[] viewModelAssemblies = null)
        {

        }

        protected override IMvxApplication CreateApp()
        {
            return null;
        }

        /// <summary>
        /// 设置默认日志系统
        /// </summary>
        /// <returns></returns>
        public override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.NLog;
        }
    }
}
