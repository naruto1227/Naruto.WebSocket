using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
        public static T ToDeserialize<T>(this string source)
        {
            if (source == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(source);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object ToDeserialize(this string source, Type type)
        {
            if (source == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject(source, type);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson(this object source)
        {
            if (source == null)
            {
                return default;
            }
            return JsonConvert.SerializeObject(source);
        }
    }
}
