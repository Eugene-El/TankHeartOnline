using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankHeartOnlineClient.Mvvm.Common;
using System.Windows.Input;
using TankHeartOnlineClient.Logic;
using System.Net;

namespace TankHeartOnlineClient.Mvvm.Models
{
    public class ConfigurationModel : NotifyBase
    {
        public ConfigurationModel()
        {
            var configuration = ConfigurationManager.Instance.GetConfiguration();
            Ip = configuration.Ip;
            Port = configuration.Port;
        }

        // Proporties
        private string _ip = "";
        public string Ip {
            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        private string _port = "";
        public string Port {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
        //

        public void SaveLocalAddress()
        {
             Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault().ToString();
        }

        public void SaveConfiguration()
        {
            ConfigurationManager.Instance.SetConfiguration(
                new Data.Configuration()
                {
                    Ip = Ip,
                    Port = Port
                });
            GameLobbyModel.Instance.Status = "Configuration saved";
        }
    }
}
