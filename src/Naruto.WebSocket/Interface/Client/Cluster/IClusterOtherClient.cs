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
    ///  用于集群环境中订阅发送
    /// </summary>
    public interface IClusterOtherClient
    {
        /// <summary>
        /// 给除此连接外的所有用户发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageAsync(string connectionId, string msg);

        /// <summary>
        /// 给除此连接外的所有用户发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageAsync(List<string> connectionId, string msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给其它连接发送消息
    ///  用于集群环境中订阅发送
    /// </summary>
    public interface IClusterOtherClient<TService> : IClusterOtherClient where TService : NarutoWebSocketService
    {

    }
}
