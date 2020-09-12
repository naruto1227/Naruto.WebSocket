using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Redis
{
    public class EnableSubscribeHostServices : BackgroundService
    {
        private readonly IEventBus eventBus;

        private readonly ILogger logger;


        public EnableSubscribeHostServices(IEventBus _eventBus, ILogger<EnableSubscribeHostServices> _logger)
        {
            eventBus = _eventBus;
            logger = _logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
            {
                logger.LogInformation("已停止");
            });
            logger.LogInformation("开启订阅");
            //启用订阅
            await eventBus.SubscribeMessageAsync();
        }
    }
}
