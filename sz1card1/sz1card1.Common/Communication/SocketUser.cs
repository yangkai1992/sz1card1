using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;

namespace sz1card1.Common.Communication
{
    /// <summary>
    /// socket用户
    /// </summary>
    public class SocketUser
    {
        private Guid sessionId;
        private byte[] receiveBuffer;
        private int receiveBufferIndex;
        private Socket socket;
        private SocketAsyncEventArgs receiveArgs;

        public SocketUser(Socket client)
        {
            receiveBuffer = new byte[1024 * 1024 * 5];
            receiveBufferIndex = 0;
            socket = client;
            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.UserToken = this;
            sessionId = Guid.NewGuid();
        }

        /// <summary>
        /// 会话ID
        /// </summary>
        public Guid SessionId
        {
            get
            {
                return sessionId;
            }
        }

        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        public byte[] ReceiveBuffer
        {
            get
            {
                return receiveBuffer;
            }
        }

        /// <summary>
        /// 接收缓冲区当前位置
        /// </summary>
        public int ReceiveBufferIndex
        {
            get
            {
                return receiveBufferIndex;
            }
            internal set
            {
                receiveBufferIndex = value;
            }
        }

        /// <summary>
        /// 通信套接字
        /// </summary>
        public Socket Socket
        {
            get
            {
                return socket;
            }
        }

        /// <summary>
        /// 接收数据上下文
        /// </summary>
        public SocketAsyncEventArgs ReceiveArgs
        {
            get
            {
                return receiveArgs;
            }
        }
    }
}
