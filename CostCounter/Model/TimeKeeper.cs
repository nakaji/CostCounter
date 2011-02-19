using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CostCounter.Model
{
    public class TimeKeeper
    {
        private List<Participant> _participants;
        private Clock _clock;
        private Timer _timer; 

        public TimeKeeper(Clock clock, int interval = 60)
        {
            _participants = new List<Participant>();
            _clock = clock;
            _timer = new Timer(Notify, null, -1, interval*1000);
            IsRunning = false;
        }

        public List<Participant> Participants
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
            _participants.ForEach(x => x.Start(_clock.Now));
            IsRunning = true;
        }
        
        public void Notify(object nothing=null)
        {
            _participants.ForEach(x => x.Notify(_clock.Now));
        }

        public void Stop()
        {
            _participants.ForEach(x => x.Stop(_clock.Now));
            IsRunning = false;
        }

        public bool IsRunning { get; private set; }
    }
}