namespace FrHello.NetLib.Core.Security.Hash
{
    /// <summary>
    /// Hash算法集合
    /// </summary>
    public class HashAlgorithms
    {
        /// <summary>
        /// 构造
        /// </summary>
        internal HashAlgorithms()
        {
        }

        /// <summary>
        /// Md5 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm Md5 { get; } = new HashAlgorithmBase(HashAlgorithmType.Md5);

        /// <summary>
        /// Sha1 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm Sha1 { get; } = new HashAlgorithmBase(HashAlgorithmType.Sha1);

        /// <summary>
        /// Sha256 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm Sha256 { get; } = new HashAlgorithmBase(HashAlgorithmType.Sha256);

        /// <summary>
        /// Sha384 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm Sha384 { get; } = new HashAlgorithmBase(HashAlgorithmType.Sha384);

        /// <summary>
        /// Sha512 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm Sha512 { get; } = new HashAlgorithmBase(HashAlgorithmType.Sha512);
    }
}