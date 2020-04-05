using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    /// <summary>
    /// 发送给指定连接外的其它用户
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class InMemoryOtherClient<TService> : IOtherClient<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// webscoket 客户端存储
        /// </summary>
        private readonly IWebSocketClientStorage socketClientStorage;

        public InMemoryOtherClient(IWebSocketClientStorage<TService> _socketClientStorage)
        {
            socketClientStorage = _socketClientStorage;
        }


        public async Task SendAsync(string connectionId, string msg)
        {
            var webSockets = await socketClientStorage.ExceptConnectionIdAsync(connectionId);
            if (webSockets != null && webSockets.Count() > 0)
            {
                Parallel.ForEach(webSockets,async item=> await item.SendMessage(msg));
            }
        }

        public async Task SendAsync(List<string> connectionId, string msg)
        {
            var webSockets = await socketClientStorage.ExceptConnectionIdAsync(connectionId);
            if (webSockets != null && webSockets.Count() > 0)
            {
                Parallel.ForEach(webSockets, async item => await item.SendMessage(msg));
            }
        }
    }
}
