using System;

namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 常用协议
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// Http,80
        /// </summary>
        Http,

        /// <summary>
        /// Securely transferring web pages,443
        /// </summary>
        Https,

        /// <summary>
        /// Telnet,23
        /// </summary>
        Telnet,

        /// <summary>
        /// Ftp,21
        /// </summary>
        Ftp,

        /// <summary>
        /// Trivial File Transfer Protocol,69
        /// </summary>
        TFTP,

        /// <summary>
        /// Ssh,22
        /// </summary>
        Ssh,

        /// <summary>
        /// Simple Mail Transfer Protocol (E-mail),25
        /// </summary>
        Smtp,

        /// <summary>
        /// Post Office Protocol (E-mail),110
        /// </summary>
        Pop3
    }

    /// <summary>
    /// 协议枚举的扩展方法
    /// </summary>
    public static class ProtocolTypeExtension
    {
        /// <summary>
        /// 获取默认端口号
        /// </summary>
        /// <param name="protocolType"></param>
        /// <returns></returns>
        public static int GetDefaultPort(this ProtocolType protocolType)
        {
            switch (protocolType)
            {
                case ProtocolType.Http:
                    return 80;
                case ProtocolType.Https:
                    return 443;
                case ProtocolType.Telnet:
                    return 23;
                case ProtocolType.Ftp:
                    return 21;
                case ProtocolType.TFTP:
                    return 69;
                case ProtocolType.Ssh:
                    return 22;
                case ProtocolType.Smtp:
                    return 25;
                case ProtocolType.Pop3:
                    return 110;
            }

            throw new ArgumentException(nameof(protocolType));
        }
    }
}
