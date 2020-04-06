using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.WebSocket.Internal.Cache
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 存储租户 对应的路由信息
    /// </summary>
    public class TenantPathCache
    {
        private static readonly Dictionary<PathString, Type> cache;
        static TenantPathCache()
        {
            cache = new Dictionary<PathString, Type>();
        }

        public static void Add(PathString pathString, Type type)
        {
            cache.TryAdd(pathString, type);
        }


        public static void Remove(PathString pathString)
        {
            cache.Remove(pathString);
        }

        public static string GetByType(Type type)
        {
            return cache.Where(a => a.Value == type).Select(a => a.Key).FirstOrDefault();
        }

        public static Type GetByKey(PathString pathString)
        {
            return cache.Where(a => a.Key == pathString).Select(a => a.Value).FirstOrDefault();
        }

        /// <summary>
        /// 匹配路由
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static bool Match(PathString pathString)
        {
            return cache.Where(a => a.Key == pathString).Any();
        }
    }
}
