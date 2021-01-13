using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Extensions;
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
    public sealed class CurrentClient<TService> : ICurrentClient<TService>, IClusterCurrentClient<TService> where TService : NarutoWebSocketService
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
        private readonly ILogger logger;

        public CurrentClient(IWebSocketClientStorage<TService> _socketClientStorage, IEventBusProxy _eventBusProxy, ILogger<CurrentClient<TService>> _logger)
        {
            socketClientStorage = _socketClientStorage;
            RequestPath = TenantPathCache.GetByType(typeof(TService));
            eventBusProxy = _eventBusProxy;
            logger = _logger;
        }

        public async Task SendAsync(string connectionId, string execAction, object msg)
        {
            await SendMessageAsync(connectionId, execAction, msg);
            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.Current,
                ParamterEntity = new ParamterEntity
                {
                    Message = new WebSocketMessageModel
                    {
                        action = execAction,
                        message = msg
                    },
                    ConnectionId = connectionId
                }
            });
        }

        public async Task SendAsync(List<string> connectionIds, string execAction, object msg)
        {
            await SendMessageAsync(connectionIds, execAction, msg);
            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.Current,
                ParamterEntity = new ParamterEntity
                {
                    Message = new WebSocketMessageModel
                    {
                        action = execAction,
                        message = msg
                    },
                    ConnectionIds = connectionIds
                },
                ActionTypeEnum = MessageSendActionTypeEnum.Many
            });
        }

        public async Task SendMessageAsync(string connectionId, string execAction, object msg)
        {
            logger.LogTrace("给指定用户发送消息,connectionId={connectionId},execAction={execAction},msg={msg}", connectionId, execAction, msg.ToJson());
            var list = await socketClientStorage.GetByConnectionIdAsync(connectionId);
            foreach (var item in list)
            {
                await item.SendMessage(new Object.WebSocketMessageModel
                {
                    message = msg,
                    action = execAction
                });
            }
        }

        public async Task SendMessageAsync(List<string> connectionIds, string execAction, object msg)
        {
            logger.LogTrace("给指定用户发送消息,connectionId={connectionId},execAction={execAction},msg={msg}", connectionIds.ToJson(), execAction, msg.ToJson());
            var webSockets = await socketClientStorage.GetByConnectionIdAsync(connectionIds);
            Parallel.ForEach(webSockets, async item =>
            {
                await item.SendMessage(new Object.WebSocketMessageModel
                {
                    message = msg,
                    action = execAction
                });
            });
        }
    }
}
