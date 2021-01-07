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
    /// 给指定的群发送消息
    /// </summary>
    public interface IGroupClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息的信息</param>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        Task SendAsync(string groupId, string execAction, object msg);

    }
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给指定的群发送消息
    /// </summary>
    public interface IGroupClient<TService> : IGroupClient where TService : NarutoWebSocketService
    {

    }
}
