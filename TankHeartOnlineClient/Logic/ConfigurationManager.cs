using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankHeartOnlineClient.Data;

namespace TankHeartOnlineClient.Logic
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _configurationManager;
        public static ConfigurationManager Instance {
            get
            {
                if (_configurationManager == null)
                    _configurationManager = new ConfigurationManager();
                return _configurationManager;
            }
        }
        private ConfigurationManager() { }

        public Configuration GetConfiguration()
        {
            var configuration = FileManager.Instance.ReadFile<Configuration>("configuration.bin");
            return configuration == null ? new Configuration() : configuration;
        }

        public void SetConfiguration(Configuration configuration)
        {
            FileManager.Instance.WriteFile("configuration.bin", configuration);
        }

    }
}
