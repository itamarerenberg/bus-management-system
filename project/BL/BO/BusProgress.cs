using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum Activities
    {
        Refuling,
        InTrartment,
        Traveling,
        Prepering_to_ride
    }
    public class BusProgress
    {
        public string BusLicensNum { get; set; }
        public float Progress { get; set; }
        public Activities Activity { get; set; }
    }
}
