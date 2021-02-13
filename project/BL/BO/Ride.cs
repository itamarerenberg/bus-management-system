using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// represents one ride
    /// </summary>
    class Ride
    {
        public int LineId { get; set; }
        public int LineTripId { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
