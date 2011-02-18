﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostCounter.Model;
using System.Threading;

namespace CostCounter
{
    public class TimeKeeper
    {
        private List<Participant> _participants;
        private IClock _clock;
        private Timer _timer; 

        public TimeKeeper(IClock clock, int interval = 60)
        {
            _participants = new List<Participant>();
            _clock = clock;
            _timer = new Timer(Notify, null, -1, interval*1000);
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
        }
        
        public void Notify(object nothing=null)
        {
            _participants.ForEach(x => x.Notify(_clock.Now));
        }
    }
}