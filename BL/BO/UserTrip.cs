using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class UserTrip
    {
        public TimeSpan Ontime { get; set; }
        public TimeSpan Offtime { get; set; }
        public BusTrip Trip { get; set; }
    }
}
