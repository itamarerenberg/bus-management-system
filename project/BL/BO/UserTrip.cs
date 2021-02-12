using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class UserTrip
    {
        public int TripId { get; set; }
        public string UserName { get; set; }
        public int LineId { get; set; }
        public int LineNum { get; set; }
        public int InStation { get; set; }
        public string InStationName { get; set; }
        public DateTime InTime { get; set; }
        public int OutStation { get; set; }
        public string OutStationName { get; set; }
        public DateTime OutTime { get; set; }
    }
}
