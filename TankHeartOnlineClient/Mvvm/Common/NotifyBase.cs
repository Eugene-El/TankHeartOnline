using System.ComponentModel;

namespace TankHeartOnlineClient.Mvvm.Common
{
    public class NotifyBase : INotifyPropertyChanged
    {
        // Mvvm property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //
    }
}
