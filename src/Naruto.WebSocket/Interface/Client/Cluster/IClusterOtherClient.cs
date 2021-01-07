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
        /// <param name="execAction">调用的方法</param>
        /// <param name="connectionId">连接id信息</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageAsync(string connectionId, string execAction, object msg);

        /// <summary>
        /// 给除此连接外的所有用户发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="execAction">调用的方法</param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task SendMessageAsync(List<string> connectionId, string execAction, object msg);
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
