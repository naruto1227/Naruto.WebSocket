using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 订阅的消息实体
    /// 用于集群环境中
    /// </summary>
    public class SubscribeMessage
    {
        /// <summary>
        /// 系统标识
        /// </summary>
        public string SystemIdentity { get; set; } = GlobalCache.SystemIdentity;

        /// <summary>
        /// 租户的标识
        /// </summary>
        public string TenantIdentity { get; set; }

        /// <summary>
        /// 消息的发送类型的枚举
        /// </summary>
        public MessageSendTypeEnum SendTypeEnum { get; set; }
        /// <summary>
        /// 方法类型枚举
        /// </summary>
        public MessageSendActionTypeEnum ActionTypeEnum { get; set; } = MessageSendActionTypeEnum.Single;

        /// <summary>
        /// 方法的参数
        /// </summary>
        public ParamterEntity ParamterEntity { get; set; }

    }

    /// <summary>
    ///方法的参数实体
    /// </summary>
    public class ParamterEntity
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public SendMessageModel Message { get; set; }
        /// <summary>
        ///连接Id
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// 连接Id集合
        /// </summary>
        public List<string> ConnectionIds { get; set; }
        /// <summary>
        /// 群组Id
        /// </summary>
        public string GroupId { get; set; }
    }
}
