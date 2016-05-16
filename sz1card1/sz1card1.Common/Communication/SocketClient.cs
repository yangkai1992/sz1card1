using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using sz1card1.Common.Delegate;
using sz1card1.Common;

namespace sz1card1.Common.Communication
{
    public class SocketClient
    {
        private bool isConnected = false;
        private int timeoutSeconds = 30;
        private Socket client;
        private ArrayList requests;//请求队列
        private object[] result;//接收结果
        private bool isError = false;
        private byte[] receiveBuffer;
        private int curentBufferIndex = 0;//缓冲区当前位置
        private AutoResetEvent syncEvent = new AutoResetEvent(false);

        public event EventHandler StatusChanged;
        public event PushEventHandler PushReceived;

        public SocketClient()
        {
            requests = new ArrayList();
            receiveBuffer = new byte[1024 * 50];//设置缓冲区大小为20kb
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        /// <summary>
        /// 等待超时时间
        /// </summary>
        public int TimeoutSecondes
        {
            get
            {
                return timeoutSeconds;
            }
            set
            {
                timeoutSeconds = value;
            }
        }

        private void Receive()
        {
            try
            {
                while (isConnected)
                {
                    if (client.Poll(-1, SelectMode.SelectRead) && client.Available > 0)
                    {
                        //将数据接收到缓冲区
                        int count = client.Receive(receiveBuffer, curentBufferIndex, client.Available, SocketFlags.None);
                        curentBufferIndex += count;
                        Message message;
                        //循环从缓冲区中提取完整的消息包
                        while ((message = Message.FromBytes(receiveBuffer, curentBufferIndex)) != null)
                        {
                            switch (message.Type)
                            {
                                case MessageType.Push:
                                    if (PushReceived != null)
                                        PushReceived(message.Action, message.Body);
                                    break;
                                case MessageType.Response:
                                    lock (requests)
                                    {
                                        for (int i = 0; i < requests.Count; i++)
                                        {
                                            object request = requests[i];
                                            if (request.GetType().GetProperty("Identity").GetValue(request, null).ToString() == message.Identity.ToString())
                                            {
                                                if (bool.Parse(request.GetType().GetProperty("IsAsync").GetValue(request, null).ToString()))
                                                {
                                                    RPCCallback callback = (RPCCallback)request.GetType().GetProperty("Callback").GetValue(request, null);
                                                    object obj = request.GetType().GetProperty("Obj").GetValue(request, null);
                                                    if (callback != null)
                                                    {
                                                        callback(obj, message.Body);
                                                    }
                                                }
                                                else
                                                {
                                                    result = message.Body;
                                                    isError = false;
                                                    syncEvent.Set();
                                                }
                                                requests.RemoveAt(i);
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case MessageType.Exception:
                                    lock (requests)
                                    {
                                        for (int i = 0; i < requests.Count; i++)
                                        {
                                            object request = requests[i];
                                            if (request.GetType().GetProperty("Identity").GetValue(request, null).ToString() == message.Identity.ToString())
                                            {
                                                if (bool.Parse(request.GetType().GetProperty("IsAsync").GetValue(request, null).ToString()))
                                                {
                                                    requests.RemoveAt(i);
                                                    throw new Exception(message.Body[0].ToString());
                                                }
                                                else
                                                {
                                                    requests.RemoveAt(i);
                                                    result = message.Body;
                                                    isError = true;
                                                    syncEvent.Set();
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            curentBufferIndex -= message.Length;
                            Buffer.BlockCopy(receiveBuffer, message.Length, receiveBuffer, 0, curentBufferIndex);
                            Array.Clear(receiveBuffer, curentBufferIndex, message.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn(this, ex);
            }
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="host">服务器</param>
        /// <param name="port">端口</param>
        public void Connect(string host, int port)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 20480);//设置发送缓冲区大小20k
            IPHostEntry entry = Dns.GetHostEntry(host);
            IPEndPoint endPoint = new IPEndPoint(entry.AddressList[0], port);
            client.Connect(endPoint);
            isConnected = true;
            if (StatusChanged != null)
                StatusChanged(this, EventArgs.Empty);
            //启动接收线程
            Thread receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        /// <summary>
        /// 同步调用远程方法
        /// </summary>
        /// <param name="action">操作</param>
        /// <param name="body">参数</param>
        /// <returns>结果</returns>
        public object[] Call(string action, object[] body)
        {
            Guid identity = Guid.NewGuid();
            lock (requests)
            {
                requests.Add(new
                {
                    Identity = identity,
                    IsAsync = false,
                    Callback = new object(),
                    Obj = new object()
                });
            }
            Message message = new Message(identity, action, body);
            message.Type = MessageType.Request;
            client.Send(message.ToBytes());
            if (!syncEvent.WaitOne(timeoutSeconds * 1000, false))
            {
                throw new Exception("连接超时!");
            }
            if (isError)
            {
                throw new Exception(result[0].ToString());
            }
            return result;
        }

        /// <summary>
        /// 异步调用远程方法
        /// </summary>
        /// <param name="action">操作</param>
        /// <param name="body">参数</param>
        /// <param name="callback">回调</param>
        /// <param name="obj">关联对象</param>
        public void CallAsync(string action, object[] body, RPCCallback callback, object obj)
        {
            Guid identity = Guid.NewGuid();
            lock (requests)
            {
                requests.Add(new
                {
                    Identity = identity,
                    IsAsync = true,
                    Callback = callback,
                    Obj = obj
                });
            }
            Message message = new Message(identity, action, body);
            message.Type = MessageType.Request;
            client.Send(message.ToBytes());
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (client != null && client.Connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            isConnected = false;
            if (StatusChanged != null)
                StatusChanged(this, EventArgs.Empty);
        }
    }
}
