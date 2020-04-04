using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.WebSocket.Internal.Cache
{
    /// <summary>
    /// 路径的缓存
    /// </summary>
    public class PathCache
    {
        private static List<PathString> pathStrings = new List<PathString>();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="pathString"></param>
        public static void Add(PathString pathString)
        {
            pathStrings.Add(pathString);
        }
        /// <summary>
        /// 匹配路由
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static bool Match(PathString pathString)
        {
            return pathStrings.Where(a => a == pathString).Any();
        }
    }
}
