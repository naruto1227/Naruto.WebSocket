using Naruto.WebSocket.Exceptions;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Naruto.WebSocket.Internal.Cache
{
    /// <summary>
    /// 方法的缓存
    /// </summary>
    public class MethodCache
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> methods = new ConcurrentDictionary<string, MethodInfo>();

        public static MethodInfo Get(Type service, string action)
        {
            var key = service.Name + action;
            if (methods.TryGetValue(key, out var method))
            {
                return method;
            }
            //验证当前执行的方法是否为内部方法
            if (IsInternalMethod(service, action))
            {
                //获取方法
                method = service.GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
            }
            else
            {
                //获取方法
                method = service.GetMethod(action, BindingFlags.Public | BindingFlags.Instance);
            }
            if (method == null)
            {
                throw new NotMethodException($"查找不到服务{service.Name}中的{action}方法");
            }
            methods.TryAdd(key, method);
            return method;
        }
        /// <summary>
        /// 验证当前执行的方法是否为 内部的方法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static bool IsInternalMethod(Type service, string action)
        {
            if (action == NarutoWebSocketServiceMethodEnum.OnConnectionBeginAsync.ToString() || action == NarutoWebSocketServiceMethodEnum.OnDisConnectionAsync.ToString())
            {
                return true;
            }
            return false;
        }
    }
}
