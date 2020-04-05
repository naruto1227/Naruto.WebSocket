using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 客户端发送消息对象
    /// </summary>
    public interface IClientSend
    {
        /// <summary>
        /// 给指定连接发送消息
        /// </summary>
        ICurrentClient Current { get; }
        /// <summary>
        /// 给所有用户发送消息
        /// </summary>
        IAllClient All { get;}
        /// <summary>
        /// 给群发送消息
        /// </summary>

        IGroupClient Group { get; }
        /// <summary>
        /// 给其它用户发送消息
        /// </summary>
        IOtherClient Other { get;  }
    }
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 客户端发送消息对象
    /// </summary>
    public interface IClientSend<TService> : IClientSend where TService : NarutoWebSocketService
    {

    }
}
