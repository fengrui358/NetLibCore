using System.Text;

namespace FrHello.NetLib.Core.Serialization
{
    public static class GlobalSerializationOptions
    {
        /// <summary>
        /// 默认编码格式
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;
    }
}
