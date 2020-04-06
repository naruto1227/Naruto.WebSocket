using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-02
    /// 事件总线 定义发布订阅接口 
    /// 用来实现websocket的横向扩展
    /// 单例模式
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <returns></returns>
        Task SubscribeMessageAsync();

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task PublishAsync(string key, string data);

    }
}
