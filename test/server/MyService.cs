using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Internal;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    public class MyService : NarutoWebSocketService, IDisposable
    {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> _logger)
        {
            logger = _logger;
        }
        public override Task OnConnectionBegin(WebSocketClient client)
        {
            return base.OnConnectionBegin(client);
        }

        public override Task OnDisConnection(WebSocketClient client)
        {
            return base.OnDisConnection(client);
        }
        public void Dispose()
        {
           
        }

        public async Task Recive(string str)
        {

        }

        public async Task send(SendMsg sendMsg)
        {
            await Task.Delay(2);
        }

        private async Task send2() { 
        
        }
    }
}
