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
        /// <summary>
        /// the length of the line(in km)
        /// </summary>
        public double Length { get => LastStation.Distance_from_start; }
        /// <summary>
        /// the estemate time the that the rout of this line takes
        /// </summary>
        public TimeSpan Time { get => LastStation.Time_from_start; }
    }
}
