using Naruto.WebSocket.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 群组的存储操作
    /// </summary>
    public interface IGroupStorage : IDisposable
    {
        /// <summary>
        /// 添加连接到群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task AddAsync(string groupId, string connectionId);
        /// <summary>
        /// 添加连接到群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task AddAsync(string groupId, List<string> connectionIds);
        /// <summary>
        /// 移除群组中的连接
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task RemoveAsync(string groupId, string connectionId);

        /// <summary>
        /// 获取群组中的所有的连接
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<string>> GetAsync(string groupId);
    }
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 群组的存储操作
    /// </summary>
    public interface IGroupStorage<TService> : IGroupStorage where TService : NarutoWebSocketService
    {
    }
}
