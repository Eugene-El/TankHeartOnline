using CommandNetworking.Logic;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using TankHeartOnlineClient.Logic;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var configuration = ConfigurationManager.Instance.GetConfiguration();
            if (string.IsNullOrEmpty(configuration.Ip) || string.IsNullOrEmpty(configuration.Port))
                StateLabel.Content = "Configuration not setted";
        }

        private void DisconnectLbl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ConfigurationLbl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Uri("Pages/ConfigurationPage.xaml", UriKind.Relative));
        }

        //private void GetLobbiesBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    new Client(ipTxt.Text).StartClient();
        //}

    }
}
