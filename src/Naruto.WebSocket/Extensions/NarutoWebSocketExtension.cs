using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Naruto.WebSocket;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Internal.Client;
using Naruto.WebSocket.Internal.Storage;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Extensions
{
    /// <summary>
    /// 中间件的扩展方法
    /// </summary>
    public static class NarutoWebSocketExtension
    {
        /// <summary>
        /// 启用websocket
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseNarutoWebSocket(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            return app.UseMiddleware<NarutoWebSocketMiddleware>();
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNarutoWebSocket<TService>(this IServiceCollection services) where TService : NarutoWebSocketService
        {
            return services.AddNarutoWebSocket<TService>(a => { });
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNarutoWebSocket<TService>(this IServiceCollection services, Action<WebSocketOption> option) where TService : NarutoWebSocketService
        {
            if (services.BuildServiceProvider().GetService<IWebSocketOptionFactory>() == null)
            {
                services.AddSingleton(typeof(IGroupStorage<>), typeof(InMemoryGroupStorage<>));
                services.AddSingleton(typeof(IWebSocketClientStorage<>), typeof(InMemoryWebSocketClientStorage<>));
                services.AddSingleton(typeof(IClientSend<>), typeof(ClientSend<>));
                services.AddSingleton(typeof(IAllClient<>), typeof(AllClient<>));
                services.AddSingleton(typeof(IOtherClient<>), typeof(OtherClient<>));
                services.AddSingleton(typeof(ICurrentClient<>), typeof(CurrentClient<>));
                services.AddSingleton(typeof(IGroupClient<>), typeof(GroupClient<>));
                services.AddSingleton(typeof(IClusterAllClient<>), typeof(AllClient<>));
                services.AddSingleton(typeof(IClusterOtherClient<>), typeof(OtherClient<>));
                services.AddSingleton(typeof(IClusterCurrentClient<>), typeof(CurrentClient<>));
                services.AddSingleton(typeof(IClusterGroupClient<>), typeof(GroupClient<>));
                services.AddSingleton<IEventBusProxy, EventBusProxy>();
                services.AddSingleton<IMessageReviceHandler, MessageReviceHandler>();
                services.AddSingleton<IWebSocketOptionFactory, WebSocketOptionFactory>();

                services.AddScoped(typeof(CurrentContext<>));
                services.AddScoped(typeof(IReplyClient<>), typeof(ReplyClient<>));
            }

            services.AddScoped<TService>();

            //获取配置
            WebSocketOption webSocketOption = new WebSocketOption();
            option?.Invoke(webSocketOption);
            //匹配是否存在
            if (!TenantPathCache.Match(webSocketOption.Path))
            {
                //获取服务类型
                webSocketOption.ServiceType = typeof(TService);
                //添加进路由缓存
                TenantPathCache.Add(webSocketOption.Path, webSocketOption.ServiceType);
                //注入配置服务
                services.Add(new ServiceDescriptor(MergeNamedType.Merge(webSocketOption.Path.ToString(), typeof(WebSocketOption)), serviceProvider => webSocketOption, ServiceLifetime.Singleton));
            }
            return services;
        }
    }
}
