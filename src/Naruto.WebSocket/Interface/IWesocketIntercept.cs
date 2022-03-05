using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// websocket拦截器
    /// </summary>
    public interface IWesocketIntercept
    {
        /// <summary>
        /// 上线通知
        /// </summary>
        /// <returns></returns>
        ValueTask OnLineAsync(WebSocketClient webSocketClient);
        /// <summary>
        /// 下线通知
        /// </summary>
        /// <returns></returns>
        ValueTask OffLineAsync(WebSocketClient webSocketClient);
        /// <summary>
        /// 消息接收
        /// </summary>
        /// <param name="webSocketClient"></param>
        /// <param name="webSocketMessage"></param>
        /// <returns></returns>
        ValueTask ReciveAsync(WebSocketClient webSocketClient, WebSocketMessageModel webSocketMessage);
    }
}
