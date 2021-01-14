using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object
{
    
    /// <summary>
    /// 消息的实体模型
    /// </summary>
    public class WebSocketMessageModel : MessageBase
    {
        
        /// <summary>
        /// 消息的具体内容
        /// </summary>
        public object message { get; set; }
    }
}
