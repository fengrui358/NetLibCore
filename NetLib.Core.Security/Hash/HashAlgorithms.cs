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
        /// MD5 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm MD5 { get; } = new HashAlgorithmBase(HashAlgorithmType.MD5);

        /// <summary>
        /// SHA1 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm SHA1 { get; } = new HashAlgorithmBase(HashAlgorithmType.SHA1);

        /// <summary>
        /// SHA256 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm SHA256 { get; } = new HashAlgorithmBase(HashAlgorithmType.SHA256);

        /// <summary>
        /// SHA384 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm SHA384 { get; } = new HashAlgorithmBase(HashAlgorithmType.SHA384);

        /// <summary>
        /// SHA512 Hash算法
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IHashAlgorithm SHA512 { get; } = new HashAlgorithmBase(HashAlgorithmType.SHA512);
    }
}