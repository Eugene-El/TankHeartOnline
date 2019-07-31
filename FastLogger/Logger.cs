using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastLogger
{
    public class Logger
    {
        private static Logger _logger;
        public static Logger Instance
        {
            get
            {
                if (_logger == null)
                    _logger = new Logger();
                return _logger;
            }   
        }
        private Logger() { }

        public void Log(string message)
        {
            if (!message.EndsWith("\n"))
                message += "\n";
            using (StreamWriter writer = File.AppendText("logs.txt"))
            {
                writer.Write("[{0}] {1}", DateTime.UtcNow.ToShortDateString()
                    + " " + DateTime.UtcNow.ToShortTimeString(), message);
            }
        }
    }
}
