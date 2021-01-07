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
    ///  用于集群环境中订阅发送
    /// </summary>
    public interface IClusterCurrentClient
    {
        /// <summary>
        /// 给单人发送消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="connectionId">连接id信息</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageAsync(string connectionId, string execAction, object msg);
        /// <summary>
        /// 给多人发送消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="connectionIds">连接id 信息</param>
        /// <param name="msg">消息模型</param>
        /// <returns></returns>

        Task SendMessageAsync(List<string> connectionIds, string execAction, object msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 操作连接
    ///  用于集群环境中订阅发送
    /// </summary>
    public interface IClusterCurrentClient<TService>: IClusterCurrentClient where TService :NarutoWebSocketService
    {
    }
}
