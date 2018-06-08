using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// Xml转换的一些辅助方法
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(this string xml)
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
        /// <param name="omitXmlDeclaration">忽略申明</param>
        /// <returns>Xml字符串</returns>
        public static string SerializeXml<T>(this T obj, bool omitXmlDeclaration = false)
        {
            var sb = new StringBuilder();
            using (var xw = XmlWriter.Create(sb, new XmlWriterSettings()
            {
                Encoding = GlobalSerializationOptions.DefaultEncoding,
                OmitXmlDeclaration = omitXmlDeclaration,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = GlobalSerializationOptions.XmlIndentFormat
            }))
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(xw, obj);
            }

            return sb.ToString();
        }
    }
}
