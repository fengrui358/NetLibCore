using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// 转换的一些辅助方法
    /// </summary>
    public static class BaseSerializationExtensions
    {
        /// <summary>
        /// 字符数组转字符串
        /// </summary>
        /// <param name="chars">字符数组</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this char[] chars)
        {
            return chars == null ? null : new string(chars);
        }

        /// <summary>
        /// 字节数组转换为字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">字节长度</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this byte[] bytes, int? startIndex = null, int? length = null)
        {
            return ToStringEasy(bytes, GlobalSerializationOptions.DefaultEncoding);
        }

        /// <summary>
        /// 字节数组转换为字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">字节长度</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this byte[] bytes, Encoding encoding, int? startIndex = null,
            int? length = null)
        {
            if (encoding == null)
            {
                if (GlobalSerializationOptions.DefaultEncoding != null)
                {
                    encoding = GlobalSerializationOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(encoding));
                }
            }

            if (bytes == null)
            {
                return null;
            }

            bytes = GetRealBytes(bytes, startIndex, length);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 流转换为字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this Stream stream)
        {
            return stream.ToStringEasy(GlobalSerializationOptions.DefaultEncoding);
        }

        /// <summary>
        /// 流转换为字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this Stream stream, Encoding encoding)
        {
            return stream?.ToBytes().ToStringEasy(encoding);
        }

        /// <summary>
        /// 字符串转字符数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符数组</returns>
        public static char[] ToChars(this string str)
        {
            return str.ToCharArray();
        }

        /// <summary>
        /// 字符串转换为Base64字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="base64FormattingOptions">base64格式</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string str,
            Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
        {
            return ToBase64String(str, GlobalSerializationOptions.DefaultEncoding,
                base64FormattingOptions);
        }

        /// <summary>
        /// 字符串转换为Base64字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="base64FormattingOptions">base64格式</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string str, Encoding encoding,
            Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
        {
            if (encoding == null)
            {
                if (GlobalSerializationOptions.DefaultEncoding != null)
                {
                    encoding = GlobalSerializationOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(encoding));
                }
            }

            return str == null ? null : Convert.ToBase64String(str.ToBytes(encoding), base64FormattingOptions);
        }

        /// <summary>
        /// 字符串转换为Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="base64FormattingOptions">base64格式</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">字节长度</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this byte[] bytes,
            Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None, int? startIndex = null,
            int? length = null)
        {
            return bytes == null
                ? null
                : Convert.ToBase64String(GetRealBytes(bytes, startIndex, length), base64FormattingOptions);
        }

        /// <summary>
        /// 字符串转换为Base64字符串
        /// </summary>
        /// <param name="stream">字节流</param>
        /// <param name="base64FormattingOptions">base64格式</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this Stream stream,
            Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
        {
            return stream == null ? null : Convert.ToBase64String(stream.ToBytes(), base64FormattingOptions);
        }

        /// <summary>
        /// Base64字符串转换为明文字符串
        /// </summary>
        /// <param name="str">Base64字符串</param>
        /// <returns>明文字符串</returns>
        public static string ToStringFromBase64(this string str)
        {
            return str == null ? null : Convert.FromBase64String(str).ToStringEasy();
        }

        /// <summary>
        /// Base64字符串转换为字节数组
        /// </summary>
        /// <param name="str">Base64字符串</param>
        /// <returns>明文字符串</returns>
        public static byte[] ToBytesFromBase64(this string str)
        {
            return str == null ? null : Convert.FromBase64String(str);
        }

        /// <summary>
        /// Base64字符串转换为字节流
        /// </summary>
        /// <param name="str">Base64字符串</param>
        /// <returns>明文字符串</returns>
        public static Stream ToStreamFromBase64(this string str)
        {
            return str == null ? null : Convert.FromBase64String(str).ToStream();
        }

        /// <summary>
        /// 字符串转换为字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string str)
        {
            return str.ToBytes(GlobalSerializationOptions.DefaultEncoding);
        }

        /// <summary>
        /// 字符串转换为字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            if (encoding == null)
            {
                if (GlobalSerializationOptions.DefaultEncoding != null)
                {
                    encoding = GlobalSerializationOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(encoding));
                }
            }

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 流转字节数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="progress">进度通知</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this Stream stream, IProgress<double> progress = null)
        {
            if (stream == null)
            {
                return null;
            }

            if (stream.Position != 0L && stream.CanSeek)
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }

            var bytes = new byte[stream.Length];

            if (progress != null)
            {
                if (stream.Length > GlobalSerializationOptions.SegmentSize)
                {
                    var segmentCount =
                        (int) Math.Ceiling(bytes.Length / (double) GlobalSerializationOptions.SegmentSize);

                    if (segmentCount > 1)
                    {
                        var totalLength = 0;

                        for (int i = 0; i < segmentCount; i++)
                        {
                            var startIndex = i * GlobalSerializationOptions.SegmentSize;
                            var endIndex = startIndex + GlobalSerializationOptions.SegmentSize;
                            if (endIndex > bytes.Length)
                            {
                                endIndex = bytes.Length;
                            }

                            var length = endIndex - startIndex;
                            stream.Read(bytes, startIndex, length);
                            totalLength = totalLength + length;

                            progress.Report(totalLength / (double) bytes.Length);
                        }
                    }
                    else
                    {
                        progress.Report(1);
                    }
                }
            }
            else
            {
                stream.Read(bytes, 0, bytes.Length);
            }

            if (stream.Position != 0L && stream.CanSeek)
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }

            return bytes;
        }

        /// <summary>
        /// 字符串转流
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>流</returns>
        public static Stream ToStream(this string str)
        {
            return str == null ? null : ToStream(str, GlobalSerializationOptions.DefaultEncoding);
        }

        /// <summary>
        /// 字符串转流
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码规则</param>
        /// <returns>流</returns>
        public static Stream ToStream(this string str, Encoding encoding)
        {
            if (encoding == null)
            {
                if (GlobalSerializationOptions.DefaultEncoding != null)
                {
                    encoding = GlobalSerializationOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(encoding));
                }
            }

            return ToStream(str.ToBytes(encoding));
        }

        /// <summary>
        /// 字节数组转流
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">字节长度</param>
        /// <returns>流</returns>
        public static Stream ToStream(this byte[] bytes, int? startIndex = null,
            int? length = null)
        {
            return bytes == null ? null : new MemoryStream(GetRealBytes(bytes, startIndex, length));
        }

        /// <summary>
        /// 字节数组转十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">字节长度</param>
        /// <returns>流</returns>
        public static string ToHex(this byte[] bytes, int? startIndex = null,
            int? length = null)
        {
            if (bytes == null || !bytes.Any())
            {
                return string.Empty;
            }

            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var b in GetRealBytes(bytes, startIndex, length))
            {
                sBuilder.Append(b.ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 转换为Unicode编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>Unicode编码的字符串</returns>
        public static string ToUnicode(this string str)
        {
            var bytes = Encoding.Unicode.GetBytes(str);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0:x2}{1:x2}", bytes[i + 1], bytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将Unicode字符串转换为明文
        /// </summary>
        /// <param name="unicodeStr">Unicode字符串</param>
        /// <returns>明文字符串</returns>
        public static string ToStringFromUnicode(this string unicodeStr)
        {
            var dst = "";
            var src = unicodeStr;
            var len = unicodeStr.Length / 6;
            for (var i = 0; i <= len - 1; i++)
            {
                var str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                var bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)
                    .ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                    .ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }

            return dst;
        }

        /// <summary>
        /// 获取真正的字节数组
        /// </summary>
        /// <param name="bytes">待处理的字节数组</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">长度</param>
        /// <returns>截取后的字节数组</returns>
        private static byte[] GetRealBytes(byte[] bytes, int? startIndex, int? length)
        {
            if (startIndex == null)
            {
                return bytes;
            }
            else
            {
                if (length == null)
                {
                    length = bytes.Length - startIndex.Value;
                }

                var realBytes = new byte[length.Value];

                Array.Copy(bytes, startIndex.Value, realBytes, 0, length.Value);
                return realBytes;
            }
        }
    }
}