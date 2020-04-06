using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object.Enums
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 消息的发送类型
    /// </summary>
    public enum MessageSendTypeEnum
    {
        /// <summary>
        /// 指定连接 
        /// </summary>
        Current,
        /// <summary>
        /// 所有人
        /// </summary>
        All,
        /// <summary>
        /// 除了连接外的所有人
        /// </summary>
        Other,
        /// <summary>
        /// 群组
        /// </summary>
        Group
    }


    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 消息的发送方法枚举
    /// </summary>
    public enum MessageSendActionTypeEnum
    {
        /// <summary>
        /// 单连接 
        /// </summary>
        Single,
        /// <summary>
        /// 多连接
        /// </summary>
        Many
    }
}
