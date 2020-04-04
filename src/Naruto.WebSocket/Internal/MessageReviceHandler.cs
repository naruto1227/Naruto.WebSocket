using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
    public class MessageReviceHandler : IMessageReviceHandler
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly IServiceProvider serviceProvider;
        /// <summary>
        /// 配置信息工厂
        /// </summary>

        private readonly IWebSocketOptionFactory webSocketOptionFactory;


        public MessageReviceHandler(IServiceProvider _serviceProvider, IWebSocketOptionFactory _webSocketOptionFactory)
        {
            serviceProvider = _serviceProvider;
            webSocketOptionFactory = _webSocketOptionFactory;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webSocketClient"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task HandlerAsync(HttpContext context, WebSocketClient webSocketClient, string msg)
        {
            //获取基类消息
            var reciveMessageBase = msg.ToDeserialize<ReciveMessageBase>();

            //获取配置
            var webSocketOption = await webSocketOptionFactory.GetAsync(context.Request.Path);
            if (webSocketOption == null)
                throw new NotFoundOptionsException($"{context.Request.Path}：找不到对应的ws配置");
            //获取当前租户的服务对象信息
            var service = context.RequestServices.GetRequiredService(webSocketOption.ServiceType);

            //验证消息是否为内部的消息
            if (reciveMessageBase.action.Equals(NarutoWebSocketServiceMethodEnum.OnConnectionBegin.ToString()) || reciveMessageBase.action.Equals(NarutoWebSocketServiceMethodEnum.OnDisConnection.ToString()))
            {
                await EexecInternalMessage(service, webSocketClient, reciveMessageBase).ConfigureAwait(false);
            }
            else
            {
                await EexecReciveMessage(service, webSocketOption, reciveMessageBase, msg).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 执行内部消息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="webSocketClient"></param>
        /// <param name="reciveMessageBase"></param>
        /// <returns></returns>
        private async Task EexecInternalMessage(object service, WebSocketClient webSocketClient, ReciveMessageBase reciveMessageBase)
        {
            //执行操作
            await NarutoWebSocketServiceExpression.ExecAsync(service, reciveMessageBase.action, webSocketClient).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行接收的外部消息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="webSocketOption"></param>
        /// <param name="reciveMessageBase"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task EexecReciveMessage(object service, WebSocketOption webSocketOption, ReciveMessageBase reciveMessageBase, string msg)
        {
            //获取方法
            var method = MethodCache.Get(webSocketOption.ServiceType, reciveMessageBase.action);
            //获取方法的参数
            var parameters = method.GetParameters();
            //执行操作
            await NarutoWebSocketServiceExpression.ExecAsync(service, reciveMessageBase.action, parameters.Count() > 0 ? msg.ToDeserialize(parameters[0].ParameterType) : null).ConfigureAwait(false);
        }
    }
}
