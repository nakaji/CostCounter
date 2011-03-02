using System;
using CostCounter.ViewModel;

namespace CostCounter.Model
{
    public class Participant : ModelBase
    {
        private bool _isRunning;
        private DateTime _lastNotifiedTime;

        #region Property
        public string Name { get; private set; }
        public int CostPerHour { get; private set; }
        public DateTime StartTime { get; private set; }
        public TimeSpan Elaps { get; private set; }

        private int _cost;
        public int Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }
        #endregion

        public Participant(string name, int costPerHour)
        {
            Name = name;
            CostPerHour = costPerHour;
        }

        public void Start(DateTime start)
        {
            if (!_isRunning)
            {
                if (StartTime == DateTime.MinValue)
                {
                    StartTime = start;
                }
                _lastNotifiedTime = start;
                _isRunning = true;
            }
        }

        public void Notify(DateTime dateTime)
        {
            if (_isRunning)
            {
                Elaps += dateTime - _lastNotifiedTime;
                _lastNotifiedTime = dateTime;
                CalcCost();
            }
            else
            {
                Start(dateTime);
            }
        }

        public void Stop(DateTime dateTime)
        {
            if (_isRunning)
            {
                Elaps += dateTime - _lastNotifiedTime;
                _lastNotifiedTime = dateTime;
                _isRunning = false;
                CalcCost();
            }
        }

        private void CalcCost()
        {
            Cost = (int)(CostPerHour * Elaps.TotalHours);
        }
    }
}
