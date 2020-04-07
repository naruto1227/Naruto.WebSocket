using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal
{
    /// <summary>
    /// 张海波
    /// 2020-04-03
    /// websocket的服务抽象类 需继承此接口
    /// Scope
    /// </summary>
    public abstract class NarutoWebSocketService
    {
        /// <summary>
        /// 继承此泛型的类型
        /// </summary>
        private Type GenericType { get; set; }
        /// <summary>
        /// http上下文
        /// </summary>
        protected HttpContext Context { get; private set; }
        /// <summary>
        /// 连接Id
        /// </summary>
        protected string ConnectionId { get; private set; }

        /// <summary>
        /// 群组管理
        /// </summary>
        protected IGroupStorage Group { get; private set; }

        /// <summary>
        /// 消息发送客户端
        /// </summary>

        protected IClientSend Client { get; private set; }
        /// <summary>
        /// 回复的客户端
        /// </summary>

        protected IReplyClient Reply { get; private set; }

        public NarutoWebSocketService()
        {
            GenericType = this.GetType();
        }
        /// <summary>
        /// 开始连接
        /// </summary>
        /// <returns></returns>
        public virtual Task OnConnectionBeginAsync(WebSocketClient client)
        {

            #region 初始化数据

            Context = client.Context;
            ConnectionId = client.ConnectionId;
            Group = Context.RequestServices.GetRequiredService(typeof(IGroupStorage<>).MakeGenericType(GenericType)) as IGroupStorage;
            Client = Context.RequestServices.GetRequiredService(typeof(IClientSend<>).MakeGenericType(GenericType)) as IClientSend;
            Client = Context.RequestServices.GetRequiredService(typeof(IClientSend<>).MakeGenericType(GenericType)) as IClientSend;
            Reply = Context.RequestServices.GetRequiredService(typeof(IReplyClient<>).MakeGenericType(GenericType)) as IReplyClient;
            #endregion

            return Task.CompletedTask;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public virtual Task OnDisConnectionAsync(WebSocketClient client)
        {
            return Task.CompletedTask;
        }
    }
}
