using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum RideStatus
    {
        prepering,
        in_motion
    }
    public class LineTiming
    {
        public int LineId { get; set; }
        public int LineNum { get; set; }
        /// <summary>
        ///the time that the trip of this lineTiming executed
        /// </summary>
        public TimeSpan StartTime { get; set; }
        public string LastStation { get; set; }
        public int StationCode { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string BusLicensNumber { get; set; }
        /// <summary>
        /// representing the percentage that the bus has been pass in the ride
        /// </summary>
        public RideStatus Status { get; set; }
    }
}
