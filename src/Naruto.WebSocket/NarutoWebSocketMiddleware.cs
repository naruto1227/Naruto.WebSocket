using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Naruto.WebSocket.Extensions;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Object.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.WebSocket
{
    /// <summary>
    /// 张海波
    /// 2020-04-01
    /// websocket的访问中间件
    /// </summary>
    public class NarutoWebSocketMiddleware
    {

        /// <summary>
        /// 接收的消息的大小 
        /// </summary>
        private long ReciveBuffSize;


        private readonly RequestDelegate next;


        private readonly ILogger<NarutoWebSocketMiddleware> logger;
        /// <summary>
        /// 配置工厂
        /// </summary>
        private readonly IWebSocketOptionFactory socketOptionFactory;

        private readonly IServiceProvider serviceProvider;



        public NarutoWebSocketMiddleware(RequestDelegate _next, ILogger<NarutoWebSocketMiddleware> _logger, IWebSocketOptionFactory _socketOptionFactory, IServiceProvider _serviceProvider)
        {
            next = _next;
            logger = _logger;
            socketOptionFactory = _socketOptionFactory;
            serviceProvider = _serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //验证请求地址
            if (TenantPathCache.Match(context.Request.Path))
            {
                //验证是否为websocket请求
                if (context.WebSockets.IsWebSocketRequest)
                {
                    //获取当前租户的配置信息
                    var webSocketOption = await socketOptionFactory.GetAsync(context.Request.Path);
                    //获取buff接收大小
                    ReciveBuffSize = webSocketOption.MaximumReceiveMessageSize;
                    //授权验证
                    foreach (var item in webSocketOption.AuthorizationFilters)
                    {
                        if ((await item.AuthorizationAsync(context)) == false)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return;
                        }
                    }
                    //获取传递的连接Id
                    var connectionId = context.Request.Query[RequestQuery.ConnectionId.ToString()];
                    //接收websocket客户端
                    var webSocketClient = new WebSocketClient
                    {
                        WebSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false),
                        ConnectionId = connectionId.Any() ? connectionId.ToString() : Guid.NewGuid().ToString(),
                        Context = context
                    };
                    //处理
                    await Handler(context, webSocketClient).ConfigureAwait(false);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    logger.LogWarning("当前不是一个有效的webscoket连接");
                }
            }
            else
            {
                await next(context);
            }
        }

        private async Task Handler(HttpContext context, WebSocketClient webSocketClient)
        {
            //存储的key
            var key = Guid.NewGuid();
            //获取当前租户的配置信息
            var webSocketOption = await socketOptionFactory.GetAsync(context.Request.Path);
            //获取客户端存储对象
            var clientStorage = serviceProvider.GetRequiredService(typeof(IWebSocketClientStorage<>).MakeGenericType(webSocketOption.ServiceType)) as IWebSocketClientStorage;
            //添加到集合中 将当前用户连接
            await clientStorage.AddAsync(key, webSocketClient);
            //获取服务
            var messageRevice = context.RequestServices.GetRequiredService<IMessageReviceHandler>();
            // 获取当前上下文实体 配置数据信息
            BuildCurrentContext(context: context, key: key, webSocketClient: webSocketClient);
            //获取拦截器
            var wesocketIntercept = context.RequestServices.GetService<IWesocketIntercept>();
            try
            {

                //触发上线通知
                if (wesocketIntercept != null)
                {
                    await wesocketIntercept.OnLineAsync(webSocketClient);
                }

                logger.LogTrace("执行上线通知连接方法,{connectionId}", webSocketClient.ConnectionId);
                //调用开启连接的方法
                await messageRevice.HandlerAsync(webSocketClient, webSocketOption.ServiceType, new WebSocketMessageModel { action = NarutoWebSocketServiceMethodEnum.OnConnectionBeginAsync.ToString() }).ConfigureAwait(false);

                //接收消息 判断是否开启连接
                while (webSocketClient.WebSocket.State == WebSocketState.Open)
                {
                    //定义接收消息内容的大小
                    var contentBytes = new byte[ReciveBuffSize];
                    //接收消息
                    var result = await webSocketClient.WebSocket.ReceiveAsync(contentBytes, CancellationToken.None);

                    //关闭的时候直接退出
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }
                    //构建消息模型
                    var messageModel = BuildMessageModel(result.MessageType, webSocketClient.ConnectionId, contentBytes);
                    //处理接收消息事件
                    if (wesocketIntercept != null)
                    {
                        await wesocketIntercept.ReciveAsync(webSocketClient, messageModel);
                    }
                    //处理消息
                    await messageRevice.HandlerAsync(webSocketClient, webSocketOption.ServiceType, messageModel).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.ToString());
            }
            finally
            {
                logger.LogTrace("执行下线通知连接方法,{connectionId}", webSocketClient.ConnectionId);
                //处理断开事件的事件
                await messageRevice.HandlerAsync(webSocketClient, webSocketOption.ServiceType, new WebSocketMessageModel { action = NarutoWebSocketServiceMethodEnum.OnDisConnectionAsync.ToString() }).ConfigureAwait(false);
                //移除客户端缓存
                await clientStorage.RemoveAsync(key);
                //下线的通知
                if (wesocketIntercept != null)
                {
                    await wesocketIntercept.OffLineAsync(webSocketClient);
                }
            }
        }
        /// <summary>
        /// 构建消息模型
        /// </summary>
        /// <param name="webSocketMessageType">消息类型</param>
        /// <param name="bytes">接收的内容</param>
        /// <param name="connectionId">连接id</param>
        /// <returns></returns>
        private WebSocketMessageModel BuildMessageModel(WebSocketMessageType webSocketMessageType, string connectionId, byte[] bytes)
        {
            //定义接收的对象
            var sendMessageModel = new WebSocketMessageModel();
            //验证消息类型 是否为文本
            if (webSocketMessageType == WebSocketMessageType.Text)
            {
                //转换消息格式
                var msg = bytes.ToUtf8String();
                //反序列化
                sendMessageModel = msg.ToDeserialize<WebSocketMessageModel>();
                logger.LogTrace("接收文本消息,{connectionId},{msg}", connectionId, msg);
            }
            //验证是否为二进制数据
            else if (webSocketMessageType == WebSocketMessageType.Binary)
            {
                //var res = MessagePack.MessagePackSerializer.ConvertToJson(bytes);
                //if (!res.IsNullOrEmpty())
                //{
                //    sendMessageModel = res.ToDeserialize<WebSocketMessageModel>();
                //}
                throw new NotSupportedException("暂未实现二进制数据接收");
            }
            return sendMessageModel;
        }
        /// <summary>
        ///  构建当前上下文实体 配置数据信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="webSocketClient"></param>
        private void BuildCurrentContext(HttpContext context, Guid key, WebSocketClient webSocketClient)
        {
            var currentContext = context.RequestServices.GetRequiredService(typeof(CurrentContext<>).MakeGenericType(TenantPathCache.GetByKey(context.Request.Path))) as CurrentContext;
            currentContext.WebSocketClient = webSocketClient;
            currentContext.Key = key;
        }
    }
}
