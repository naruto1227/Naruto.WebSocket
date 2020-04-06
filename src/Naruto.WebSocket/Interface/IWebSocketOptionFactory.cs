using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-03
    /// 获取配置工厂
    /// </summary>
    public interface IWebSocketOptionFactory : IDisposable
    {

        Task<WebSocketOption> GetAsync(string key);
    }
}
