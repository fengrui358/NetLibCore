using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 网络检查工具
    /// </summary>
    public static class NetHelper
    {
        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(IPAddress address)
        {
            return Ping(address, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(IPAddress address, int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }
            
            var ping = new Ping();
            var pingReply = ping.Send(address, timeout);
            return pingReply?.Status == IPStatus.Success;
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(string hostNameOrAddress)
        {
            return Ping(hostNameOrAddress, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(string hostNameOrAddress, int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            //todo
            return false;
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckPortInUse(string address, int port)
        {
            //todo
            return false;
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckPortInUse(string address, int port, int timeout)
        {
            //todo
            return false;
        }

        /// <summary>
        /// 检查本地端口是否在使用
        /// </summary>
        /// <param name="port">某一个本地端口</param>
        /// <returns></returns>
        public static bool CheckPortInUse(int port)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveTcpListeners();

            return ipEndPoints.Any(endPoint => endPoint.Port == port);
        }
    }
}
