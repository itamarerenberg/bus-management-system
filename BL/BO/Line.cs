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
        public Station FirstStation { get; set; }
        public Station LastStation { get; set; }
        public List<Station> Stations { get; set; }
    }
}
