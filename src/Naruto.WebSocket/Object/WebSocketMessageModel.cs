using System;
using System.Collections.Generic;
using MessagePack;
using System.Text;

namespace Naruto.WebSocket.Object
{
    
    /// <summary>
    /// 消息的实体模型
    /// </summary>
    public class WebSocketMessageModel : MessageBase
    {
        [Key(1)]
        /// <summary>
        /// 消息的具体内容
        /// </summary>
        public object message { get; set; }
    }
}
