using CommandNetworking.Data;
using CommandNetworking.Enums;
using CommandNetworking.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TankHeartOnlineConsoleServer
{
    public class TankHeartServer : Server
    {
        protected override void ProcessRequest(Command command, Socket socket)
        {
            if (command.OperationType == OperationType.DataMessage)
            {
                Console.WriteLine("Data received");
            }
        }
    }
}
