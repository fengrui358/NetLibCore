using System;
using System.Threading;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core
{
    public static class StreamExtensions
    {
        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="source">原始流</param>
        /// <param name="destination">目标流</param>
        /// <param name="bufferSize">分段复制流的大小</param>
        /// <param name="progress">进度</param>
        /// <param name="cancellationToken">取消标示</param>
        /// <returns></returns>
        public static async Task CopyToAsync(this System.IO.Stream source, System.IO.Stream destination,
            long bufferSize = 81920, IProgress<double> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            var totalLength = (double) source.Length;
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead =
                       await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalLength / totalBytesRead);
            }
        }
    }
}