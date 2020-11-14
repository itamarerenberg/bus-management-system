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
    public struct Station_in_use
    {
        public int Code { get; set; }
        public Point Loc { get; set; }
        public int Count;
    }
    public class Station
    {
        #region static lists
        private static List<int> usedcodes = new List<int>();
        private static List<Station_in_use> Stations_rep;
        #endregion

        #region CONSTANTS
        private const int SIXDIGITS = 1000000;
        private const int MIN_LAT = -90;
        private const int MAX_LAT = 90;
        private const int MIN_LON = -180;
        private const int MAX_LON = 180;
        #endregion

        #region private fields
        private int stationCode;
        #endregion

        #region propertys
        public int StationCode
        {
            get => this.stationCode;
            protected set
            {
                if(usedcodes.Contains(value))
                {
                    throw new ArgumentException("this code is already taken");
                }
                if (value < 0 || value > SIXDIGITS)
                {
                    throw new ArgumentException("invalid id");
                }
                else
                {
                    this.stationCode = value;
                    usedcodes.Add(this.stationCode);
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
        #endregion

        public Station(int code, double latitude, double longitude, string address = "")
        {
            StationCode = code;
            Loc = new Point { Latitude = latitude, Longitude = longitude };//*Point is astruct
            Address = address;
            //this = temp_st;
            //else if(temp_st.loc != loc{ latitude, longitude})
        }

        ~Station()
        {
            int index = Stations_rep.FindIndex((Station_in_use siu) => siu.Code == StationCode);
            if(Stations_rep[index].Count <= 1)
            {
                Stations_rep.RemoveAt(index);
            }
            else
            {
                int code = Stations_rep[index].Code;
                Point loc= Stations_rep[index].Loc;
                int count = Stations_rep[index].Count - 1;
                Stations_rep.RemoveAt(index);
                //Stations_rep.Add((Station_in_use));

            }
        }
        public override string ToString()
        {
            return string.Format("Station code: {0}, {1}°N {2}°E", stationCode, loc.Latitude, loc.Longitude);
        }

    }
}
