using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = ID
    /// </summary>
    public class LineTrip
    {
        public int ID { get; set; }
        public string LineId{ get; set; }
        public TimeSpan StartTime{ get; set; }
        public TimeSpan Frequency{ get; set; }
        public TimeSpan FinishAt { get; set; }
        public bool IsActive { get; set; }

    }
}
