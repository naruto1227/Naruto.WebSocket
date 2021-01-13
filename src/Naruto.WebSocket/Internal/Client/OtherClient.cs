using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Extensions;
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
    /// 发送给指定连接外的其它用户
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public sealed class OtherClient<TService> : IOtherClient<TService>, IClusterOtherClient<TService> where TService : NarutoWebSocketService
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

        public OtherClient(IWebSocketClientStorage<TService> _socketClientStorage, IEventBusProxy _eventBusProxy, ILogger<OtherClient<TService>> _logger)
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
                SendTypeEnum = MessageSendTypeEnum.Other,
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

        public async Task SendAsync(List<string> connectionId, string execAction, object msg)
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
                    ConnectionIds = connectionId
                },
                ActionTypeEnum = MessageSendActionTypeEnum.Many
            });
        }

        public async Task SendMessageAsync(string connectionId, string execAction, object msg)
        {
            var webSockets = await socketClientStorage.ExceptConnectionIdAsync(connectionId);
            if (webSockets != null && webSockets.Count() > 0)
            {
                logger.LogTrace("给除此连接外的其它连接发送消息,execAction={execAction},connectionId={connectionId},msg={msg}", execAction, connectionId, msg.ToJson());
                Parallel.ForEach(webSockets, async item => await item.SendMessage(new Object.WebSocketMessageModel
                {
                    message = msg,
                    action = execAction
                }));
            }
        }

        public async Task SendMessageAsync(List<string> connectionId, string execAction, object msg)
        {
            var webSockets = await socketClientStorage.ExceptConnectionIdAsync(connectionId);
            if (webSockets != null && webSockets.Count() > 0)
            {
                logger.LogTrace("给除此连接外的其它连接发送消息,execAction={execAction},connectionId={connectionId},msg={msg}", execAction, connectionId.ToJson(), msg.ToJson());
                Parallel.ForEach(webSockets, async item => await item.SendMessage(new Object.WebSocketMessageModel
                {
                    message = msg,
                    action = execAction
                }));
            }
        }
    }
}
