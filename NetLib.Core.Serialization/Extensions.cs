using System;
using System.IO;
using System.Text;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// 转换的一些辅助方法
    /// </summary>
    public static partial class Extensions
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
        /// <returns>字符串</returns>
        public static string ToStringEasy(this byte[] bytes)
        {
            return ToStringEasy(bytes, GlobalSerializationOptions.DefaultEncoding);
        }

        /// <summary>
        /// 字节数组转换为字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this byte[] bytes, Encoding encoding)
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

            return bytes == null ? null : encoding.GetString(bytes);
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
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this byte[] bytes,
            Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
        {
            return bytes == null ? null : Convert.ToBase64String(bytes, base64FormattingOptions);
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
                throw new ArgumentNullException(nameof(encoding));
            }

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 流转字节数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            var position = stream.Position;
            if (position != 0L)
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }

            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(position, SeekOrigin.Begin);

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
        /// <returns>流</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return bytes == null ? null : new MemoryStream(bytes);
        }
    }
}
