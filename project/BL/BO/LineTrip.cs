using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public int ID { get; set; }
        public int LineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Frequency { get; set; }
        public TimeSpan FinishAt { get; set; }
        /// <summary>
        /// the length of the trip (in km)
        /// </summary>
        public double Length { get; set; }
    }
}
