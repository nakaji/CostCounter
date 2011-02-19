using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
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

        private TimeKeeper _keeper;

        public MainWindowViewModel()
        {
            _keeper = new TimeKeeper(new Clock(), 60);
        }

        #region Property
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _costPerHour;
        public int CostPerHour
        {
            get { return _costPerHour; }
            set
            {
                _costPerHour = value;
                OnPropertyChanged("CostPerHour");
            }
        }

        public List<Participant> Participants
        {
            get { return _keeper.Participants; }
        }


        public string TotalCost
        {
            get { return string.Format("{0}", _keeper.TotalCost); }
        }
        #endregion

        private Command _startCommand;
        public ICommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new Command(() => _keeper.Start(), () => !_keeper.IsRunning)); }
        }

        private Command _stopCommand;
        public ICommand StopCommand
        {
            get { return _stopCommand ?? (_stopCommand = new Command(() => _keeper.Stop(), () => _keeper.IsRunning)); }
        }

        private Command _addParticipantpCommand;
        public ICommand AddParticipantCommand
        {
            get
            {
                return _addParticipantpCommand ?? (_addParticipantpCommand = new Command(
                    () =>
                    {
                        _keeper.AddMenber(new Participant(_name, _costPerHour));
                        OnPropertyChanged("Participants");
                    },
                    () => true)
                  );
            }
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

        public void Execute(object parameter)
        {
            _execute();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}