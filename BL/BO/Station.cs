using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        public int Code { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Address { get; set; }
        public GeoCoordinate Location { get => new GeoCoordinate(Longitude, Latitude); }
        public List<Line> GetLines { get; set; }
    }
}
