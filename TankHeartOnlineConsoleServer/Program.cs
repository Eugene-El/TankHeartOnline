using CommandNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankHeartOnlineConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new TankHeartServer().StartListening();
        }
    }
}
