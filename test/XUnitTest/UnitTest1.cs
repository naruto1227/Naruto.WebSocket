using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Net.WebSockets;
using System.Collections.Generic;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            List<ClientWebSocket> s = new List<ClientWebSocket>();
            for (int i = 0; i < 10000; i++)
            {
                ClientWebSocket webSocket = new ClientWebSocket();
                s.Add(webSocket);
                await webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            }
           
        }
    }
}
