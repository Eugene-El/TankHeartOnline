using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TankHeartOnlineClient.Data;
using TankHeartOnlineClient.Logic;

namespace TankHeartOnlineClient.Pages
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        public ConfigurationPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var configuration = ConfigurationManager.Instance.GetConfiguration();
            IpBox.Text = configuration.Ip;
            PortBox.Text = configuration.Port;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.Instance.SetConfiguration(new Configuration()
            {
                Ip = IpBox.Text,
                Port = PortBox.Text
            });
        }

        private void SetLocalLbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IpBox.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault().ToString();
        }
    }
}
