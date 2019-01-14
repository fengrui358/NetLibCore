using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FrHello.NetLib.Core.Serialization;

namespace FrHello.NetLib.Core.Security.Hash
{
    /// <summary>
    /// 基础Hash算法
    /// </summary>
    internal class HashAlgorithmBase : IHashAlgorithm
    {
        private readonly HashAlgorithmType _hashAlgorithmType;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="hashAlgorithmType">Hash算法类型</param>
        internal HashAlgorithmBase(HashAlgorithmType hashAlgorithmType)
        {
            _hashAlgorithmType = hashAlgorithmType;
        }

        /// <summary>
        /// 计算字符串的Hash值
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <returns>输出Hash值</returns>
        public string ComputeHash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                if (hashAlgorithm == null)
                {
                    throw new InvalidOperationException($"{_hashAlgorithmType.ToString()} not exist.");
                }

                var datas = hashAlgorithm.ComputeHash(
                    GlobalSerializationOptions.DefaultEncoding.GetBytes(inputString));

                return datas.ToHex();
            }
        }

        /// <summary>
        /// 计算数据流的Hash值
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>输出Hash值</returns>
        public string ComputeHash(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                if (hashAlgorithm == null)
                {
                    throw new InvalidOperationException($"{_hashAlgorithmType.ToString()} not exist.");
                }

                if (stream.Position != 0L && stream.CanSeek)
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                }

                return hashAlgorithm.ComputeHash(stream).ToHex();
            }
        }

        /// <summary>
        /// 计算字节数组的Hash值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>输出Hash值</returns>
        public string ComputeHash(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                if (hashAlgorithm == null)
                {
                    throw new InvalidOperationException($"{_hashAlgorithmType.ToString()} not exist.");
                }

                return hashAlgorithm.ComputeHash(bytes).ToHex();
            }
        }

        /// <summary>
        /// 计算文件的Hash值
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>输出Hash值</returns>
        public string ComputeHash(FileInfo file)
        {
            if (file == null || !file.Exists)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (var stream = file.OpenRead())
            {
                using (var hashAlgorithm = CreateHashAlgorithm())
                {
                    if (hashAlgorithm == null)
                    {
                        throw new InvalidOperationException($"{_hashAlgorithmType.ToString()} not exist.");
                    }

                    return hashAlgorithm.ComputeHash(stream).ToHex();
                }
            }
        }

        /// <summary>
        /// 计算数据流的Hash值(按比例截取)
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        public async Task<string> ComputeHashFast(Stream stream, IProgress<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (stream.Length > GlobalSecurityOptions.SegmentSizeForComputer)
            {
                var segmentCount = (int) Math.Ceiling(stream.Length / (double) GlobalSecurityOptions.SegmentSize);

                if (stream.Position != 0L && stream.CanSeek)
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                }

                long totalProgressLength = 0L;
                long lastSegmentSizeForComputer =
                    stream.Length - (segmentCount - 1) * GlobalSecurityOptions.SegmentSize;

                if (lastSegmentSizeForComputer > GlobalSecurityOptions.SegmentSizeForComputer)
                {
                    lastSegmentSizeForComputer = GlobalSecurityOptions.SegmentSizeForComputer;
                }

                //总进度
                long totalComputerSize = (segmentCount - 1) * GlobalSecurityOptions.SegmentSizeForComputer +
                                         lastSegmentSizeForComputer;

                using (var destinationStream = new MemoryStream())
                {
                    //最后还有一段Hash预算，预留1/segmentCount的进度
                    totalComputerSize += totalComputerSize * (1 / segmentCount);

                    for (long i = 0; i < segmentCount; i++)
                    {
                        long startIndex = i * GlobalSecurityOptions.SegmentSize;
                        long endIndex = startIndex + GlobalSecurityOptions.SegmentSizeForComputer;
                        if (endIndex > stream.Length)
                        {
                            endIndex = startIndex;
                        }

                        var bytes = new byte[endIndex - startIndex];
                        await stream.ReadAsync(bytes, 0, bytes.Length, cancellationToken);
                        stream.Seek(endIndex - stream.Position, SeekOrigin.Current);

                        await destinationStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);

                        totalProgressLength += bytes.Length;
                        progress?.Report(totalProgressLength / (double) totalComputerSize);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    var hash = ComputeHash(destinationStream);
                    progress?.Report(1);

                    return hash;
                }
            }
            else
            {
                var hash = ComputeHash(stream);
                progress?.Report(1);

                return hash;
            }
        }

        /// <summary>
        /// 计算字节数组的Hash值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        public string ComputeHashFast(byte[] bytes, IProgress<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length > GlobalSecurityOptions.SegmentSizeForComputer)
            {
                var segmentCount = (int) Math.Ceiling(bytes.Length / (double) GlobalSecurityOptions.SegmentSize);

                long totalProgressLength = 0L;
                long lastSegmentSizeForComputer =
                    bytes.Length - (segmentCount - 1) * GlobalSecurityOptions.SegmentSize;

                if (lastSegmentSizeForComputer > GlobalSecurityOptions.SegmentSizeForComputer)
                {
                    lastSegmentSizeForComputer = GlobalSecurityOptions.SegmentSizeForComputer;
                }

                //总进度
                long totalComputerSize = (segmentCount - 1) * GlobalSecurityOptions.SegmentSizeForComputer +
                                         lastSegmentSizeForComputer;

                var destinationBytes = new byte[totalComputerSize];

                //最后还有一段Hash预算，预留1/segmentCount的进度
                totalComputerSize += (int) (totalComputerSize * (1 / segmentCount));

                for (long i = 0; i < segmentCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    long startIndex = i * GlobalSecurityOptions.SegmentSize;
                    long endIndex = startIndex + GlobalSecurityOptions.SegmentSizeForComputer;
                    if (endIndex > bytes.Length)
                    {
                        endIndex = startIndex;
                    }

                    Array.Copy(bytes, startIndex, destinationBytes, i * GlobalSecurityOptions.SegmentSizeForComputer,
                        endIndex - startIndex);

                    totalProgressLength += endIndex - startIndex;
                    progress?.Report(totalProgressLength / (double) totalComputerSize);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                var hash = ComputeHash(destinationBytes);
                progress?.Report(1);

                return hash;
            }
            else
            {
                var hash = ComputeHash(bytes);
                progress?.Report(1);

                return hash;
            }
        }

        /// <summary>
        /// 计算文件的Hash值
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        public async Task<string> ComputeHashFast(FileInfo file, IProgress<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (file == null || !file.Exists)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (var stream = file.OpenRead())
            {
                return await ComputeHashFast(stream, progress, cancellationToken);
            }
        }

        /// <summary>
        /// 获得对应Hash算法
        /// </summary>
        /// <returns></returns>
        private HashAlgorithm CreateHashAlgorithm()
        {
            switch (_hashAlgorithmType)
            {
                case HashAlgorithmType.MD5:
                    return MD5.Create();
                case HashAlgorithmType.SHA1:
                    return SHA1.Create();
                case HashAlgorithmType.SHA256:
                    return SHA256.Create();
                case HashAlgorithmType.SHA384:
                    return SHA384.Create();
                case HashAlgorithmType.SHA512:
                    return SHA512.Create();
            }

            throw new InvalidOperationException();
        }
    }
}