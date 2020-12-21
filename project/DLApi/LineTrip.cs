using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLApi
{
    class LineTrip
    {
        public string LineId{ get; set; }
        public TimeSpan StartTime{ get; set; }
        public TimeSpan Frequency{ get; set; }
        public TimeSpan FinishAt { get; set; }

    }
}
