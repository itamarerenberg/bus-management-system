using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjastStations
    {
        public Station stationA { get; set; }
        public Station stationB { get; set; }
        public float dist_km { get; set; }
        public TimeSpan? from_A_to_B { get; set; }
        public TimeSpan? from_B_to_A { get; set; }
    }
}
