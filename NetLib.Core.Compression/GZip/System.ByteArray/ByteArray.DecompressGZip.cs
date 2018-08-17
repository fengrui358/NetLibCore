using System;
using System.IO;
using System.IO.Compression;
using System.Text;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Compression
{
    /// <summary>
    /// 字节压缩GZip辅助方法
    /// </summary>
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     A byte[] extension method that decompress the byte array gzip to string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The byte array gzip to string.</returns>
        public static string DecompressGZip(this byte[] @this)
        {
            return DecompressGZip(@this, GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     A byte[] extension method that decompress the byte array gzip to string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The byte array gzip to string.</returns>
        public static string DecompressGZip(this byte[] @this, Encoding encoding)
        {
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

            const int bufferSize = 1024;
            using (var memoryStream = new MemoryStream(@this))
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    // Memory stream for storing the decompressed bytes
                    using (var outStream = new MemoryStream())
                    {
                        var buffer = new byte[bufferSize];
                        int totalBytes = 0;
                        int readBytes;
                        while ((readBytes = zipStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            outStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }
                        return encoding.GetString(outStream.GetBuffer(), 0, totalBytes);
                    }
                }
            }
        }
    }
}
