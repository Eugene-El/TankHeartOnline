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
using TankHeartOnlineClient.Mvvm.ViewModels;

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

        private void SetLocalLbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var context = DataContext as ConfigurationViewModel;
            if (context != null)
            {
                context.SetLocalAddressCommand.Execute(null);
            }
        }
    }
}
