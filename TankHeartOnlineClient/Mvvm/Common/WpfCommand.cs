using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TankHeartOnlineClient.Mvvm.Common
{
    public class WpfCommand : ICommand
    {
        public WpfCommand(Action action, bool canExecute = true)
        {
            _action = action;
            CanExecuteCommand = canExecute;
        }

        protected Action _action;

        public bool CanExecuteCommand { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action();
        }
    }
}
