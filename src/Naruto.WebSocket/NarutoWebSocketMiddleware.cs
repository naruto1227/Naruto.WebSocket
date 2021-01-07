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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        /// 接收的消息的大小 默认4K
        /// </summary>
        private const long ReciveBuffSize = 1024 * 4;


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
            //根据请求地址获取租户信息
            //验证请求地址
            if (TenantPathCache.Match(context.Request.Path))
            {
                //验证是否为websocket请求
                if (context.WebSockets.IsWebSocketRequest)
                {
                    //获取当前租户的配置信息
                    var webSocketOption = await socketOptionFactory.GetAsync(context.Request.Path);
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
                    WebSocketClient webSocketClient = new WebSocketClient
                    {
                        WebSocket = await context.WebSockets.AcceptWebSocketAsync(),
                        ConnectionId = connectionId.Any() ? connectionId.ToString() : Guid.NewGuid().ToString(),
                        Context = context
                    };
                    //处理
                    await Handler(context, webSocketClient).ConfigureAwait(false);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    logger.LogInformation("当前不是一个有效的webscoket连接");
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

            try
            {
                //触发上线通知
                NarutoWebSocketEvent.OnLineEvent?.Invoke(webSocketClient);
                //调用开启连接的方法
                await messageRevice.HandlerAsync(webSocketClient, new ReciveMessageBase { action = NarutoWebSocketServiceMethodEnum.OnConnectionBeginAsync.ToString() }.ToJson()).ConfigureAwait(false);

                //接收消息 判断是否开启连接
                while (webSocketClient.WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    var bytes = new byte[ReciveBuffSize];
                    //接收消息
                    var result = await webSocketClient.WebSocket.ReceiveAsync(bytes, CancellationToken.None);
                    //验证消息类型 是否为文本
                    if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                    {
                        //转换消息格式
                        var msg = bytes.ToUtf8String();
                        //处理接收消息事件
                        NarutoWebSocketEvent.ReciveEvent?.Invoke(webSocketClient, msg);
                        //处理消息
                        await messageRevice.HandlerAsync(webSocketClient, msg).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
            finally
            {
                //处理断开事件的事件
                await messageRevice.HandlerAsync(webSocketClient, new ReciveMessageBase { action = NarutoWebSocketServiceMethodEnum.OnDisConnectionAsync.ToString() }.ToJson()).ConfigureAwait(false);
                //移除客户端缓存
                await clientStorage.RemoveAsync(key);
                //下线的通知
                NarutoWebSocketEvent.OffLineEvent?.Invoke(webSocketClient);
            }
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
