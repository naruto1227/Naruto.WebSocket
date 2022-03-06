using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Object;

namespace server
{
	public class DefaultWesocketIntercept: IWesocketIntercept
	{
        private readonly ILogger<DefaultWesocketIntercept> _logger;
		public DefaultWesocketIntercept(ILogger<DefaultWesocketIntercept> logger)
		{
            _logger = logger;
		}

        public ValueTask OffLineAsync(WebSocketClient webSocketClient)
        {
            _logger.LogInformation("连接下线，ConnectionId={ConnectionId}",webSocketClient.ConnectionId);
            return new ValueTask();
        }

        public ValueTask OnLineAsync(WebSocketClient webSocketClient)
        {
            _logger.LogInformation("上线通知，ConnectionId={ConnectionId}", webSocketClient.ConnectionId);
            return new ValueTask();
        }

        public ValueTask ReciveAsync(WebSocketClient webSocketClient, WebSocketMessageModel webSocketMessage)
        {
            _logger.LogInformation("消息接收，ConnectionId={ConnectionId}", webSocketClient.ConnectionId);
            return new ValueTask();
        }
    }
}

