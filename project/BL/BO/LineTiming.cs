using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
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
    }
}
