using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusTrip
    {
        public int ID { get; set; }
        public string Bus_Id { get; set; }
        public int LineId { get; set; }
        public int LineNum { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }

    }
}
