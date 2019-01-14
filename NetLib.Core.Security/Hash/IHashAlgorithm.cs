using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core.Security.Hash
{
    /// <summary>
    /// Hash算法
    /// </summary>
    public interface IHashAlgorithm
    {
        /// <summary>
        /// 计算字符串的Hash值
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        string ComputeHash(string inputString);

        /// <summary>
        /// 计算字符串的Hash值
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>输出Hash值</returns>
        string ComputeHash(Stream stream);

        /// <summary>
        /// 计算字符串的Hash值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>输出Hash值</returns>
        string ComputeHash(byte[] bytes);

        /// <summary>
        /// 计算文件的Hash值
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>输出Hash值</returns>
        string ComputeHash(FileInfo file);

        /// <summary>
        /// 计算数据流的Hash值(按比例截取)
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        Task<string> ComputeHashFast(Stream stream, IProgress<double> progress = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 计算字节数组的Hash值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        string ComputeHashFast(byte[] bytes, IProgress<double> progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 计算文件的Hash值
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="progress">进度通知</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>输出Hash值</returns>
        Task<string> ComputeHashFast(FileInfo file, IProgress<double> progress = null,
            CancellationToken cancellationToken = default);
    }
}
