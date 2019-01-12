using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core.Net
{
    /// <summary>
    /// 网络检查工具
    /// </summary>
    public static class NetHelper
    {
        #region Ping

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
        public static async Task<bool> PingAsync(string hostNameOrAddress)
        {
            return await PingAsync(hostNameOrAddress, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>是否连接成功</returns>
        public static async Task<bool> PingAsync(string hostNameOrAddress, int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = await ping.SendPingAsync(hostNameOrAddress, timeout);

            return pingReply?.Status == IPStatus.Success;
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <returns>是否连接成功</returns>
        public static async Task<bool> PingAsync(IPAddress address)
        {
            return await PingAsync(address, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>是否连接成功</returns>
        public static async Task<bool> PingAsync(IPAddress address, int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = await ping.SendPingAsync(address, timeout);
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

            var ping = new Ping();
            var pingReply = ping.Send(hostNameOrAddress, timeout);

            return pingReply?.Status == IPStatus.Success;
        }

        #endregion

        #region CheckRemotePort

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckRemotePort(IPAddress address, int port)
        {
            return CheckRemotePort(address, port, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeoutMillionSeconds">超时(毫秒)</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckRemotePort(IPAddress address, int port, int timeoutMillionSeconds)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(address, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(timeoutMillionSeconds);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="port">端口</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckRemotePort(string hostNameOrAddress, int port)
        {
            return CheckRemotePort(hostNameOrAddress, port, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckRemotePort(string hostNameOrAddress, int port, int timeout)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(hostNameOrAddress, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(timeout);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <returns>端口是否正在使用</returns>
        public static async Task<bool> CheckRemotePortAsync(IPAddress address, int port)
        {
            return await CheckRemotePortAsync(address, port, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>端口是否正在使用</returns>
        public static async Task<bool> CheckRemotePortAsync(IPAddress address, int port, int timeout)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            return await Task.Run(() =>
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        var result = client.BeginConnect(address, port, null, null);
                        var success = result.AsyncWaitHandle.WaitOne(timeout);
                        if (!success)
                        {
                            return false;
                        }

                        client.EndConnect(result);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="port">端口</param>
        /// <returns>端口是否正在使用</returns>
        public static async Task<bool> CheckRemotePortAsync(string hostNameOrAddress, int port)
        {
            return await CheckRemotePortAsync(hostNameOrAddress, port, GlobalNetOptions.DefaultPingTimeOut);
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时(毫秒)</param>
        /// <returns>端口是否正在使用</returns>
        public static async Task<bool> CheckRemotePortAsync(string hostNameOrAddress, int port, int timeout)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            return await Task.Run(() =>
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        var result = client.BeginConnect(hostNameOrAddress, port, null, null);
                        var success = result.AsyncWaitHandle.WaitOne(timeout);
                        if (!success)
                        {
                            return false;
                        }

                        client.EndConnect(result);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        #endregion

        #region CheckLocalPort

        /// <summary>
        /// 检查本地端口是否在使用
        /// </summary>
        /// <param name="port">某一个本地端口</param>
        /// <returns></returns>
        public static bool CheckLocalPort(int port)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveTcpListeners();

            return ipEndPoints.Any(endPoint => endPoint.Port == port);
        }

        #endregion

        #region GetLocalIPAddress

        /// <summary>
        /// 获取本地所有的IPv4地址
        /// </summary>
        /// <param name="type">要获取IP地址的网卡类型</param>
        /// <returns>对应类型的IP地址</returns>
        public static IPAddress[] GetAllLocalIPv4(NetworkInterfaceType type)
        {
            var ipAddrList = new List<IPAddress>();
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address);
                        }
                    }
                }
            }

            return ipAddrList.ToArray();
        }

        #endregion
    }
}