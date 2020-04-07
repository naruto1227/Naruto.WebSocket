using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Internal
{
    /// <summary>
    /// 存在当前上下文的配置信息
    /// </summary>
    public class CurrentContext
    {
        /// <summary>
        /// 对应的key信息
        /// </summary>
        public Guid Key { get; set; }
        /// <summary>
        /// websocket客户端信息
        /// </summary>
        public WebSocketClient WebSocketClient { get; set; }

    }

    /// <summary>
    /// 存在当前上下文的配置信息
    /// </summary>
    public class CurrentContext<TService> : CurrentContext where TService : NarutoWebSocketService
    {

    }

}
