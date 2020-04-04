using Naruto.WebSocket.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Naruto.WebSocket.Internal
{
    /// <summary>
    /// 存放websocket客户端集合
    /// </summary>
    public class WebSocketClientCollection
    {
        private static ConcurrentDictionary<Guid, WebSocketClient> webSocketClients = new ConcurrentDictionary<Guid, WebSocketClient>();

        /// <summary>
        /// 添加一个新的客户端
        /// </summary>
        public static void Add(Guid key, WebSocketClient webSocketClient)
        {
            webSocketClients.TryAdd(key, webSocketClient);
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        public static void Remove(Guid key)
        {
            webSocketClients.TryRemove(key, out var webSocketClient);
            webSocketClient.WebSocket?.Abort();
            webSocketClient.WebSocket?.Dispose();
        }
        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static WebSocketClient Get(Guid key)
        {
            webSocketClients.TryGetValue(key, out var socket);
            return socket;
        }
        /// <summary>
        /// 通过连接Id 获取websocket客户端
        /// </summary>
        /// <returns></returns>
        public static System.Net.WebSockets.WebSocket GetByConnectionId(string connectionId)
        {
            return webSocketClients.Where(a => a.Value.ConnectionId == connectionId).Select(a => a.Value.WebSocket).FirstOrDefault();
        }
    }
}
