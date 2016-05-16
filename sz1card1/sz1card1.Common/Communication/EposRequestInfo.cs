using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Communication
{
    public class EposRequestInfo : IRequestInfo
    {
        public EposRequestInfo(string key, Guid identity, object[] body)
        {
            this.Key = key;
            this.Body = body;
            this.Identity = identity;
        }

        /// <summary>
        /// Gets the key of this request.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Identity { get; private set; }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        public object[] Body { get; private set; }
    }
}
