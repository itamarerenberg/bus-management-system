using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BosOnTrip
    {
        public int MyProperty { get; set; }

        public string LicenseNum { get; set; }
        public int LineNum { get; set; }
        public TimeSpan PlannedStartTime{ get; set; }
        public TimeSpan StartTime { get; set; }
        public int LastPassedStation { get; set; }
        public TimeSpan TimeLastPassedStation { get; set; }
        public TimeSpan TimeFromTheNextStation { get; set; }

    }
}
