using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    public class MyService : NarutoWebSocketService, IDisposable
    {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> _logger)
        {
            logger = _logger;
        }
        public override async Task OnConnectionBeginAsync(WebSocketClient client)
        {
            await base.OnConnectionBeginAsync(client);
           // logger.LogInformation($"{ConnectionId}:成功连接");
        }

        public override Task OnDisConnectionAsync(WebSocketClient client)
        {
            return base.OnDisConnectionAsync(client);
        }
        public void Dispose()
        {

        }

        public async Task send(SendMsg sendMsg)
        {
            //logger.LogInformation($"{ConnectionId}:发送群组消息");
            await Client.Group.SendAsync(sendMsg.roomId, sendMsg.msg);
        }
        public async Task sendAll(SendMsg sendMsg)
        {
           // logger.LogInformation($"{ConnectionId}:发送所有人消息");
            await Client.All.SendAsync(sendMsg.msg);
        }
        public async Task join(SendMsg sendMsg)
        {
            await Group.AddAsync(sendMsg.roomId, ConnectionId);
        }
        public async Task leave(SendMsg sendMsg)
        {
            await Group.RemoveAsync(sendMsg.roomId, ConnectionId);
        }

        public async Task other(SendMsg sendMsg)
        {
            logger.LogInformation($"{ConnectionId}:发送指定连接的之外的其它用户消息");
            await Client.Other.SendAsync(sendMsg.connectionId, sendMsg.msg);
        }
        private async Task send2()
        {


        }
    }
}
