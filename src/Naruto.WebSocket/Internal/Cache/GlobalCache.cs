using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Internal.Cache
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 存储全局缓存
    /// </summary>
    public class GlobalCache
    {
        /// <summary>
        /// 系统的标识 
        /// 用于集群环境中
        /// </summary>
        public static string SystemIdentity = Environment.MachineName + Guid.NewGuid().ToString().Replace("-", "");

        /// <summary>
        /// 集群环境消息发送的key
        /// </summary>
        public const string ClusterSendMsgKey = "cluster_send_msg";
    }
}
