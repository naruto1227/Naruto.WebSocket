using Naruto.WebSocket.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface.Client
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给其它连接发送消息
    /// </summary>
    public interface IOtherClient
    {
        /// <summary>
        /// 给除此连接外的所有用户发送消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息的信息</param>
        /// <returns></returns>
        Task SendAsync(string connectionId, string execAction, object msg);

        /// <summary>
        /// 给除此连接外的所有用户发送消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息的信息</param>
        /// <returns></returns>
        Task SendAsync(List<string> connectionId, string execAction, object msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给其它连接发送消息
    /// </summary>
    public interface IOtherClient<TService> : IOtherClient where TService : NarutoWebSocketService
    {

    }
}
