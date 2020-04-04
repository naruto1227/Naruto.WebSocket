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
    /// 操作连接
    /// </summary>
    public interface ICurrentClient
    {
        /// <summary>
        /// 给单人发送消息
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendAsync(string connectionId, string msg);
        /// <summary>
        /// 给多人发送消息
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        Task SendAsync(List<string> connectionIds, string msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 操作连接
    /// </summary>
    public interface ICurrentClient<TService>: ICurrentClient where TService :NarutoWebSocketService
    {
    }
}
