using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
        public int ID { get; set; }
        public int LineNumber { get; set; }
        public AreasEnum Area { get; set; }
        public LineStation FirstStation { get => Stations.First(); }
        public LineStation LastStation { get => Stations.Last(); }
        public List<LineStation> Stations { get; set; }
        public List<LineTrip> LineTrips { get; set; }
    }
}
