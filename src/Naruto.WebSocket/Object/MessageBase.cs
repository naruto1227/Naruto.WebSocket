using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace Naruto.WebSocket.Object
{
    [MessagePack.MessagePackObject]
    /// <summary>
    /// 张海波
    /// 2020-04-01
    /// 消息的基类
    /// </summary>
    public class MessageBase
    {
        [Key(0)]
        /// <summary>
        /// 调用的方法
        /// </summary>
        public string action { get; set; }

    }
}
