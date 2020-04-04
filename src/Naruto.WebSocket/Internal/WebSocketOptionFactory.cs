using Microsoft.Extensions.DependencyInjection;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal
{
    public class WebSocketOptionFactory : IWebSocketOptionFactory
    {
        /// <summary>
        /// 存储配置
        /// </summary>
        private readonly ConcurrentDictionary<string, WebSocketOption> cache;

        private readonly IServiceProvider serviceProvider;

        public WebSocketOptionFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
            cache = new ConcurrentDictionary<string, WebSocketOption>();
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<WebSocketOption> GetAsync(string key)
        {
            if (cache.TryGetValue(key, out var webSocketOption))
            {
                return Task.FromResult(webSocketOption);
            }

            var option = serviceProvider.GetService(MergeNamedType.Get(key)) as WebSocketOption;

            if (option == null)
                return default;
            cache.TryAdd(key, option);
            return Task.FromResult(option);
        }
    }
}
