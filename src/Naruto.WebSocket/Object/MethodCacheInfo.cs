using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Naruto.WebSocket.Object
{
    public class MethodCacheInfo
    {
        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 方法的参数信息
        /// </summary>
        public ParameterInfo[] ParameterInfos { get; set; } = new ParameterInfo[0];
    }
}
