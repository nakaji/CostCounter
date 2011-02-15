using System;

namespace CostCounter.Model
{
    public class Participant
    {
        private bool _isRunning;
        private DateTime _lastNotifiedTime;

        public string Name { get; private set; }
        public int Cost { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime NotifiedTime { get; private set; }
        public TimeSpan Elaps { get; private set; }

        public Participant(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public void Start(DateTime start)
        {
            if (!_isRunning)
            {
                StartTime = start;
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
            }
        }

        public void Stop(DateTime dateTime)
        {
            if (_isRunning)
            {
                Elaps += dateTime - _lastNotifiedTime;
                _lastNotifiedTime = dateTime;
                _isRunning = false;
            }
        }
    }
}
