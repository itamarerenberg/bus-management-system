using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = TripId
    /// </summary>
    public class UserTrip
    {
        public int TripId { get; set; }
        public string UserName { get; set; }
        public int LineId { get; set; }
        public int InStation { get; set; }
        public DateTime InTime { get; set; }
        public int OutStation { get; set; }
        public DateTime OutTime { get; set; }

    }
}
