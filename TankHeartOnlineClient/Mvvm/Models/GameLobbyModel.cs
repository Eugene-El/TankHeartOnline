﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankHeartOnlineClient.Mvvm.Models
{
    public class GameLobbyModel : INotifyPropertyChanged
    {
        // Mvvm property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //

        // Singleton
        private static GameLobbyModel _instance;
        public static GameLobbyModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameLobbyModel();
                return _instance;
            }
        }
        private GameLobbyModel() {
            Status = "Loading...";
        }
        //

        // Properties
        private string _status;
        public string Status {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        //
    }
}
