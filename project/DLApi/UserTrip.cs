using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class UserTrip
    {
        static int Counter = 0;
        public int TripId = Counter++;
        public string UserName { get; set; }
        public int LineId { get; set; }
        public int InStation { get; set; }
        public DateTime InTime { get; set; }
        public int OutStation { get; set; }
        public DateTime OutTime { get; set; }

    }
}
