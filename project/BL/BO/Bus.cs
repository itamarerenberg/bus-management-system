using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus 
    {
        public readonly static int max_km_without_tratment = 20000;
        public readonly static TimeSpan max_time_without_tratment = new TimeSpan(365, 0, 0, 0);//one year
        public readonly static int min_fule_befor_warning = 100;

        public string LicenseNumber { get; set; }
        public DateTime LicenesDate { get; set; }
        public double Kilometraz { get; set; }
        public float Fuel { get; set; }
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
