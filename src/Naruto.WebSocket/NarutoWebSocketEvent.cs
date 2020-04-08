using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket
{
    /// <summary>
    /// 张海波
    /// </summary>
    public class NarutoWebSocketEvent
    {
        /// <summary>
        /// 上线的事件
        /// </summary>
        internal static Action<WebSocketClient> OnLineEvent;

        /// <summary>
        /// 下线的事件
        /// </summary>
        internal static Action<WebSocketClient> OffLineEvent;
        /// <summary>
        /// 注册上线通知的事件
        /// </summary>
        /// <param name="action"></param>
        public static void RegisterOnLine(Action<WebSocketClient> action)
        {
            OnLineEvent+= action;
        }
        /// <summary>
        /// 注册下线通知的事件
        /// </summary>
        /// <param name="action"></param>
        public static void RegisterOffLine(Action<WebSocketClient> action)
        {
            OffLineEvent += action;
        }
    }
}
