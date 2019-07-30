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
    public class Server
    {
        protected ManualResetEvent _allDone = new ManualResetEvent(false);
        protected int _port;

        public Server(int port = 11000)
        {
            _port = port;
        }

        public void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

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

        public void AcceptCallback(IAsyncResult ar)
        {
            _allDone.Set();
 
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            
            NetworkUnit networkUnit = new NetworkUnit();
            networkUnit.workSocket = handler;
            handler.BeginReceive(networkUnit.buffer, 0, NetworkUnit.BufferSize, 0,
                new AsyncCallback(ReadCallback), networkUnit);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            NetworkUnit networkUnit = (NetworkUnit)ar.AsyncState;
            Socket handler = networkUnit.workSocket;

            int bytesRead = handler.EndReceive(ar);
            StringBuilder sb = new StringBuilder();

            if (bytesRead > 0)
            {
                sb.Append(Encoding.ASCII.GetString(
                    networkUnit.buffer, 0, bytesRead));

                content = sb.ToString();

                if (content.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);



                    Send(handler, content);
                }
                else
                {
                    handler.BeginReceive(networkUnit.buffer, 0, NetworkUnit.BufferSize, 0,
                    new AsyncCallback(ReadCallback), networkUnit);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
