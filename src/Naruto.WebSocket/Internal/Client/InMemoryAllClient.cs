using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 单机版给所有人发消息
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class InMemoryAllClient<TService> : IAllClient<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// webscoket 客户端存储
        /// </summary>
        private readonly IWebSocketClientStorage socketClientStorage;

        public InMemoryAllClient(IWebSocketClientStorage<TService> _socketClientStorage)
        {
            socketClientStorage = _socketClientStorage;
        }

        public async Task SendAsync(string msg)
        {
            //获取所有在线的用户
            var webSockets = await socketClientStorage.GetAllAsync();
            Parallel.ForEach(webSockets,async item =>
            {
                await item.SendMessage(msg);
            });
        }
    }
}
