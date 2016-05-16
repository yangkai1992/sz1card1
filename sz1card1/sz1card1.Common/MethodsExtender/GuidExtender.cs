using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    /// <summary>
    /// Guid类型扩展方法
    /// </summary>
    public static class GuidExtender
    {
        public static long ToInt64(this Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
