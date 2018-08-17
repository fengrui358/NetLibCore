using System.IO;
using System.IO.Compression;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Compression
{
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="stream">The stringToCompress to act on.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this Stream stream)
        {
            using (var zipStream = new GZipStream(stream, CompressionMode.Compress))
            {
                var bytes = new byte[stream.Length];
                zipStream.Read(bytes, 0, bytes.Length);
                zipStream.Close();
                return bytes;
            }
        }
    }
}
