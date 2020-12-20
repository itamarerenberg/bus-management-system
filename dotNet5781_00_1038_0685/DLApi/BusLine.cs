using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    internal class BusLine
    {
        private static int Counter = 0;
        public int LineNumber { get; set; }
        public AreasEnum Area { get; set; }
        public int LineId = Counter++;
        public int FirstStationCode { get; set; }
        public int LastStationCode { get; set; }
    }
}
