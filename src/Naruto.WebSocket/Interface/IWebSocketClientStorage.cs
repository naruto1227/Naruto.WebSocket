using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 存储每一个websocketclient的集合数据
    /// </summary>
    public interface IWebSocketClientStorage
    {
        /// <summary>
        /// 添加一个新的客户端
        /// </summary>
        /// <param name="key"></param>
        /// <param name="webSocketClient"></param>
        /// <returns></returns>
        Task AddAsync(Guid key, WebSocketClient webSocketClient);
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(Guid key);
        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<WebSocketClient> GetAsync(Guid key);

        /// <summary>
        /// 通过连接Id 获取websocket客户端
        /// </summary>
        /// <returns></returns>
        Task<List<System.Net.WebSockets.WebSocket>> GetByConnectionIdAsync(string connectionId);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 存储每一个websocketclient的集合数据
    /// </summary>
    public interface IWebSocketClientStorage<TService> : IWebSocketClientStorage where TService : NarutoWebSocketService
    {

    }
}
