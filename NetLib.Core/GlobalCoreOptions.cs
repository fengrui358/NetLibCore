using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NetLib.Core.Test")]

namespace FrHello.NetLib.Core
{
    /// <summary>
    /// 默认的核心工具相关参数
    /// </summary>
    public static class GlobalCoreOptions
    {
        /// <summary>
        /// 默认字符串省略长度，如果超过这个长度就变成省略号
        /// </summary>
        public static int DefaultStringEllipsisLength { get; set; } = 200;
    }
}
