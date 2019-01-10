using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// Xml转换的一些辅助方法
    /// </summary>
    public static class XmlSerializationExtensions
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">xml文本</param>
        /// <returns>对象</returns>
        public static T DeserializeXml<T>(this string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xml))
            {
                var obj = (T)xs.Deserialize(sr);
                return obj;
            }
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">是否忽略文件头顶部的申明</param>
        /// <returns>Xml字符串</returns>
        public static string SerializeXml<T>(this T obj, bool omitXmlDeclaration = false)
        {
            var textWriter = new CustomTextWriter(GlobalSerializationOptions.DefaultEncoding);
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = GlobalSerializationOptions.DefaultEncoding,
                OmitXmlDeclaration = omitXmlDeclaration,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = GlobalSerializationOptions.XmlIndentFormat
            };

            using (var xw = XmlWriter.Create(textWriter, xmlWriterSettings))
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(xw, obj);
            }

            return textWriter.ToString();
        }

        /// <summary>
        /// 用于Xml序列化的文本编码输出控制的过渡类
        /// </summary>
        private class CustomTextWriter : StringWriter
        {
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="encoding">文本输出的编码</param>
            internal CustomTextWriter(Encoding encoding)
            {
                Encoding = encoding;
            }

            /// <summary>
            /// 文本输出的编码
            /// </summary>
            public override Encoding Encoding { get; }
        }
    }
}
