using System.Text;

namespace FrHello.NetLib.Core.Compression
{
    /// <summary>
    /// 压缩的全局选项
    /// </summary>
    public static class GlobalCompressionOptions
    {
        /// <summary>
        /// 默认编码格式
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 自动填充文件后缀
        /// </summary>
        public static bool AutoFillFileSuffix { get; set; } = true;
    }
}
