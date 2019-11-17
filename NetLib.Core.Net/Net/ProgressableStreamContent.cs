using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace FrHello.NetLib.Core.Net.Net
{
    /// <summary>
    /// ProgressableStreamContent
    /// </summary>
    public class ProgressableStreamContent : HttpContent
    {
        /// <summary>
        /// Lets keep buffer of 20kb
        /// </summary>
        private const int DefaultBufferSize = 5 * 4096;

        private readonly HttpContent _content;

        private readonly int _bufferSize;

        //private bool contentConsumed;
        private readonly Action<long, long> _progress;

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="content"></param>
        /// <param name="progress"></param>
        public ProgressableStreamContent(HttpContent content, Action<long, long> progress) : this(content,
            DefaultBufferSize, progress)
        {
        }

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="content"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progress"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProgressableStreamContent(HttpContent content, int bufferSize, Action<long, long> progress)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            _content = content ?? throw new ArgumentNullException(nameof(content));
            _bufferSize = bufferSize;
            _progress = progress;

            foreach (var h in content.Headers)
            {
                Headers.Add(h.Key, h.Value);
            }
        }

        /// <summary>
        /// SerializeToStreamAsync
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                var buffer = new byte[_bufferSize];
                TryComputeLength(out var size);
                var uploaded = 0;

                using (var inputs = await _content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = inputs.Read(buffer, 0, buffer.Length);
                        if (length <= 0)
                        {
                            break;
                        }

                        uploaded += length;
                        _progress?.Invoke(uploaded, size);

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }

                stream.Flush();
            });
        }

        /// <summary>
        /// TryComputeLength
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override bool TryComputeLength(out long length)
        {
            length = _content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}