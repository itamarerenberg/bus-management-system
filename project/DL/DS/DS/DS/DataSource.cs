using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public static class DataSource
    {
        public static List<AdjacentStations> AdjacentStations;
        public static List<Bus> Buses;
        public static List<Line> Lines;
        public static List<BusOnTrip> BusesOnTrip;
        public static List<BusStation> BusStations;
        public static List<LineStation> LineStations;
        public static List<LineTrip> LineTrips;
        public static List<User> Users;
        public static List<UserTrip> UsersTrips;

        public static List<object> dsList;


        static DataSource()
        {
            InitAllLists();
        }

        static void InitAllLists()
        {
            AdjacentStations = new List<AdjacentStations>();
            Buses = new List<Bus>();
            Lines = new List<Line>();
            BusesOnTrip = new List<BusOnTrip>();
            BusStations = new List<BusStation>();
            LineStations = new List<LineStation>();
            LineTrips = new List<LineTrip>();
            Users = new List<User>();
            UsersTrips = new List<UserTrip>();
            
            dsList = new List<object>()
            {
            AdjacentStations,
            Buses,
            Lines,
            BusesOnTrip,
            BusStations,
            LineStations,
            LineTrips,
            Users,
            UsersTrips
            };
        }
    }
}
