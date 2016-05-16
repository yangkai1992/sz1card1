using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using sz1card1.Common.Delegate;
using sz1card1.Common.Communication;

namespace sz1card1.Common.Iso8583Communication
{
    public class Iso8583Server
    {
        private int port = 8585;
        private string xmlPath;
        private bool isRunning = false;
        private Socket listener;

        public event EventHandler ClientConnected;
        public event PackageEventHandler MessageHandler;
        public event ExceptionEventHandler ExceptionHandler;

        public Iso8583Server(string xmlPath)
        {
            this.xmlPath = xmlPath;
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
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }

        private void Listen()
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress iPAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault<IPAddress>(i => i.AddressFamily == AddressFamily.InterNetwork);
                IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
                listener.Bind(endPoint);
                listener.Listen(100);
                isRunning = true;
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn(this, ex);
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
                    ClientConnected(client, EventArgs.Empty);
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(Receive), client);
            }
        }


        private void Receive(object client)
        {
            Socket socket = client as Socket;
            Iso8583Package oldPacakge = new Iso8583Package(xmlPath);
            try
            {
                if (MessageHandler != null)
                {
                    byte[] buffer = new byte[20480];
                    int length = socket.Receive(buffer);
                    byte[] request = new byte[length];
                    Buffer.BlockCopy(buffer, 0, request, 0, length);
                    if (length <= 20)
                    {
                        socket.Send(request);
                    }
                    else
                    {
                        oldPacakge.ParseBuffer(request);
                        sz1card1.Common.Log.LoggingService.Debug(Utility.ByteToHexWithBlank(request));
                        Iso8583Package responsePakage = MessageHandler(oldPacakge);
                        byte[] response = responsePakage.GetSendBuffer();
                        sz1card1.Common.Log.LoggingService.Debug(Utility.ByteToHexWithBlank(response));
                        socket.Send(response);
                    }
                }
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn(this, ex);
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, oldPacakge);
                }
            }
            CloseClient(socket);
        }

        private void CloseClient(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn(this, ex);
                if (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, null);
                }
            }
        }
    }
}
