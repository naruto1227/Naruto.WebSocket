using Naruto.WebSocket.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface.Client
{
    /// <summary>
    /// 张海波
    /// 回复消息
    /// </summary>
    public interface IReplyClient
    {
        /// <summary>
        /// 给当前的连接回复消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息的信息</param>
        /// <returns></returns>
        Task SendAsync(string execAction, object msg);
    }

    /// <summary>
    /// 张海波
    /// 回复消息
    /// </summary>
    public interface IReplyClient<TService>: IReplyClient where TService : NarutoWebSocketService
    {
    }
}
