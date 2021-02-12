using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.Models.PO
{
    public class TimeTrip
    {
        public int LineNum { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }

        public override string ToString()
        {
            return $"Line:{LineNum} at {(StartTime.Days > 0? StartTime.ToString("d'.'hh':'mm"): StartTime.ToString("hh':'mm"))}"; 
        }
    }
}
