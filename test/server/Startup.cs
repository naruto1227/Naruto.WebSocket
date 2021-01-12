using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Naruto.WebSocket;
using Naruto.WebSocket.Extensions;
using Naruto.WebSocket.Interface;
//using Naruto.WebSocket.Redis;
//using Naruto.WebSocket.Redis;

namespace server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNarutoWebSocket<MyService2>(a => a.Path = new PathString("/taskmanagement"));
            services.AddNarutoWebSocket<MyService>(a =>
            {
                a.Path = new PathString("/ws");
                a.AuthorizationFilters.Add(new MyAuthorizationFilters());
            });
            //services.AddNarutoWebSocketRedis(a => a.Connection = new string[] { "127.0.0.1:6379" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IClientSend<MyService> clientSend)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseNarutoWebSocket();

            NarutoWebSocketEvent.RegisterOnLine(a =>
            {
                Console.WriteLine($"{a.ConnectionId}:上线");
            });
            NarutoWebSocketEvent.RegisterOffLine(a =>
            {
                Console.WriteLine($"{a.ConnectionId}:下线");
            });
            //NarutoWebSocketEvent.RegisterRecive((client, msg) =>
            //{
            //    Console.WriteLine(msg);
            //});
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
