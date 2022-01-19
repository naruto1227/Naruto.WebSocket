using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Storage
{
    /// <summary>
    /// 存放websocket客户端集合
    /// </summary>
    public sealed class InMemoryWebSocketClientStorage<TService> : IWebSocketClientStorage<TService> where TService : NarutoWebSocketService
    {
        private static readonly ConcurrentDictionary<Guid, WebSocketClient> webSocketClients = new ConcurrentDictionary<Guid, WebSocketClient>();

        private readonly ILogger logger;

        public InMemoryWebSocketClientStorage(ILogger<InMemoryWebSocketClientStorage<TService>> _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// 添加一个新的客户端
        /// </summary>
        public ValueTask AddAsync(Guid key, WebSocketClient webSocketClient)
        {
            if (key == default)
            {
                return new ValueTask();
            }
            webSocketClients.TryAdd(key, webSocketClient);
            return new ValueTask();
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        public ValueTask RemoveAsync(Guid key)
        {
            webSocketClients.TryRemove(key, out var webSocketClient);
            webSocketClient.WebSocket?.Abort();
            webSocketClient.WebSocket?.Dispose();
            return new ValueTask();
        }
        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<WebSocketClient> GetAsync(Guid key)
        {
            webSocketClients.TryGetValue(key, out var socket);
            return Task.FromResult(socket);
        }
        /// <summary>
        /// 通过连接Id 获取websocket客户端
        /// </summary>
        /// <returns></returns>
        public Task<List<System.Net.WebSockets.WebSocket>> GetByConnectionIdAsync(string connectionId)
        {
            if (connectionId == null)
            {
                return default;
            }
            return Task.FromResult(webSocketClients.Where(a => a.Value.ConnectionId == connectionId).Select(a => a.Value.WebSocket).ToList());
        }
        /// <summary>
        /// 通过连接Id 获取websocket客户端
        /// </summary>
        /// <returns></returns>
        public Task<List<System.Net.WebSockets.WebSocket>> GetByConnectionIdAsync(List<string> connectionId)
        {
            if (connectionId == null || connectionId.Count() <= 0)
            {
                return default;
            }
            return Task.FromResult(webSocketClients.Where(a => connectionId.Contains(a.Value.ConnectionId)).Select(a => a.Value.WebSocket).ToList());
        }
        /// <summary>
        /// 获取所有在线的连接
        /// </summary>
        /// <returns></returns>
        public Task<List<System.Net.WebSockets.WebSocket>> GetAllAsync()
        {
            return Task.FromResult(webSocketClients.Select(a => a.Value.WebSocket).ToList());
        }


        /// <summary>
        /// 获取除了指定连接Id的其它websocket客户端
        /// </summary>
        /// <returns></returns>
        public Task<List<System.Net.WebSockets.WebSocket>> ExceptConnectionIdAsync(string connectionId)
        {
            if (connectionId == null || connectionId.Count() <= 0)
            {
                return GetAllAsync();
            }
            return Task.FromResult(webSocketClients.Where(a => a.Value.ConnectionId != connectionId).Select(a => a.Value.WebSocket).ToList());
        }

        /// <summary>
        /// 获取除了指定连接Id的其它websocket客户端
        /// </summary>
        /// <returns></returns>
        public Task<List<System.Net.WebSockets.WebSocket>> ExceptConnectionIdAsync(List<string> connectionId)
        {
            if (connectionId == null || connectionId.Count() <= 0)
            {
                return GetAllAsync();
            }
            return Task.FromResult(webSocketClients.Where(a => !connectionId.Contains(a.Value.ConnectionId)).Select(a => a.Value.WebSocket).ToList());
        }

        public void Dispose()
        {
            if (webSocketClients != null)
            {
                logger.LogInformation("开始释放socket资源");
                foreach (var item in webSocketClients)
                {
                    item.Value.WebSocket?.Abort();
                    item.Value.WebSocket?.Dispose();
                }
                webSocketClients.Clear();
            }
        }
    }
}
