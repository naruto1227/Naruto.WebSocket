using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    public class InMemoryCurrentClient<TService> : ICurrentClient<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// webscoket 客户端存储
        /// </summary>
        private readonly IWebSocketClientStorage socketClientStorage;

        public InMemoryCurrentClient(IWebSocketClientStorage<TService> _socketClientStorage)
        {
            socketClientStorage = _socketClientStorage;
        }

        public async Task SendAsync(string connectionId, string msg)
        {
            var list = await socketClientStorage.GetByConnectionIdAsync(connectionId);
            foreach (var item in list)
            {
                await item.SendMessage(msg);
            }
        }

        public async Task SendAsync(List<string> connectionIds, string msg)
        {
            var webSockets = await socketClientStorage.GetByConnectionIdAsync(connectionIds);
            Parallel.ForEach(webSockets, async item =>
            {
                await item.SendMessage(msg);
            });
        }
    }
}
