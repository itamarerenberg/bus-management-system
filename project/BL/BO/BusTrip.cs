using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusTrip
    {
        public int ID { get; set; }
        public Bus Bus { get; set; }
        public Line Line { get; set; }
        public TimeSpan PlannedStartTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public int PrevStation { get; set; }
        public TimeSpan TimePrevStation { get; set; }
        public TimeSpan TimeFromTheNextStation { get; set; }
        public int MyProperty { get; set; }
    }
}
