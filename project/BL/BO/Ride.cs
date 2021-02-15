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
    public class Ride
    {
        public int LineId { get; set; }
        public int LineTripId { get; set; }
        public TimeSpan StartTime { get; set; }

        public override string ToString()
        {
            return $" Line ID:{LineId} start time: {StartTime}";
        }
    }
     
}
