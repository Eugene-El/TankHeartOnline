using FastLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandNetworking
{
    public class Client
    {
        protected int _port;
        protected string _ip;

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        public Client(string ip = "localhost", int port = 11001)
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





                // Send test data to the remote device.  
                Send(client, "This is a test<EOF>");
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();





                client.Shutdown(SocketShutdown.Both);
                client.Close();

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
                NetworkUnit networkUnit = new NetworkUnit();
                networkUnit.workSocket = client;

                client.BeginReceive(networkUnit.buffer, 0, NetworkUnit.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), networkUnit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                NetworkUnit networkUnit = (NetworkUnit)ar.AsyncState;
                Socket client = networkUnit.workSocket;

                int bytesRead = client.EndReceive(ar);

                StringBuilder sb = new StringBuilder();

                if (bytesRead > 0)
                {
                    sb.Append(Encoding.ASCII.GetString(networkUnit.buffer, 0, bytesRead));

                    client.BeginReceive(networkUnit.buffer, 0, NetworkUnit.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), networkUnit);
                }
                else
                {
                    if (sb.Length > 1)
                    {
                        var message = sb.ToString();
                    }
                    receiveDone.Set();
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
            }
        }

        private void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

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
