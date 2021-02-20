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
        /// <summary>
        /// <br>when the activity is finished this fild will be true</br>
        /// <br>the defult value is false</br>
        /// </summary>
        public bool FinishedFlag { get; set; } = false;
    }
}
