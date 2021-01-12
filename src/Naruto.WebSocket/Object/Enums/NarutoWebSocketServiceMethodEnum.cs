using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object.Enums
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 存放内部的消息方法的枚举
    /// </summary>
    public enum NarutoWebSocketServiceMethodEnum
    {
        /// <summary>
        /// 连接开始
        /// </summary>
        OnConnectionBeginAsync,
        /// <summary>
        /// 连接断开
        /// </summary>
        OnDisConnectionAsync,

        /// <summary>
        /// 心跳检查
        /// </summary>
        HeartbeatCheck
    }
}
