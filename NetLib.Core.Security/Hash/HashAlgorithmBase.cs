using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FrHello.NetLib.Core.Serialization;

namespace FrHello.NetLib.Core.Security.Hash
{
    /// <summary>
    /// 基础Hash算法
    /// </summary>
    internal class HashAlgorithmBase
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

            using (var hashAlgorithm = HashAlgorithm.Create(_hashAlgorithmType.ToString()))
            {
                if (hashAlgorithm == null)
                {
                    throw new InvalidOperationException($"{_hashAlgorithmType.ToString()} not exist.");
                }

                var datas = hashAlgorithm.ComputeHash(
                    GlobalSerializationOptions.DefaultEncoding.GetBytes(inputString));

                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (var d in datas)
                {
                    sBuilder.Append(d.ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public string ComputeHash(Stream stream, IProgress<double> progress = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            //stream.ToBytes()

            return null;
        }

        public string ComputeHash(byte[] bytes, IProgress<double> progress = null)
        {
            return null;
        }

        public string ComputeHash(FileInfo file, IProgress<double> progress = null)
        {
            return null;
        }

        public string ComputeHashFast(string str, IProgress<double> progress = null)
        {
            //var md5 = new MD5CryptoServiceProvider();
            //string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            //t2 = t2.Replace("-", "");
            //return t2;

            return null;
        }

        public string ComputeHashFast(Stream stream, IProgress<double> progress = null)
        {
            return null;
        }

        public string ComputeHashFast(byte[] bytes, IProgress<double> progress = null)
        {
            return null;
        }

        public string ComputeHashFast(FileInfo file, IProgress<double> progress = null)
        {
            return null;
        }
    }
}