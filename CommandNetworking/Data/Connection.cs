using CommandNetworking.Extensions;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace CommandNetworking.Data
{
    public class Connection
    {
        public static int BufferSize = 1024;

        public Socket Socket { get; private set; }
        public MemoryStream MemoryBuilder { get; private set; }
        public byte[] WorkingBuffer { get; set; }

        public Connection(Socket socket)
        {
            Socket = socket;
            MemoryBuilder = new MemoryStream();
            WorkingBuffer = new byte[BufferSize];
        }

        public void AppendBuffer(int bytesCount)
        {
            MemoryBuilder.Append(WorkingBuffer.Take(bytesCount).ToArray());
        }

        public void ClearBuffer()
        {
            MemoryBuilder.Close();
            MemoryBuilder.Dispose();
            MemoryBuilder = new MemoryStream();
        }
    }
}
