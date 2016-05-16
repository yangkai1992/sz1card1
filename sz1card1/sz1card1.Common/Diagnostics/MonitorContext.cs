using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Diagnostics
{
    /// <summary>
    /// 监测上下文
    /// </summary>
    internal class MonitorContext
    {
        private string requestIdentity;
        private string requestKeyName;
        private Dictionary<string, object> requestParameters = new Dictionary<string, object>();

        internal MonitorContext(string requestIdentity, string requestKeyName)
        {
            this.requestIdentity = requestIdentity;
            this.requestKeyName = requestKeyName;
        }

        /// <summary>
        /// 请求唯一标识
        /// </summary>
        public string RequestIdentity
        {
            get
            {
                return requestIdentity;
            }
        }

        /// <summary>
        /// 请求实例名
        /// </summary>
        public string RequestKeyName
        {
            get
            {
                return requestKeyName;
            }
        }

        /// <summary>
        /// 请求相关参数
        /// </summary>
        public Dictionary<string, object> RequestParameters
        {
            get
            {
                return requestParameters;
            }
        }
    }
}
