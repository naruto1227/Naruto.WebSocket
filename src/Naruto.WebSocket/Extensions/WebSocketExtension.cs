using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
namespace Naruto.WebSocket
{
    /// <summary>
    /// 张海波
    /// 2020-04-1
    /// 操作websocket的扩展
    /// </summary>
    public static class WebSocketExtension
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="webSocketMessageType">消息类型 默认为文本</param>
        /// <param name="webSocket"></param>
        public static async Task SendMessage(this System.Net.WebSockets.WebSocket webSocket, string message, WebSocketMessageType webSocketMessageType = WebSocketMessageType.Text)
        {
            //发送
            await webSocket.SendAsync(Encoding.UTF8.GetBytes(message), webSocketMessageType, true, CancellationToken.None);
        }
    }
}
