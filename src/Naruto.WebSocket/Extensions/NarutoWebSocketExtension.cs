using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Naruto.WebSocket;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Internal.Cache;
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
                services.AddSingleton<IMessageReviceHandler, MessageReviceHandler>();
                services.AddSingleton<IWebSocketOptionFactory, WebSocketOptionFactory>();
            }

            services.AddScoped<TService>();

            //获取配置
            WebSocketOption webSocketOption = new WebSocketOption();
            option?.Invoke(webSocketOption);
            //匹配是否存在
            if (!PathCache.Match(webSocketOption.Path))
            {
                //添加进路由缓存
                PathCache.Add(webSocketOption.Path);
                webSocketOption.ServiceType = typeof(TService);
                //注入配置服务
                services.Add(new ServiceDescriptor(MergeNamedType.Merge(webSocketOption.Path.ToString(), typeof(WebSocketOption)), serviceProvider => webSocketOption, ServiceLifetime.Singleton));
            }
            return services;
        }
    }
}
