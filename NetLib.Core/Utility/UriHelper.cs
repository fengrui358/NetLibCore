using System;

namespace FrHello.NetLib.Core.Utility
{
    /// <summary>
    /// UriHelper
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// 将字符串转换为 uri 结构化对象
        /// </summary>
        /// <param name="host">ip or domain</param>
        /// <param name="port">如果 host 中带了端口号，也会替换端口号</param>
        /// <param name="protocol">http, https, ftp... 如果本身 host 中带了协议则不会替换协议</param>
        /// <returns></returns>
        public static Uri BuildUri(string host, int? port = null, string protocol = "http")
        {
            const string protocolFlag = "://";
            if (!host.Contains(protocolFlag))
            {
                // 不含协议信息
                host = $"{protocol}{protocolFlag}{host}";
            }

            if (port.HasValue)
            {
                // 替换端口
                if (System.Text.RegularExpressions.Regex.Match(host, @"(?<=:)\d+(?=[/$])").Success)
                {
                    // 原地址中含有端口号，可用正则表达式直接替换
                    host = System.Text.RegularExpressions.Regex.Replace(host, @"(?<=:)\d+(?=[/$])", port.ToString());
                }
                else
                {
                    // 找到左起第三个 "/"，在之前插入端口号
                    var index = host.IndexOf('/', host.IndexOf(protocolFlag, StringComparison.OrdinalIgnoreCase) + 3);
                    if (index < 0)
                    {
                        host += $":{port}";
                    }
                    else
                    {
                        host = host.Insert(index, $":{port}");
                    }
                }
            }

            return new Uri(host);
        }
    }
}
