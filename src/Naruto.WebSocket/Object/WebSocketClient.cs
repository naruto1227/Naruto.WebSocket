using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// 张海波
    /// 2020-04-1
    /// websocket的客户端类
    /// </summary>
    public class WebSocketClient
    {
        /// <summary>
        /// 连接Id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// websocket客户端
        /// </summary>
        public System.Net.WebSockets.WebSocket WebSocket { get; set; }

    }
}
