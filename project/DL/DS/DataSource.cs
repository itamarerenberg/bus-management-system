using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using SQLite;

namespace DS
{
    public static class DataSource
    {
        public static List<AdjacentStations> AdjacentStations = new List<AdjacentStations>();
        public static List<Bus> Buses = new List<Bus>();
        public static List<Line> Lines = new List<Line>();
        public static List<BusOnTrip> BusesOnTrip = new List<BusOnTrip>();
        public static List<Station> Stations = new List<Station>();
        public static List<LineStation> LineStations = new List<LineStation>();
        public static List<LineTrip> LineTrips = new List<LineTrip>();
        public static List<User> Users = new List<User>();
        public static List<UserTrip> UsersTrips = new List<UserTrip>();

        public static int serialLineID;
        static string DataBasePath = AppDomain.CurrentDomain.BaseDirectory;
        static DataSource()
        {
            serialLineID = 0;
            InitAllLists();
        }

        static void InitAllLists()
        {
            string StationsPath = System.IO.Path.Combine(DataBasePath, "Stations.db");
            using (SQLiteConnection stations = new SQLiteConnection(StationsPath))
            {
                Stations = stations.Table<Station>().ToList();
            }
        }
    }
}