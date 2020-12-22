using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public class DataSource
    {
        List<AdjacentStations> AdjacentStations;
        List<Bus> Buses;
        List<BusLine> BusLines;
        List<BusOnTrip> BusesOnTrip;
        List<BusStation> BusStations;
        List<LineStation> LineStations;
        List<LineTrip> LineTrips;
        List<User> Users;
        List<UserTrip> UsersTrips;

        internal DataSource()
        {
            InitAllLists();
        }

        private void InitAllLists()
        {
            AdjacentStations = new List<AdjacentStations>();
            Buses = new List<Bus>();
            BusLines = new List<BusLine>();
            BusesOnTrip = new List<BusOnTrip>();
            BusStations = new List<BusStation>();
            LineStations = new List<LineStation>();
            LineTrips = new List<LineTrip>();
            Users = new List<User>();
            UsersTrips = new List<UserTrip>();
        }
    }
}
