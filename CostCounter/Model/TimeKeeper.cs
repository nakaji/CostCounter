using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace CostCounter.Model
{
    public class TimeKeeper 
    {
        private ObservableCollection<Participant> _participants;
        private Clock _clock;

        public TimeKeeper(Clock clock)
        {
            _participants = new ObservableCollection<Participant>();
            _clock = clock;
            IsRunning = false;
        }

        public ObservableCollection<Participant> Participants
        {
            get { return _participants; }
            private set { _participants = value; }
        }

        public long TotalCost
        {
            get { return _participants.Sum(x => x.Cost); }
        }

        public void AddMenber(Participant participant)
        {
            _participants.Add(participant);
        }

        public void Start()
        {
            foreach (var x in _participants)
            {
                x.Start(_clock.Now);
            }
            IsRunning = true;
        }
        
        public void Notify()
        {
            foreach (var x in _participants)
            {
                x.Notify(_clock.Now);
            }
        }

        public void Stop()
        {
            foreach (var x in _participants)
            {
                x.Stop(_clock.Now);
            }
            IsRunning = false;
        }

        public bool IsRunning { get; private set; }
    }
}