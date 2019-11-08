using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>Json字符串</returns>
        public static string SerializeJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json文本</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>对象</returns>
        public static async Task<T> DeserializeJsonAsync<T>(this string json, CancellationToken cancellationToken = default)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            using var memoryStream = new MemoryStream(bytes.Length);
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0L, SeekOrigin.Begin);

            return await JsonSerializer.DeserializeAsync<T>(memoryStream, null, cancellationToken);
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>Json字符串</returns>
        public static async Task<string> SerializeJsonAsync<T>(this T obj, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, obj, null, cancellationToken);
            memoryStream.Seek(0L, SeekOrigin.Begin);

            var bytes = new byte[memoryStream.Length];
            memoryStream.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}