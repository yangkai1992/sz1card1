using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Log;

namespace sz1card1.Common.Communication
{
    public class EposServer : AppServer<EposSession, EposRequestInfo>
    {

        public EposServer()
            : base(new DefaultReceiveFilterFactory<EposReceiveFilter, EposRequestInfo>())
        {

        }
    }
}