using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Naruto.WebSocket.Object
{
    /// <summary>
    /// 张海波
    /// 2020-03-21
    /// 命名类型
    /// </summary>
    internal sealed class MergeNamedType : TypeDelegator
    {
        /// <summary>
        /// 类型缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, MergeNamedType> _cache = new ConcurrentDictionary<string, MergeNamedType>();
        /// <summary>
        /// 将类型和名称合并成一个新的type
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static MergeNamedType Merge(string name, Type serviceType) =>
            _cache.GetOrAdd((name), x => new MergeNamedType(name, serviceType));

        /// <summary>
        /// 根据名称获取type
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static MergeNamedType Get(string name)
        {
            if (_cache.TryGetValue(name, out var namedType))
            {
                return namedType;
            }
            throw new ArgumentException(nameof(name));
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="name">类型名称</param>
        /// <param name="serviceType">服务类型</param>
        private MergeNamedType(string name, Type serviceType)
            : base(typeof(object))
        {
            Name = name;
            using (var md5Hash = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(name);
                var hash = md5Hash.ComputeHash(bytes);
                GUID = new Guid(hash);
            }
            ServiceType = serviceType;
        }
        /// <summary>
        /// 实际服务类型
        /// </summary>
        public Type ServiceType { get; }
        /// <inherit />
        public override string Name { get; }
        /// <inherit />
        public override Guid GUID { get; }
        /// <inherit />
        public override bool Equals(object obj) => Equals(obj as MergeNamedType);
        /// <inherit />
        public override int GetHashCode() => Name.GetHashCode();
        /// <inherit />
        public override bool Equals(Type o) => o is MergeNamedType t && t.Name == Name;
        /// <inherit />
        public override string FullName => $"MergeNamedType.{Name}";

    }
}
