using System.Text;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// 序列化的全局选项
    /// </summary>
    public static class GlobalSerializationOptions
    {
        /// <summary>
        /// 默认编码格式
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 是否缩进Xml输出，使其格式更直观好看
        /// </summary>
        public static bool XmlIndentFormat { get; set; } = true;
    }
}
