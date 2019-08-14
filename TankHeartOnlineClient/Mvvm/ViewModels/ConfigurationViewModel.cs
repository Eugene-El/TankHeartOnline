using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TankHeartOnlineClient.Mvvm.Common;
using TankHeartOnlineClient.Mvvm.Models;

namespace TankHeartOnlineClient.Mvvm.ViewModels
{
    public class ConfigurationViewModel : NotifyBase
    {
        private ConfigurationModel _configurationModel;
        public ConfigurationViewModel()
        {
            _configurationModel = new ConfigurationModel();
            _configurationModel.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };

            SaveCommand = new WpfCommand(_configurationModel.SaveConfiguration);
            SetLocalAddressCommand = new WpfCommand(_configurationModel.SaveLocalAddress);
        }

        // Proporties
        public string Ip
        {
            get { return _configurationModel.Ip; }
            set
            {
                _configurationModel.Ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        public string Port
        {
            get { return _configurationModel.Port; }
            set
            {
                _configurationModel.Port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
        //


        // Commands
        public ICommand SaveCommand { get; protected set; }
        public ICommand SetLocalAddressCommand { get; protected set; }
        //
    }
}
