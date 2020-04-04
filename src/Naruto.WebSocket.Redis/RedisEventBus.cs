using Naruto.Redis;
using Naruto.Redis.IRedisManage;
using Naruto.WebSocket.Interface;
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

        public RedisEventBus(IRedisRepository _redisRepository)
        {
            redisSubscribe = _redisRepository.Subscribe();
        }
        public Task PublishAsync(string key, string data)
        {
            return redisSubscribe.PublishAsync(key, data);
        }

        public Task SubscribeAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
