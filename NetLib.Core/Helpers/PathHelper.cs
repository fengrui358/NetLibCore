using System;
using System.Linq;
using System.Text;

namespace FrHello.NetLib.Core.Helpers
{
    /// <summary>
    /// Path helper
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Url combine without protocol
        /// </summary>
        /// <param name="urlSplit">url split</param>
        /// <returns></returns>
        public static string UrlCombine(params string[] urlSplit)
        {
            return UrlCombine(null, urlSplit);
        }

        /// <summary>
        /// Url combine
        /// </summary>
        /// <param name="protocol">protocol</param>
        /// <param name="urlSplit">url split</param>
        /// <returns></returns>
        public static string UrlCombine(string protocol, params string[] urlSplit)
        {
            if (urlSplit == null)
            {
                throw new ArgumentNullException(nameof(urlSplit));
            }

            if (!string.IsNullOrEmpty(protocol))
            {
                if (!protocol.EndsWith("://"))
                {
                    protocol = $"{protocol}://";
                }
            }

            var result = new StringBuilder(protocol ?? string.Empty);

            var urls = string.Join("/", urlSplit.Select(s => s.Replace('\\', '/').ToLower()))
                .Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            
            for (var i = 0; i < urls.Length; i++)
            {
                if (i == 0)
                {
                    result.Append(urls[i]);
                }
                else
                {
                    result.Append($"/{urls[i]}");
                }
            }

            return result.ToString();
        }
    }
}
