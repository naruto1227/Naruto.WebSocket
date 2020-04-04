﻿using Microsoft.Extensions.Logging;
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
        public override async Task OnConnectionBeginAsync(WebSocketClient client)
        {
            await base.OnConnectionBeginAsync(client);
            logger.LogInformation($"{ConnectionId}:成功连接");
        }

        public override Task OnDisConnectionAsync(WebSocketClient client)
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
            await Task.Delay(2);
        }

        private async Task send2()
        {


        }
    }
}