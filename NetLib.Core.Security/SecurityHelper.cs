using FrHello.NetLib.Core.Security.Hash;

namespace FrHello.NetLib.Core.Security
{
    /// <summary>
    /// 安全功能辅助类
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Hash算法
        /// </summary>
        public static HashAlgorithms Hash { get; } = new HashAlgorithms();
    }
}
