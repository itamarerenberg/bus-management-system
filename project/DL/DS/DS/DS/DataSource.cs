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
        public static List<Station> Stations;
        public static List<LineStation> LineStations;
        public static List<LineTrip> LineTrips;
        public static List<User> Users;
        public static List<UserTrip> UsersTrips;

        public static int serialLineID;

        static DataSource()
        {
            serialLineID = 0;

            AdjacentStations = new List<AdjacentStations>();
            Buses = new List<Bus>();
            Lines = new List<Line>();
            BusesOnTrip = new List<BusOnTrip>();
            Stations = new List<Station>();
            LineStations = new List<LineStation>();
            LineTrips = new List<LineTrip>();
            Users = new List<User>();
            UsersTrips = new List<UserTrip>(); InitAllLists();
         }

        static void InitAllLists()
        {
           //init list with data
            
        }
    }
}
