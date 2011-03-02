using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Timer _timer;

        public MainWindowViewModel()
        {
            _keeper = new TimeKeeper(new Clock());
            _timer = new Timer(x =>
                                   {
                                       _keeper.Notify();
                                       OnPropertyChanged("TotalCost");
                                   }, null, -1, _interval * 1000);
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

        private int _interval;
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged("Interval");
            }
        }

        public ObservableCollection<Participant> Participants
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
            get
            {
                return _startCommand ?? (_startCommand = new Command(() =>
                                                                         {
                                                                             _keeper.Start();
                                                                             _timer.Change(0, _interval * 1000);
                                                                         }, () => !_keeper.IsRunning));
            }
        }

        private Command _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new Command(() =>
                                                                       {
                                                                           _keeper.Stop();
                                                                           _timer.Change(-1, _interval * 1000);
                                                                       }, () => _keeper.IsRunning));
            }
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
}