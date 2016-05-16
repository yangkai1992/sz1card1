using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using sz1card1.Common.Delegate;
using sz1card1.Common.Memcached;

namespace sz1card1.Common.Communication
{
    public class SocketServer
    {
        private int port = 9096;
        private bool isRunning = false;
        private Socket listener;//面向连接的服务器端Socket
        private List<SocketUser> users;//客户端Socket列表

        public event EventHandler ClientConnected;
        public event RPCEventHandler MessageHandler;
        public event ExceptionEventHandler ExceptionHandler;

        public SocketServer()
        {
            users = new List<SocketUser>();
        }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        /// <summary>
        /// 启动socket服务
        /// </summary>
        public void Start()
        {
            Thread listenThread = new Thread(new ThreadStart(Listen));
            listenThread.IsBackground = true;
            listenThread.Start();
        }


        /// <summary>
        /// 推消息到指定客户端
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="action">操作</param>
        /// <param name="body">内容</param>
        public void Push(Guid sessionId, string action, object[] body)
        {
            SocketUser user = users.FirstOrDefault<SocketUser>(u => u.SessionId == sessionId);
            if (user == null) return;
            Message message = new Message(Guid.NewGuid(), action, body);
            message.Type = MessageType.Push;
            try
            {
                user.Socket.Send(message.ToBytes());
            }
            catch (Exception ex)
            {
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, user);
                }
                CloseClient(user);
            }

        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            for (int i = users.Count - 1; i >= 0; i--)
            {
                if (users[i].Socket != null && users[i].Socket.Connected)
                {
                    users[i].Socket.Shutdown(SocketShutdown.Both);
                    users[i].Socket.Close();
                }
            }
            if (listener.Connected)
            {
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }

        private void Listen()
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                IPAddress iPAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault<IPAddress>(i => i.AddressFamily == AddressFamily.InterNetwork);
                sz1card1.Common.Log.LoggingService.Debug("监听IP地址：" + iPAddress.ToString());
                IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
                listener.Bind(endPoint);
                listener.Listen(100);
                isRunning = true;
            }
            catch (Exception ex)
            {
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, null);
                    return;
                }
            }
            while (isRunning)
            {
                Socket client = listener.Accept();
                if (ClientConnected != null)
                {
                    ClientConnected(this, EventArgs.Empty);
                }
                SocketUser user = new SocketUser(client);
                //sz1card1.Common.Log.LoggingService.Info("客户端IP:" + ((IPEndPoint)client.RemoteEndPoint).Address.ToString());
                lock (users)
                {
                    if (users.Count() > 100)
                    {
                        CloseClient(users[users.Count() - 1]);
                    }
                    users.Add(user);
                    sz1card1.Common.Log.LoggingService.Info("当前连接用户数:" + users.Count());
                }
                user.ReceiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveArgs_Completed);
                user.ReceiveArgs.SetBuffer(user.ReceiveBuffer, 0, user.ReceiveBuffer.Length);
                try
                {
                    bool status = client.ReceiveAsync(user.ReceiveArgs);
                    if (!status)
                    {
                        CompleteReceive(user.ReceiveArgs);
                    }
                }
                catch (Exception ex)
                {
                    if (ExceptionHandler != null)
                    {
                        ExceptionHandler(ex, user);
                    }
                    CloseClient(user);
                }
            }
        }

        void ReceiveArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            SocketUser user = e.UserToken as SocketUser;
            if (e.BytesTransferred == 0)
            {
                CloseClient(user);
            }
            else
            {
                CompleteReceive(e);
            }
        }

        private void CompleteReceive(SocketAsyncEventArgs e)
        {
            SocketUser user = e.UserToken as SocketUser;
            user.ReceiveBufferIndex += e.BytesTransferred;
            Message message;
            while ((message = Message.FromBytes(e.Buffer, user.ReceiveBufferIndex)) != null)
            {
                user.ReceiveBufferIndex -= message.Length;
                Buffer.BlockCopy(e.Buffer, message.Length, e.Buffer, 0, user.ReceiveBufferIndex);
                Array.Clear(e.Buffer, user.ReceiveBufferIndex, message.Length);
                if (message != null && message.Type == MessageType.Request && MessageHandler != null)
                {
                    Message response;
                    try
                    {
                        DistributedContext.RegisterContext(user.SessionId.ToString());
                        object[] result = MessageHandler(user.SessionId.ToString(), message.Action, message.Body);
                        response = new Message(message.Identity, message.Action, result);
                        response.Type = MessageType.Response;
                        DistributedContext.UnRegisterContext(user.SessionId.ToString());
                    }
                    catch (Exception ex)
                    {
                        DistributedContext.UnRegisterContext(user.SessionId.ToString());
                        if (ExceptionHandler != null)
                            ExceptionHandler(ex, string.Format("会话ID:{0},Action:{1}", user.SessionId, message.Action));
                        response = new Message(message.Identity, message.Action, new object[] { ex.Message });
                        response.Type = MessageType.Exception;
                    }
                    try
                    {
                        byte[] b = response.ToBytes();
                        user.Socket.Send(b);
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionHandler != null)
                        {
                            ExceptionHandler(ex, null);
                        }
                        CloseClient(user);
                        return;
                    }
                }
            }
            user.ReceiveArgs.SetBuffer(user.ReceiveBuffer, user.ReceiveBufferIndex, user.ReceiveBuffer.Length - user.ReceiveBufferIndex);
            try
            {
                bool status = user.Socket.ReceiveAsync(user.ReceiveArgs);
                if (!status)
                {
                    CompleteReceive(user.ReceiveArgs);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, null);
                }
                CloseClient(user);
            }
        }

        private void CloseClient(SocketUser user)
        {
            try
            {
                users.Remove(user);
                if (user.Socket != null && user.Socket.Connected)
                {
                    user.Socket.Shutdown(SocketShutdown.Both);
                    user.Socket.Close();
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, null);
                }
            }
        }
    }
}
