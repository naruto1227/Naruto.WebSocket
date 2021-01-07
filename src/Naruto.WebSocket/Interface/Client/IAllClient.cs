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
    /// 给所有客户端发送消息接口
    /// </summary>
    public interface IAllClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息的信息</param>
        /// <returns></returns>
        Task SendAsync(string execAction, object msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给所有客户端发送消息接口
    /// </summary>
    public interface IAllClient<TService> : IAllClient where TService : NarutoWebSocketService
    {

    }
}
