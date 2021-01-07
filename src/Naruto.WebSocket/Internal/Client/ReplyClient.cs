using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    /// <summary>
    /// 回复消息
    /// </summary>
    public class ReplyClient<TService> : IReplyClient<TService> where TService : NarutoWebSocketService
    {
        private readonly CurrentContext currentContext;

        public ReplyClient(CurrentContext<TService> _currentContext)
        {
            currentContext = _currentContext;
        }

        public async Task SendAsync(string execAction, object msg)
        {
            await currentContext.WebSocketClient.WebSocket.SendMessage(new Object.SendMessageModel
            {
                message = msg,
                action = execAction
            });
        }
    }
}
