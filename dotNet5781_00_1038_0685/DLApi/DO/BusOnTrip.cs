using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
/// <summary>
/// identity property = ID
/// </summary>
    public class BusOnTrip
    {
        public int ID { get; set; }
        public string LicenseNum { get; set; }
        public int LineNum { get; set; }
        public TimeSpan PlannedStartTime{ get; set; }
        public TimeSpan StartTime { get; set; }
        public int PrevStation { get; set; }
        public TimeSpan TimePrevStation { get; set; }
        public TimeSpan TimeFromTheNextStation { get; set; }
        public bool IsActive { get; set; }

    }
}
