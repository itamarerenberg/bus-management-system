
using System.Collections.Generic;

namespace BO
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public List<UserTrip> UserTrips{ get; set; }
    }
}