using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
