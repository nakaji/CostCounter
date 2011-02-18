using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CostCounter.Model
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
