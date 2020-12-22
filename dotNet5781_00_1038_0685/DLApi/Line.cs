using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Line
    {
        public int ID { get; set; }
        public int LineNumber { get; set; }
        public AreasEnum Area { get; set; }
        public int FirstStationCode { get; set; }
        public int LastStationCode { get; set; }
    }
}
