using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// 张海波
    /// 2020-04-01
    /// 消息的基类
    /// </summary>
    public class MessageBase
    {
        /// <summary>
        /// 调用的方法
        /// </summary>
        public string action { get; set; }

    }
}
