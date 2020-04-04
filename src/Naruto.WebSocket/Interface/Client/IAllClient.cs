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
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendAsync(string msg);
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
