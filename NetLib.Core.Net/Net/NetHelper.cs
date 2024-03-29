﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FrHello.NetLib.Core.Utility;

// ReSharper disable once CheckNamespace
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
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(IPAddress address, int? timeout = null)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = ping.Send(address, timeout ?? GlobalNetOptions.DefaultPingTimeOut);
            return pingReply?.Status == IPStatus.Success;
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>是否连接成功</returns>
        public static async Task<bool> PingAsync(string hostNameOrAddress, int? timeout = null)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = await ping.SendPingAsync(UriHelper.BuildUri(hostNameOrAddress).Host, timeout ?? GlobalNetOptions.DefaultPingTimeOut);

            return pingReply?.Status == IPStatus.Success;
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>是否连接成功</returns>
        public static async Task<bool> PingAsync(IPAddress address, int? timeout = null)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = await ping.SendPingAsync(address, timeout ?? GlobalNetOptions.DefaultPingTimeOut);
            return pingReply?.Status == IPStatus.Success;
        }

        /// <summary>
        /// Ping某个地址是否正常工作
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>是否连接成功</returns>
        public static bool Ping(string hostNameOrAddress, int? timeout = null)
        {
            if (timeout < 0)
            {
                throw new ArgumentException(nameof(timeout));
            }

            var ping = new Ping();
            var pingReply = ping.Send(UriHelper.BuildUri(hostNameOrAddress).Host, timeout ?? GlobalNetOptions.DefaultPingTimeOut);

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
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>端口是否正在使用</returns>
        public static bool CheckRemotePort(IPAddress address, int port, int timeout)
        {
            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("The port range should be 0 ~ 65535.", nameof(port));
            }

            try
            {
                using var client = new TcpClient();
                var result = client.BeginConnect(address, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout);
                if (!success)
                {
                    return false;
                }

                client.EndConnect(result);
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
                using var client = new TcpClient();
                var result = client.BeginConnect(hostNameOrAddress, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout);
                if (!success)
                {
                    return false;
                }

                client.EndConnect(result);
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
        /// <param name="timeout">超时(MillionSeconds)</param>
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
                    using var client = new TcpClient();
                    var result = client.BeginConnect(address, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(timeout);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
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
        /// <param name="timeout">超时(MillionSeconds)</param>
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
                    using var client = new TcpClient();
                    var result = client.BeginConnect(UriHelper.BuildUri(hostNameOrAddress).Host, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(timeout);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// 检查某个远程地址的端口是否正在使用，最大限度地址的正确性来让API易于使用
        /// </summary>
        /// <param name="hostNameOrAddress">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时(MillionSeconds)</param>
        /// <returns>端口是否正在使用</returns>
        public static async Task<bool> CheckRemotePortEasy(string hostNameOrAddress, int? port = null,
            int? timeout = null)
        {
            if (string.IsNullOrEmpty(hostNameOrAddress))
            {
                return false;
            }

            var uri = UriHelper.BuildUri(hostNameOrAddress);
            hostNameOrAddress = uri.Host;
            if (uri.Port != 0 && port is null or 0)
            {
                port = uri.Port;
            }

            if (port == null)
            {
                //判断协议
                if (!string.IsNullOrEmpty(uri.Scheme))
                {
                    var p = GetDefaultPort(uri.Scheme);
                    port = p ?? 80;
                }
                else
                {
                    port = 80;
                }
            }

            if (port < 0 || port > 65535)
            {
                return false;
            }

            return await Task.Run(() =>
            {
                try
                {
                    using var client = new TcpClient();
                    var result = client.BeginConnect(UriHelper.BuildUri(hostNameOrAddress).Host, port.Value, null,
                        null);
                    var success = result.AsyncWaitHandle.WaitOne(timeout ?? GlobalNetOptions.DefaultPingTimeOut);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
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
        /// 获取本地所有的IP地址
        /// </summary>
        /// <param name="type">要获取IP地址的网卡类型</param>
        /// <param name="addressFamily">寻址方案</param>
        /// <returns>对应类型的IP地址</returns>
        public static IPAddress[] GetAllLocalIp(NetworkInterfaceType type,
            AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var ipAddressList = new List<IPAddress>();
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == addressFamily)
                        {
                            ipAddressList.Add(ip.Address);
                        }
                    }
                }
            }

            return ipAddressList.ToArray();
        }

        /// <summary>
        /// 获取本地所有的IPv4地址
        /// </summary>
        /// <param name="type">要获取IP地址的网卡类型</param>
        /// <returns></returns>
        public static IPAddress[] GetAllLocalIPv4(NetworkInterfaceType type)
        {
            return GetAllLocalIp(type);
        }


        /// <summary>
        /// 获取本地Ip
        /// </summary>
        /// <param name="type"></param>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static IPAddress GetLocalIp(NetworkInterfaceType type = NetworkInterfaceType.Ethernet,
            AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == addressFamily)
                        {
                            return ip.Address;
                        }
                    }
                }
            }

            return IPAddress.None;
        }

        /// <summary>
        /// 获取本地Ip
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPAddress GetLocalIpv4(NetworkInterfaceType type = NetworkInterfaceType.Ethernet)
        {
            return GetLocalIp(type);
        }

        /// <summary>
        /// 根据协议名称获取默认端口
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static int? GetDefaultPort(string protocol)
        {
            if (string.IsNullOrEmpty(protocol))
            {
                return null;
            }

            var index = protocol.FirstOrDefault(s => s == ':' || s == '/' || s == '\\');
            if (index > 0)
            {
                protocol = protocol.Substring(0, index);
            }

            protocol = protocol.ToUpper();

            foreach (ProtocolType protocolType in System.Enum.GetValues(typeof(ProtocolType)))
            {
                if (protocolType.ToString().ToUpper() == protocol)
                {
                    return protocolType.GetDefaultPort();
                }
            }

            return null;
        }

        #endregion
    }
}