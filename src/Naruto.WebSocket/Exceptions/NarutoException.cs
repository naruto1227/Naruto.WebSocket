using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Exceptions
{
    /// <summary>
    /// 查找不到配置异常
    /// </summary>
    public class NotFoundOptionsException : ApplicationException
    {
        public NotFoundOptionsException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// 找不到指定的方法
    /// </summary>
    public class NotMethodException : ApplicationException
    {
        public NotMethodException(string message) : base(message)
        {

        }
    }
}
