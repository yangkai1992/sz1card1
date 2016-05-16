using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Communication;
using sz1card1.Common.Log;

namespace sz1card1.Common.Communication
{
    class EposReceiveFilter : IReceiveFilter<EposRequestInfo>
    {
        public EposRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            byte[] buffer = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, buffer, 0, length);
            Message message = Message.FromBytes(buffer, length);
            return new EposRequestInfo(message.Action, message.Identity, message.Body);
        }

        public int LeftBufferSize
        {
            get { return 0; }
        }

        public IReceiveFilter<EposRequestInfo> NextReceiveFilter
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the filter state.
        /// </summary>
        /// <value>
        /// The filter state.
        /// </value>
        public FilterState State { get; private set; }

        public void Reset()
        {

        }
    }
}

