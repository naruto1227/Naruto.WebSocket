using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    /// <summary>
    /// 群组成员发送消息
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class InMemoryGroupClient<TService> : IGroupClient<TService>, IClusterGroupClient<TService> where TService : NarutoWebSocketService
    {
        private readonly IWebSocketClientStorage socketClientStorage;

        private readonly IGroupStorage groupStorage;

        /// <summary>
        /// 定义一个当前服务对应的租户请求path字段
        /// </summary>
        private readonly string RequestPath;
        /// <summary>
        /// 事件总线代理对象
        /// </summary>
        private readonly IEventBusProxy eventBusProxy;
        public InMemoryGroupClient(IWebSocketClientStorage<TService> _socketClientStorage, IGroupStorage<TService> _groupStorage, IEventBusProxy _eventBusProxy)
        {
            socketClientStorage = _socketClientStorage;
            groupStorage = _groupStorage;
            eventBusProxy = _eventBusProxy;
            RequestPath = TenantPathCache.GetByType(typeof(TService));
        }


        public async Task SendAsync(string groupId, string msg)
        {
            await SendMessageAsync(groupId, msg);

            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.Group,
                ParamterEntity = new ParamterEntity
                {
                    Message = msg,
                    GroupId = groupId
                }
            });
        }

        public async Task SendMessageAsync(string groupId, string msg)
        {
            //获取所有的连接
            var connections = await groupStorage.GetAsync(groupId);
            if (connections == null || connections.Count() <= 0)
            {
                return;
            }
            //获取所有的socket客户端
            var webSockets = await socketClientStorage.GetByConnectionIdAsync(connections);
            if (webSockets != null && webSockets.Count() > 0)
            {
                Parallel.ForEach(webSockets, async item =>
                {
                    await item.SendMessage(msg);
                });
            }
        }
    }
}
