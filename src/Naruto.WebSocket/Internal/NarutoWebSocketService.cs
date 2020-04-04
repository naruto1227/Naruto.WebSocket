using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal
{
    /// <summary>
    /// 张海波
    /// 2020-04-03
    /// websocket的服务抽象类 需继承此接口
    /// Scope
    /// </summary>
    public abstract class NarutoWebSocketService
    {
        /// <summary>
        /// 开始连接
        /// </summary>
        /// <returns></returns>
        public virtual Task OnConnectionBegin(WebSocketClient client)
        {
            return Task.CompletedTask;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public virtual Task OnDisConnection(WebSocketClient client)
        {
            return Task.CompletedTask;
        }
    }
}
