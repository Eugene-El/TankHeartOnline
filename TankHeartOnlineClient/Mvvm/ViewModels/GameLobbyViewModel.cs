using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankHeartOnlineClient.Mvvm.Models;

namespace TankHeartOnlineClient.Mvvm.ViewModels
{
    public class GameLobbyViewModel : INotifyPropertyChanged
    {
        // Mvvm property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //

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
