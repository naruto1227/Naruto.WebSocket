using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Client
{
    public class CurrentClient<TService> : ICurrentClient<TService>, IClusterCurrentClient<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// webscoket 客户端存储
        /// </summary>
        private readonly IWebSocketClientStorage socketClientStorage;
        /// <summary>
        /// 定义一个当前服务对应的租户请求path字段
        /// </summary>
        private readonly string RequestPath;
        /// <summary>
        /// 事件总线代理对象
        /// </summary>
        private readonly IEventBusProxy eventBusProxy;
        public CurrentClient(IWebSocketClientStorage<TService> _socketClientStorage, IEventBusProxy _eventBusProxy)
        {
            socketClientStorage = _socketClientStorage;
            RequestPath = TenantPathCache.GetByType(typeof(TService));
            eventBusProxy = _eventBusProxy;
        }

        public async Task SendAsync(string connectionId, string msg)
        {
            await SendMessageAsync(connectionId, msg);
            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.Current,
                ParamterEntity = new ParamterEntity
                {
                    Message = msg,
                    ConnectionId = connectionId
                }
            });
        }

        public async Task SendAsync(List<string> connectionIds, string msg)
        {
            await SendMessageAsync(connectionIds, msg);
            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.Current,
                ParamterEntity = new ParamterEntity
                {
                    Message = msg,
                    ConnectionIds = connectionIds
                },
                ActionTypeEnum = MessageSendActionTypeEnum.Many
            });
        }

        public async Task SendMessageAsync(string connectionId, string msg)
        {
            var list = await socketClientStorage.GetByConnectionIdAsync(connectionId);
            foreach (var item in list)
            {
                await item.SendMessage(msg);
            }
        }

        public async Task SendMessageAsync(List<string> connectionIds, string msg)
        {
            var webSockets = await socketClientStorage.GetByConnectionIdAsync(connectionIds);
            Parallel.ForEach(webSockets, async item =>
            {
                await item.SendMessage(msg);
            });
        }
    }
}
