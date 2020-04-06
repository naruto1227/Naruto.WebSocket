using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 事件总线的代理接口
    /// 隔离委托类 扩展新功能
    /// </summary>
    public interface IEventBusProxy
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task PublishAsync(SubscribeMessage message);
    }
}
