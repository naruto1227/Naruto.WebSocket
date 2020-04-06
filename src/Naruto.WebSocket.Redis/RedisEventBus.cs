using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Naruto.Redis;
using Naruto.Redis.IRedisManage;
using Naruto.WebSocket.Extensions;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Object.Enums;
using System;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Redis
{
    /// <summary>
    /// 使用redis作为事件总线的实现者
    /// </summary>
    public class RedisEventBus : IEventBus
    {
        /// <summary>
        /// redis发布订阅服务
        /// </summary>
        private readonly IRedisSubscribe redisSubscribe;

        private readonly ILogger<RedisEventBus> logger;


        private readonly IServiceProvider serviceProvider;


        private readonly ISubscribeMessageStorage messageStorage;

        public RedisEventBus(IRedisRepository _redisRepository, ILogger<RedisEventBus> _logger, IServiceProvider _serviceProvider, ISubscribeMessageStorage _messageStorage)
        {
            redisSubscribe = _redisRepository.Subscribe();
            logger = _logger;
            serviceProvider = _serviceProvider;
            messageStorage = _messageStorage;
        }
        public Task PublishAsync(string key, string data)
        {
            return redisSubscribe.PublishAsync(key, data);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task SubscribeMessageAsync()
        {
            await redisSubscribe.SubscribeAsync(GlobalCache.ClusterSendMsgKey, async (channel, message) =>
            {
                if (message.HasValue)
                {
                    try
                    {
                        //获取消息
                        var subscribeMessage = await messageStorage.GetAsync(message.ToString());
                        //logger.LogInformation($"接收消息订阅信息之前:messageSystemIdentity=[{subscribeMessage?.SystemIdentity}],NowSystemIdentity=[{GlobalCache.SystemIdentity}]");
                        //验证系统的标识，不处理当前系统的发送出去的消息
                        if (subscribeMessage == null || subscribeMessage.SystemIdentity.Equals(GlobalCache.SystemIdentity))
                        {
                            return;
                        }
                        switch (subscribeMessage.SendTypeEnum)
                        {
                            //所有用户
                            case MessageSendTypeEnum.All:
                                //获取客户端
                                var allClient = serviceProvider.GetRequiredService(typeof(IClusterAllClient<>).MakeGenericType(TenantPathCache.GetByKey(subscribeMessage.TenantIdentity))) as IClusterAllClient;
                                await allClient.SendMessageAsync(subscribeMessage.ParamterEntity.Message);
                                break;
                            //群组用户
                            case MessageSendTypeEnum.Group:
                                //获取客户端
                                var groupClient = serviceProvider.GetRequiredService(typeof(IClusterGroupClient<>).MakeGenericType(TenantPathCache.GetByKey(subscribeMessage.TenantIdentity))) as IClusterGroupClient;
                                await groupClient.SendMessageAsync(subscribeMessage.ParamterEntity.GroupId, subscribeMessage.ParamterEntity.Message);
                                break;

                            case MessageSendTypeEnum.Current:
                                //获取客户端
                                var currentClient = serviceProvider.GetRequiredService(typeof(IClusterCurrentClient<>).MakeGenericType(TenantPathCache.GetByKey(subscribeMessage.TenantIdentity))) as IClusterCurrentClient;
                                if (subscribeMessage.ActionTypeEnum == MessageSendActionTypeEnum.Single)
                                {
                                    await currentClient.SendMessageAsync(subscribeMessage.ParamterEntity.ConnectionId, subscribeMessage.ParamterEntity.Message);
                                }
                                if (subscribeMessage.ActionTypeEnum == MessageSendActionTypeEnum.Many)
                                {
                                    await currentClient.SendMessageAsync(subscribeMessage.ParamterEntity.ConnectionIds, subscribeMessage.ParamterEntity.Message);
                                }

                                break;
                            case MessageSendTypeEnum.Other:
                                //获取客户端
                                var otherClient = serviceProvider.GetRequiredService(typeof(IClusterOtherClient<>).MakeGenericType(TenantPathCache.GetByKey(subscribeMessage.TenantIdentity))) as IClusterOtherClient;
                                if (subscribeMessage.ActionTypeEnum == MessageSendActionTypeEnum.Single)
                                {
                                    await otherClient.SendMessageAsync(subscribeMessage.ParamterEntity.ConnectionId, subscribeMessage.ParamterEntity.Message);
                                }
                                if (subscribeMessage.ActionTypeEnum == MessageSendActionTypeEnum.Many)
                                {
                                    await otherClient.SendMessageAsync(subscribeMessage.ParamterEntity.ConnectionIds, subscribeMessage.ParamterEntity.Message);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }
            });
        }
    }
}
