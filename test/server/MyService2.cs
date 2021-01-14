using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    public class MyService2 : NarutoWebSocketService, IDisposable
    {
        private readonly ILogger<MyService> logger;

        public MyService2(ILogger<MyService> _logger)
        {
            logger = _logger;
        }
        protected override Task OnConnectionBeginAsync(WebSocketClient client)
        {
            return base.OnConnectionBeginAsync(client);
        }

        protected override Task OnDisConnectionAsync(WebSocketClient client)
        {
            return base.OnDisConnectionAsync(client);
        }

        public void Dispose()
        {

        }

        public async Task Recive(string str)
        {

        }

        public async Task send(SendMsg sendMsg)
        {
            await Group.AddAsync("11", "2");
            await Reply.SendAsync("test", new { msg = "asdasd", ss="123123" });
            await Task.Delay(2);
        }

        private async Task send2()
        {


        }
    }
}
