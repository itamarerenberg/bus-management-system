using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    class LineStation : Station
    {
        //fields
        public double Distance { get; set; }

        public TimeSpan RideTime { get; set; }

        //construcor
        public LineStation(int code, double latiude, double longitude, double distance, TimeSpan rideTime, string address = "")
            : base(code, latiude, longitude, address)
        {
            Distance = distance;
            RideTime = rideTime;
        }
    }
}
