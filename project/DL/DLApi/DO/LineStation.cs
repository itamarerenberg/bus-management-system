using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = LineId|StationNumber
    /// for exempol if LineId = 123 and StationNumber = 456 then 
    /// the identity property = 123456
    /// </summary>
    public class LineStation
    {
        public int LineId { get; set; }
        public int StationNumber { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public int LineStationIndex { get; set; }
        public int? PrevStation { get; set; }
        public int? NextStation { get; set; }
        public bool IsActive { get; set; }
    }
}
