using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    class LineTiming
    {
        public int LineId { get; set; }
        public int lineNum { get; set; }
        public TimeSpan StatrtTime { get; set; }
        public string LastStation { get; set; }
        public int StationCode { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}
