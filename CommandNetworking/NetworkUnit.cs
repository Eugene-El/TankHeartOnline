using System.Net.Sockets;

namespace CommandNetworking
{
    internal class NetworkUnit
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
