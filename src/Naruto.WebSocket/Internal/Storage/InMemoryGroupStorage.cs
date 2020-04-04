using Naruto.WebSocket.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Internal.Storage
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 使用内存存储群组信息
    /// </summary>
    public class InMemoryGroupStorage<TService> : IGroupStorage<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// 存储群组和连接的数据
        /// </summary>
        private readonly ConcurrentDictionary<string, List<string>> cache;

        public InMemoryGroupStorage()
        {
            cache = new ConcurrentDictionary<string, List<string>>();
        }
        public async Task AddAsync(string groupId, string connectionId)
        {
            //获取所有的连接
            var connections = await GetAsync(groupId);
            if (connections == null || connections.Count() <= 0)
            {
                cache.TryRemove(groupId, out var list);
                connections = new List<string>() { connectionId };
                cache.TryAdd(groupId, connections);
            }
            else
            {
                connections.Add(connectionId);
            }
        }

        public async Task AddAsync(string groupId, List<string> connectionIds)
        {
            //获取所有的连接
            var connections = await GetAsync(groupId);
            if (connections == null || connections.Count() <= 0)
            {
                cache.TryRemove(groupId, out var list);
                connections = new List<string>();
                connections.AddRange(connectionIds);
                cache.TryAdd(groupId, connections);
            }
            else
            {
                connections.AddRange(connectionIds);
            }
        }

        public Task<List<string>> GetAsync(string groupId)
        {
            cache.TryGetValue(groupId, out var connections);
            return Task.FromResult(connections);
        }

        public async Task RemoveAsync(string groupId, string connectionId)
        {
            //获取所有的连接
            var connections = await GetAsync(groupId);
            if (connections != null && connections.Count() > 0)
            {
                connections.Remove(connectionId);
            }
        }
    }
}
