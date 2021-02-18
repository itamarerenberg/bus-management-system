using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using SQLite;

namespace DS
{
    public static class DataSource
    {
        #region lists
        public static List<AdjacentStations> AdjacentStations = new List<AdjacentStations>();
        public static List<Bus> Buses = new List<Bus>();
        public static List<Line> Lines = new List<Line>();
        public static List<BusTrip> BuseTrips = new List<BusTrip>();
        public static List<Station> Stations = new List<Station>();
        public static List<LineStation> LineStations = new List<LineStation>();
        public static List<LineTrip> LineTrips = new List<LineTrip>();
        public static List<User> Users = new List<User>();
        public static List<UserTrip> UsersTrips = new List<UserTrip>();
        #endregion

        #region serial numbers
        static int serialLineID;
        static int serialLineTripID;
        static int serialUserTripID;

        public static int SerialLineID { get => serialLineID++; }
        public static int SerialLineTripID { get => serialLineTripID++; }
        public static int SerialUserTripID { get => serialUserTripID++; } 
        #endregion

        static DataSource()
        {
            serialLineID = 0;
            serialLineTripID = 0;
            serialUserTripID = 0;
        }
    }
}