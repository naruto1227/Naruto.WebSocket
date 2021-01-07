using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// 发送消息的实体模型
    /// </summary>
    public class SendMessageModel : MessageBase
    {
        /// <summary>
        /// 消息的具体内容
        /// </summary>
        public object message { get; set; }
    }
}
