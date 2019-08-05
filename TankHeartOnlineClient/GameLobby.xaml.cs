using CommandNetworking.Logic;
using System.Linq;
using System.Net;
using System.Windows;

namespace TankHeartOnlineClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameLobby : Window
    {
        public GameLobby()
        {
            InitializeComponent();
        }

        private void GetLobbiesBtn_Click(object sender, RoutedEventArgs e)
        {
            new Client(ipTxt.Text).StartClient();
        }

        private void SetLocalBtn_Click(object sender, RoutedEventArgs e)
        {
            ipTxt.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault().ToString();
        }
    }
}
