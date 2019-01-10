using Newtonsoft.Json;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// Json转换的一些辅助方法
    /// </summary>
    public static class JsonSerializationExtensions
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json文本</param>
        /// <returns>对象</returns>
        public static T DeserializeJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>Xml字符串</returns>
        public static string SerializeJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}