using Microsoft.Extensions.Logging;
using Naruto.Redis;
using Naruto.WebSocket.Extensions;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Redis
{
    /// <summary>
    /// 使用redis进行消息的存储
    /// </summary>
    public class RedisSubscribeMessageStorage : ISubscribeMessageStorage
    {
        /// <summary>
        /// redis 仓储服务
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 存储的key的前缀
        /// </summary>
        private const string KeyPrefix = nameof(RedisSubscribeMessageStorage) + ":";

        private readonly ILogger logger;

        public RedisSubscribeMessageStorage(IRedisRepository _redisRepository, ILogger<RedisSubscribeMessageStorage> _logger)
        {
            redisRepository = _redisRepository;
            logger = _logger;
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<SubscribeMessage> GetAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await redisRepository.String.GetAsync<SubscribeMessage>(KeyPrefix + key);
        }
        /// <summary>
        /// 存储消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscribeMessage"></param>
        /// <returns></returns>
        public async Task StoreAsync(string key, SubscribeMessage subscribeMessage)
        {

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (subscribeMessage == null)
            {
                throw new ArgumentNullException(nameof(subscribeMessage));
            }
            logger.LogTrace("存储订阅的消息,key={key},subscribeMessage=【{subscribeMessage}】", key, subscribeMessage.ToJson());
            //存储消息，并设置90s有效期
            await redisRepository.String.AddAsync(KeyPrefix + key, subscribeMessage, TimeSpan.FromSeconds(90));
        }
    }
}
