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
        public string Name { get; set; }
        public GeoCoordinate Location { get; set; }
        public double Longitude { get => Location.Longitude; set => Location.Longitude = value; }
        public double Latitude { get => Location.Latitude; set => Location.Latitude = value; }
        public string Address { get; set; }
        public List<int> LinesNums { get; set; }
    }
}
