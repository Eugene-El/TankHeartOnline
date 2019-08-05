using CommandNetworking.Data;
using CommandNetworking.Extensions;
using FastLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CommandNetworking.Logic
{
    public abstract class Server
    {
        protected ManualResetEvent _allDone = new ManualResetEvent(false);
        protected int _port;
        protected List<Connection> _connections = new List<Connection>();

        public Server(int port = 11001)
        {
            _port = port;
        }

        public void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                Logger.Instance.Log(string.Format("Started on {0}:{1}", ipAddress.ToString(), _port));

                while (true)
                {
                    _allDone.Reset();

                    Logger.Instance.Log("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
 
                    _allDone.WaitOne();
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
            Logger.Instance.Log("Server ended");
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _allDone.Set();
 
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Connection connection = new Connection(handler);
            _connections.Add(connection);
            

            handler.BeginReceive(connection.WorkingBuffer, 0, Connection.BufferSize, 0,
                new AsyncCallback(ReadCallback), connection);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            Connection  connection = ar.AsyncState as Connection;

            int bytesRead = connection.Socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                connection.AppendBuffer(bytesRead);

                if (bytesRead >= Connection.BufferSize)
                {
                    connection.Socket.BeginReceive(connection.WorkingBuffer, 0, Connection.BufferSize, 0,
                    new AsyncCallback(ReadCallback), connection);
                }
                else
                {
                    Command command = connection.MemoryBuilder.ToArray().FromByteArray<Command>();

                    PreProcessRequest(command, connection.Socket);
                    ProcessRequest(command, connection.Socket);
                    PostProcessRequest(command, connection.Socket);

                    if (command.OperationType != Enums.OperationType.ByeMessage)
                    {
                        connection.ClearBuffer();
                        connection.Socket.BeginReceive(connection.WorkingBuffer, 0, Connection.BufferSize, 0,
                            new AsyncCallback(ReadCallback), connection);
                    }
                }
            }
        }

        protected virtual void PreProcessRequest(Command command, Socket socket)
        {
            Logger.Instance.Log("Received: " + command.OperationType.ToString());
        }

        protected abstract void ProcessRequest(Command command, Socket socket);

        protected virtual void PostProcessRequest(Command command, Socket socket)
        {
            if (command.OperationType == Enums.OperationType.ByeMessage)
            {
                _connections.RemoveAll(c => c.Socket.Equals(socket));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        protected void Send(Command command, Socket socket)
        {
            byte[] byteData = command.ToByteArray();
            socket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;                
                int bytesSent = handler.EndSend(ar);
                Logger.Instance.Log("Bytes sent: " + bytesSent);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }
    }
}
