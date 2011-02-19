using System.ComponentModel;
using System.Windows.Input;
using CostCounter.Model;
using System;

namespace CostCounter.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private TimeKeeper _keeper = new TimeKeeper(new Clock(), 60);

        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new Command(() => _keeper.Start(), () => !_keeper.IsRunning)); }
        }

        private ICommand _stopCommand;
        public ICommand StopCommand
        {
            get { return _stopCommand ?? (_stopCommand = new Command(() => _keeper.Stop(), () => _keeper.IsRunning)); }
        }
    }

    public class Command : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Command(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public event System.EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}