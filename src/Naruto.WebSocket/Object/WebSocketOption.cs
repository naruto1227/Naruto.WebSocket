using Microsoft.AspNetCore.Http;
using Naruto.WebSocket.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// websocket的配置操作
    /// </summary>
    public class WebSocketOption
    {
        /// <summary>
        /// websocket的请求地址 默认为/ws
        /// </summary>
        public PathString Path { get; set; } = new PathString("/ws");

        /// <summary>
        /// 授权
        /// </summary>
        public List<INarutoWeSocketAuthorizationFilters> AuthorizationFilters { get; set; } = new List<INarutoWeSocketAuthorizationFilters>();

        /// <summary>
        /// 服务类型
        /// </summary>
        internal Type ServiceType { get; set; }
    }
}
