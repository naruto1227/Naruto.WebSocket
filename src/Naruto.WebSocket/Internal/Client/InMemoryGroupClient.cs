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
    /// 群组成员发送消息
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class InMemoryGroupClient<TService> : IGroupClient<TService> where TService : NarutoWebSocketService
    {
        private readonly IWebSocketClientStorage socketClientStorage;

        private readonly IGroupStorage groupStorage;


        public InMemoryGroupClient(IWebSocketClientStorage<TService> _socketClientStorage, IGroupStorage<TService> _groupStorage)
        {
            socketClientStorage = _socketClientStorage;
            groupStorage = _groupStorage;
        }


        public async Task SendAsync(string groupId, string msg)
        {
            //获取所有的连接
            var connections = await groupStorage.GetAsync(groupId);
            if (connections == null || connections.Count() <= 0)
            {
                return;
            }
            //获取所有的socket客户端
            var webSockets = await socketClientStorage.GetByConnectionIdAsync(connections);
            if (webSockets != null && webSockets.Count() > 0)
            {
                Parallel.ForEach(webSockets, async item =>
                {
                    await item.SendMessage(msg);
                });
            }
        }

    }
}
