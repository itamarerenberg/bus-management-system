using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus 
    {
        public string LicenseNumber { get; set; }
        public DateTime LicenesDate { get; set; }
        public double Kilometraz { get; set; }
        public float Fule { get; set; }
        public BusStatus Stat { get; set; }
        public  double KmAfterTreat { get; set; }
        public DateTime LastTreatDate { get; set; }
        public TimeSpan TimeUntilReady { get; set; }
        public bool IsBusy
        {
            get
            {
                if (this.Stat == BusStatus.In_refueling || this.Stat == BusStatus.In_treatment || this.Stat == BusStatus.Traveling)
                    return true;
                return false;
            }
        }
    }
}
