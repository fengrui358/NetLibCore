namespace FrHello.NetLib.Core.Security.Hash
{
    /// <summary>
    /// 可使用的Hash算法类型
    /// </summary>
    internal enum HashAlgorithmType
    {
        /// <summary>
        /// Md5
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Md5 = 32771, // 0x00008003,

        /// <summary>
        /// Sha1
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Sha1 = 32772, // 0x00008004

        /// <summary>
        /// Sha256
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Sha256 = 32780, // 0x0000800C

        /// <summary>
        /// Sha384
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Sha384 = 32781, // 0x0000800D

        /// <summary>
        /// SHA512
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Sha512 = 32782, // 0x0000800E
    }
}
