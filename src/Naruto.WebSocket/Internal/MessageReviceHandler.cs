using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Exceptions;
using Naruto.WebSocket.Extensions;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Internal.Expressions;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal
{
    public sealed class MessageReviceHandler : IMessageReviceHandler
    {
        /// <summary>
        /// 配置信息工厂
        /// </summary>

        private readonly IWebSocketOptionFactory webSocketOptionFactory;

        private readonly ILogger Logger;

        public MessageReviceHandler(IWebSocketOptionFactory _webSocketOptionFactory, ILogger<MessageReviceHandler> _Logger)
        {
            webSocketOptionFactory = _webSocketOptionFactory;
            Logger = _Logger;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentServiceType">当前上下文的类型</param>
        /// <param name="webSocketClient"></param>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        public async Task HandlerAsync(WebSocketClient webSocketClient, Type currentServiceType, WebSocketMessageModel messageModel)
        {
            //获取基类消息
            if (messageModel == null || messageModel.action.IsNullOrEmpty())
            {
                Logger.LogWarning("action={action}：传递的消息不符合约束", messageModel.action);
                return;
            }
            //获取当前租户的服务对象信息
            var service = webSocketClient.Context.RequestServices.GetRequiredService(currentServiceType);

            //验证消息是否为内部的消息
            if (messageModel.action.Equals(NarutoWebSocketServiceMethodEnum.OnConnectionBeginAsync.ToString()) || messageModel.action.Equals(NarutoWebSocketServiceMethodEnum.OnDisConnectionAsync.ToString()))
            {
                Logger.LogTrace("调用内部方法,action={action},ConnectionId={connectionId}", messageModel.action, webSocketClient.ConnectionId);
                await EexecInternalMessage(service, webSocketClient, messageModel).ConfigureAwait(false);
            }
            //验证是否为心跳检查
            else if (string.Compare(messageModel.action, NarutoWebSocketServiceMethodEnum.HeartbeatCheck.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                //不执行任何的操作
                Logger.LogTrace("执行心跳检查,ConnectionId={connectionId}", webSocketClient.ConnectionId);
            }
            else
            {
                Logger.LogTrace("调用外部方法,action={action},ConnectionId={connectionId}", messageModel.action, webSocketClient.ConnectionId);
                await EexecReciveMessage(service, currentServiceType, messageModel).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 执行内部消息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="webSocketClient"></param>
        /// <param name="reciveMessageBase"></param>
        /// <returns></returns>
        private async Task EexecInternalMessage(object service, WebSocketClient webSocketClient, MessageBase reciveMessageBase)
        {
            //执行操作
            await NarutoWebSocketServiceExpression.ExecAsync(service, reciveMessageBase.action, isParamter: true, parameterEntity: webSocketClient, webSocketClient.GetType()).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行接收的外部消息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="currentServiceType">当前上下文的类型</param>
        /// <param name="messageModel">消息模型</param>
        /// <returns></returns>
        private async Task EexecReciveMessage(object service, Type currentServiceType, WebSocketMessageModel messageModel)
        {
            //获取方法
            var methodCacheInfo = MethodCache.Get(currentServiceType, messageModel.action);
            //获取方法的参数
            var parameters = methodCacheInfo.ParameterInfos;

            //是否含有参数
            var isParamater = parameters.Count() > 0;
            //参数信息
            var parameterEntity = parameters.Count() > 0 ? messageModel.message?.ToString().ToDeserialize(parameters[0].ParameterType) : null;
            //执行操作
            await NarutoWebSocketServiceExpression.ExecAsync(service, messageModel.action, isParamter: isParamater, parameterEntity: parameterEntity, parameterType: isParamater ? parameters[0].ParameterType : default).ConfigureAwait(false);
        }
    }
}
