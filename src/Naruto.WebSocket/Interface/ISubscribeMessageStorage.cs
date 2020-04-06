using Naruto.WebSocket.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-04-05
    /// 存储发布的消息接口
    /// 单例
    /// </summary>
    public interface ISubscribeMessageStorage
    {

        /// <summary>
        /// 存储消息
        /// </summary>
        /// <param name="key">消息的主键key</param>
        /// <param name="subscribeMessage">消息内容实体</param>
        /// <returns></returns>
        Task StoreAsync(string key, SubscribeMessage subscribeMessage);

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="key">消息的主键key</param>
        /// <returns></returns>
        Task<SubscribeMessage> GetAsync(string key);
    }
}
