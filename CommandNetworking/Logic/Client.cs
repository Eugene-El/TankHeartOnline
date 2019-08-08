using CommandNetworking.Data;
using CommandNetworking.Extensions;
using FastLogger;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommandNetworking.Logic
{
    public abstract class Client
    {
        protected int _port;
        protected string _ip;

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        public Client(string ip, int port = 11001)
        {
            _ip = ip;
            _port = port;
        }

        public void StartClient()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(_ip);
                IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault();
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, _port);

                Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();


                Send(client, new Command() { OperationType = Enums.OperationType.HelloMessage });
                sendDone.WaitOne();

                while (true)
                {
                    Receive(client);
                    receiveDone.WaitOne();
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Logger.Instance.Log(string.Format("Socket connected to {0}",
                    client.RemoteEndPoint.ToString()));

                connectDone.Set();
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                Connection connection = new Connection(client);

                client.BeginReceive(connection.WorkingBuffer, 0, Connection.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), connection);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Connection connection = ar.AsyncState as Connection;

                int bytesRead = connection.Socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    connection.AppendBuffer(bytesRead);

                    if (bytesRead >= Connection.BufferSize)
                    {
                        connection.Socket.BeginReceive(connection.WorkingBuffer, 0, Connection.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), connection);
                    }
                    else
                    {
                        Command command = connection.MemoryBuilder.ToArray().FromByteArray<Command>();
                        receiveDone.Set();

                        PreProcessRequest(command, connection.Socket);
                        ProcessRequest(command, connection.Socket);
                        PostProcessRequest(command, connection.Socket);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }

        protected virtual void PreProcessRequest(Command command, Socket socket)
        {
            Logger.Instance.Log("Received: " + command.OperationType.ToString());
        }

        protected abstract void ProcessRequest(Command command, Socket socket);

        protected virtual void PostProcessRequest(Command command, Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void Send(Socket client, Command command)
        {
            byte[] byteData = command.ToByteArray();

            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Logger.Instance.Log(string.Format("Sent {0} bytes to server.", bytesSent));
 
                sendDone.Set();
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }
    }
}
