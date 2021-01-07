using Naruto.WebSocket.Exceptions;
using Naruto.WebSocket.Object;
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

        private static readonly ConcurrentDictionary<string, MethodCacheInfo> methods = new ConcurrentDictionary<string, MethodCacheInfo>();

        public static MethodCacheInfo Get(Type service, string action)
        {
            //缓存key
            var key = service.Name + action;
            if (methods.TryGetValue(key, out var method))
            {
                return method;
            }

            return GetMethod(key, service, action);
        }
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="service">需要映射的服务类型</param>
        /// <param name="action">执行的方法</param>
        /// <returns></returns>
        private static MethodCacheInfo GetMethod(string key, Type service, string action)
        {
            //构建返回的结果
            var methodCacheInfo = new MethodCacheInfo();

            //验证当前执行的方法是否为内部方法
            if (IsInternalMethod(service, action))
            {
                //获取方法
                methodCacheInfo.Method = service.GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
            }
            else
            {
                //获取方法
                methodCacheInfo.Method = service.GetMethod(action, BindingFlags.Public | BindingFlags.Instance);
            }
            if (methodCacheInfo.Method == null)
            {
                throw new NotMethodException($"查找不到服务{service.Name}中的{action}方法");
            }
            //获取参数信息
            methodCacheInfo.ParameterInfos = methodCacheInfo.Method.GetParameters();
            //存储到内存
            methods.TryAdd(key, methodCacheInfo);
            return methodCacheInfo;
        }
        /// <summary>
        /// 验证当前执行的方法是否为 内部的方法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static bool IsInternalMethod(Type service, string action)
        {
            //内部方法 只能为NarutoWebSocketServiceMethodEnum 中定义的枚举值
            if (action == NarutoWebSocketServiceMethodEnum.OnConnectionBeginAsync.ToString() || action == NarutoWebSocketServiceMethodEnum.OnDisConnectionAsync.ToString())
            {
                return true;
            }
            return false;
        }
    }
}
