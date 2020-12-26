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
        public LineStation FirstStation { get; set; }
        public LineStation LastStation { get; set; }
        public List<LineStation> Stations { get; set; }
    }
}
