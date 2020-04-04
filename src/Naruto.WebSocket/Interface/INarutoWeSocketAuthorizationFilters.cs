using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// websocket的授权过滤器
    /// </summary>
    public interface INarutoWeSocketAuthorizationFilters
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> AuthorizationAsync(HttpContext context);
    }
}
