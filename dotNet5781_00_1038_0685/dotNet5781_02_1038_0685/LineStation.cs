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
        public double Distance { get ; set; }

        public TimeSpan RideTime { get { return new TimeSpan(0, (int)Distance, 0); } set { }}

        //construtcor
        public LineStation(int code, double latiude, double longitude, double distance, TimeSpan rideTime, string address = "")
            : base(code, latiude, longitude, address)
        {
            Distance = distance;
            RideTime = rideTime;
        }
        //copy construcrot
        public LineStation(LineStation other)
            : base(other.StationCode, other.Loc.Latitude, other.Loc.Longitude, other.Address)
        {
            Distance = other.Distance;
            RideTime = other.RideTime;
        }
    }
}
