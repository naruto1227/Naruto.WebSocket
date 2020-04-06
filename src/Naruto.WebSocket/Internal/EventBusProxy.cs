using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Naruto.WebSocket.Internal.Cache;

namespace Naruto.WebSocket.Internal
{
    /// <summary>
    /// 事件总线的代理对象
    /// </summary>
    public class EventBusProxy : IEventBusProxy
    {
        private readonly IServiceProvider serviceProvider;

        public EventBusProxy(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task PublishAsync(SubscribeMessage message)
        {
            //获取总线服务
            var eventBus = serviceProvider.GetService<IEventBus>();
            if (eventBus == null)
            {
                return;
            }
            //定义一个数据的缓存key
            var cacheKey = Guid.NewGuid().ToString();
            //消息消息存储服务
            var messageStorage = serviceProvider.GetRequiredService<ISubscribeMessageStorage>();
            //存储消息
            await messageStorage.StoreAsync(cacheKey, message);
            //发布消息
            await eventBus.PublishAsync(GlobalCache.ClusterSendMsgKey, cacheKey);
        }
    }
}
