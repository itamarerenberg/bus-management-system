using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Pasenger
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<UserTrip> UserTrips { get; set; }
    }
}
