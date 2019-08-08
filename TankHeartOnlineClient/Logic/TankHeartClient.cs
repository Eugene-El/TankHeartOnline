using CommandNetworking.Data;
using CommandNetworking.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TankHeartOnlineClient.Logic
{
    public class TankHeartClient : Client
    {
        public TankHeartClient(string ip, int port) : base(ip, port) { }

        protected override void ProcessRequest(Command command, Socket socket)
        {
            
        }
    }
}
