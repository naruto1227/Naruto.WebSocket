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
    public class InMemoryWebSocketClientStorage<TService> : IWebSocketClientStorage<TService> where TService : NarutoWebSocketService
    {
        private ConcurrentDictionary<Guid, WebSocketClient> webSocketClients = new ConcurrentDictionary<Guid, WebSocketClient>();

        /// <summary>
        /// 添加一个新的客户端
        /// </summary>
        public Task AddAsync(Guid key, WebSocketClient webSocketClient)
        {
            webSocketClients.TryAdd(key, webSocketClient);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        public Task RemoveAsync(Guid key)
        {
            webSocketClients.TryRemove(key, out var webSocketClient);
            webSocketClient.WebSocket?.Abort();
            webSocketClient.WebSocket?.Dispose();
            return Task.CompletedTask;
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
            return Task.FromResult(webSocketClients.Where(a => a.Value.ConnectionId == connectionId).Select(a => a.Value.WebSocket).ToList());
        }
    }
}
