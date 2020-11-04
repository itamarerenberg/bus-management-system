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
    }
    class Station
    {
        private const int SIXDIGITS = 1000000;
        private const int MIN_LAT = -90;
        private const int MAX_LAT = 90;
        private const int MIN_LON = -180;
        private const int MAX_LON = 180;
        private int id;
        public int Id
        {
            get => this.id;
            private set
            {
                if (0 < value || value > SIXDIGITS)
                {
                    throw new ArgumentException("unvalid id");
                }
                else
                {
                    this.id = value;
                }
            }
        }

        private Point loc;
        public Point Loc
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

        private string address { get; set; }
    }
}
