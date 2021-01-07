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
        protected override async Task OnConnectionBeginAsync(WebSocketClient client)
        {
            await base.OnConnectionBeginAsync(client);
            // logger.LogInformation($"{ConnectionId}:成功连接");
        }

        protected override Task OnDisConnectionAsync(WebSocketClient client)
        {
            return base.OnDisConnectionAsync(client);
        }
        public void Dispose()
        {

        }
        protected async Task sss()
        {

        }
        public async Task send(SendMsg sendMsg)
        {
            await Reply.SendAsync("send", "回复");
            //logger.LogInformation($"{ConnectionId}:发送群组消息");
            await Client.Group.SendAsync(sendMsg.roomId, "send", sendMsg.msg);
        }
        public async Task sendAll(SendMsg sendMsg)
        {
            await Reply.SendAsync("reply", "回复");
            // logger.LogInformation($"{ConnectionId}:发送所有人消息");
            await Client.All.SendAsync("sendAll", sendMsg.msg);
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
            await Client.Other.SendAsync(sendMsg.connectionId, "other", sendMsg.msg);
        }

        public async Task current(SendMsg sendMsg)
        {
            logger.LogInformation($"{ConnectionId}:发送给指定的用户");
            await Client.Current.SendAsync(sendMsg.connectionId, "current", sendMsg.msg);
        }
        private async Task send2()
        {


        }
    }
}
