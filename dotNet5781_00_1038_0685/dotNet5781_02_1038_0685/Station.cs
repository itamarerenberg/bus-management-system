using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    public class Station
    {
        #region static
        #region fildes
        public static List<Station> exists_stations = new List<Station>();
        #endregion

        #region fanctions
        public static Station get_station(int code)
        {
            Station temp = exists_stations.Find((Station st) => st.StationCode == code);
            if (temp == null)
            {
                throw new ArgumentException("this station does not exist");
            }
            return temp;
        }  
        #endregion

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
        private List<BusLine> pass_here;
        #endregion

        #region propertys
        public int StationCode
        {
            get => this.stationCode;
            private set
            {
                if (exists_stations.Exists((Station st) => st.StationCode == value))
                {
                    throw new ArgumentException("this code has already been taken");
                }
                if (value < 0 || value > SIXDIGITS)
                {
                    throw new ArgumentException("unvalid id");
                }
                else
                {
                    this.stationCode = value;
                    exists_stations.Add(this);
                }
            }
        }

        public List<BusLine> Pass_here
        {
            get => this.pass_here.FindAll(l => true);
            set => pass_here = value;
        }


        private LocPoint loc;
        public LocPoint Loc
        {
            get => loc;
            set
            {
                if (value.Latitude < -90 || value.Longitude < -180 || value.Latitude > 90 || value.Longitude > 180)
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

        #region constructor
        public Station(int code, double latitude, double longitude, string address = "")
        {
            StationCode = code;
            Loc = new LocPoint { Latitude = latitude, Longitude = longitude };//*LocPoint is astruct
            Address = address;
            pass_here = new List<BusLine>();
        }
        #endregion

        #region methods
        public void Add_line(BusLine bl)
        {
            pass_here.Add(bl);
        }

        public override string ToString()
        {
            return $"Station code: {stationCode,-10}{loc.Latitude + "°N",-23}{loc.Longitude + "°E",-23}";
        } 
        #endregion

    }
}
