// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 默认的网络工具相关参数
    /// </summary>
    public static class GlobalNetOptions
    {
        /// <summary>
        /// 默认Ping超时等待时间，秒
        /// </summary>
        public static int DefaultPingTimeOut { get; set; } = 5000;
    }
}
