using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using CostCounter.Model;

namespace CostCounter.ViewModel
{
    public class MainWindowViewModel : ModelBase
    {
        private readonly TimeKeeper _keeper;
        private readonly Timer _timer;

        public MainWindowViewModel()
        {
            _keeper = new TimeKeeper(new Clock());
            _timer = new Timer(x =>
                                   {
                                       _keeper.Notify();
                                       TotalCost = _keeper.TotalCost;
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

        private long _totalCost = 0;
        public long TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
                OnPropertyChanged("TotalCost");
            }
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
                                                                           TotalCost = _keeper.TotalCost;
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
                        Name = "";
                        OnPropertyChanged("Participants");
                    },
                    () => true)
                  );
            }
        }
    }
}