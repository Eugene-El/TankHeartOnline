using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankHeartOnlineClient.Mvvm.Common;
using TankHeartOnlineClient.Mvvm.Models;

namespace TankHeartOnlineClient.Mvvm.ViewModels
{
    public class GameLobbyViewModel : NotifyBase
    {
        // Model connection
        public GameLobbyViewModel()
        {
            GameLobbyModel.Instance.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };
        }
        //

        // Properties
        public string Status
        {
            get { return GameLobbyModel.Instance.Status; }
        }
        //

    }
}
