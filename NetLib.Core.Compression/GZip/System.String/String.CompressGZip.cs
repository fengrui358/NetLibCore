using System;
using System.IO;
using System.IO.Compression;
using System.Text;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Compression
{
    /// <summary>
    /// String压缩Gizp辅助方法
    /// </summary>
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this)
        {
            return CompressGZip(@this, GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this, Encoding encoding)
        {
            if (@this == null)
            {
                @this = string.Empty;
            }

            if (encoding == null)
            {
                if (GlobalCompressionOptions.DefaultEncoding != null)
                {
                    encoding = GlobalCompressionOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(encoding));
                }
            }
            
            var stringAsBytes = encoding.GetBytes(@this);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                    zipStream.Close();
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
