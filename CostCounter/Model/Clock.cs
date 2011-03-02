using System;

namespace CostCounter.Model
{
    public class Clock
    {
        public virtual DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
