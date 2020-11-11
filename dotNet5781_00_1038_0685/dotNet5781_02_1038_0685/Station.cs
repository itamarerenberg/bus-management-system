using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    public struct Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        static public bool operator ==(Point a, Point b)
        {
            return ((a.Latitude == b.Latitude) && (a.Longitude == b.Longitude));
        }
        static public bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
    class Station
    {
        private static List<Station> AllStations;
        private const int SIXDIGITS = 1000000;
        private const int MIN_LAT = -90;
        private const int MAX_LAT = 90;
        private const int MIN_LON = -180;
        private const int MAX_LON = 180;
        private int stationCode;
        public int StationCode
        {
            get => this.stationCode;
            protected set
            {
                if (value < 0 || value > SIXDIGITS)
                {
                    throw new ArgumentException("unvalid id");
                }
                else
                {
                    this.stationCode = value;
                }
            }
        }

        private Point loc;
        protected Point Loc
        {
            get => loc;
            set
            {
                if(value.Latitude < -90 || value.Longitude < -180 || value.Latitude > 90 || value.Longitude > 180)
                {
                    throw new ArgumentException(string.Format("latitude must be between {0} and {1}, longitude must be between {2} and {3}", MIN_LAT, MAX_LAT, MIN_LON, MAX_LON));
                }
                else
                {
                    loc = value;
                }
            }
        }

        protected string Address { get; set; }

        public Station(int code, double latitude, double longitude, string address = "")
        {
            Station temp_st = AllStations.Find((Station st) => st.StationCode == code);
            if (temp_st == null)
            {
                StationCode = code;
                Loc = new Point { Latitude = latitude, Longitude = longitude };//*Point is astruct
                Address = address;
            }
            this = temp_st;
            //else if(temp_st.loc != loc{ latitude, longitude})
        }

        public override string ToString()
        {
            return string.Format("Station code: {0}, {1}°N {2}°E", stationCode, loc.Latitude, loc.Longitude);
        }

        public override bool Equals(Object s)//posibol error change to int
        {
            if (s is Station)
            {
                if (((Station)s).StationCode == this.StationCode)
                {
                    return true;
                }
            }
            if(s is int)
            {
                if ((int)s == this.StationCode)
                {
                    return true;
                }
            }
            return true;
        }
    }
}
