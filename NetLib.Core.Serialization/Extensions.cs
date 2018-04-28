using System.IO;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// 转换的一些辅助方法
    /// </summary>
    public static class Extensions
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
            return bytes == null ? null : "";
        }

        /// <summary>
        /// 流转换为字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字符串</returns>
        public static string ToStringEasy(this Stream stream)
        {
            return "";
        }

        /// <summary>
        /// 字符串转换为Base64字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string str)
        {
            return "";
        }

        /// <summary>
        /// 字符数组转Base64字符串
        /// </summary>
        /// <param name="chars">字符数组</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this char[] chars)
        {
            if (chars == null)
            {
                return null;
            }

            return new string(chars);
        }

        /// <summary>
        /// 字节数组转换为Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            return "";
        }

        /// <summary>
        /// 流转换为Base64字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this Stream stream)
        {
            return "";
        }

        /// <summary>
        /// 字符串转换为字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string str)
        {
            return null;
        }

        /// <summary>
        /// 字符数组转字节数组
        /// </summary>
        /// <param name="chars">字符数组</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this char[] chars)
        {
            return null;
        }

        /// <summary>
        /// 流转字节数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            return null;
        }

        /// <summary>
        /// 字符串转流
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>流</returns>
        public static Stream ToStream(this string str)
        {
            return null;
        }

        /// <summary>
        /// 字符数组转流
        /// </summary>
        /// <param name="chars">字符数组</param>
        /// <returns>流</returns>
        public static Stream ToStream(this char[] chars)
        {
            return null;
        }

        /// <summary>
        /// 字节数组转流
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>流</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return null;
        }
    }
}
