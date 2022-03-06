using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Extensions
{
    /// <summary>
    /// 基础工具的扩展
    /// </summary>
    public static class UtilExtension
    {
        /// <summary>
        /// 将bute转换成 字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUtf8String(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }
        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static async Task<T> ToDeserializeAsync<T>(this string source)
        {
            if (source == null)
            {
                return default;
            }

            using var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(source));
            memoryStream.Position = 0;
            return await JsonSerializer.DeserializeAsync<T>(memoryStream);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static async Task<T> ToDeserializeAsync<T>(this byte[] source)
        {
            if (source == null)
            {
                return default;
            }

            using var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(source);
            memoryStream.Position = 0;
            return await JsonSerializer.DeserializeAsync<T>(memoryStream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static async Task<object> ToDeserializeAsync(this string source, Type type)
        {
            if (source == null)
            {
                return default;
            }

            var options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); // 中文序列化处理
            using var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(source));
            memoryStream.Position = 0;
            return await JsonSerializer.DeserializeAsync(memoryStream, type,options);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static async Task<string> ToJsonAsync(this object source)
        {
            if (source == null)
            {
                return default;
            }

            using var memoryStream = new MemoryStream();
            var options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); // 中文序列化处理
            await JsonSerializer.SerializeAsync(memoryStream, source,options);
            memoryStream.Position = 0;
            using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
            return await streamReader.ReadToEndAsync();
        }
    }
}
