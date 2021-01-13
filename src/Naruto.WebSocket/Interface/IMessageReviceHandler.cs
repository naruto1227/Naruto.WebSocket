using Microsoft.AspNetCore.Http;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 消息接收处理
    /// </summary>
    public interface IMessageReviceHandler
    {
        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="currentServiceType">当前服务类型</param>
        /// <param name="messageModel">消息模型</param>
        /// <returns></returns>
        Task HandlerAsync(WebSocketClient webSocketClient, Type currentServiceType, WebSocketMessageModel messageModel);
    }
}
