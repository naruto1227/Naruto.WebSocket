using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Extensions;
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
    public sealed class ReplyClient<TService> : IReplyClient<TService> where TService : NarutoWebSocketService
    {
        private readonly CurrentContext currentContext;

        private readonly ILogger logger;

        public ReplyClient(CurrentContext<TService> _currentContext, ILogger<ReplyClient<TService>> _logger)
        {
            currentContext = _currentContext;
            logger = _logger;
        }

        public async Task SendAsync(string execAction, object msg)
        {
            logger.LogTrace("回复当前用户,execAction={execAction},connectionId={connectionId},msg={msg}", execAction, currentContext.WebSocketClient.ConnectionId, msg.ToJson());
            await currentContext.WebSocketClient.WebSocket.SendMessage(new Object.WebSocketMessageModel
            {
                message = msg,
                action = execAction
            });
        }
    }
}
