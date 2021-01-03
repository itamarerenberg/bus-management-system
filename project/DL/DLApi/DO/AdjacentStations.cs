using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = StationCode1|StationCode2
    /// for exempol if StationCode1 = 123 and StationCode2 = 456 then 
    /// the identity property = 123456
    /// </summary>
    public class AdjacentStations
    {
        public int StationCode1{ get; set; }
        public int StationCode2 { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsActive { get; set; }

    }
}
