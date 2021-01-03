using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum AreasEnum
    {
        Jerusalem,
        TelAviv,
        Hifa,
        Rehovot,
        General
    }
    public enum BusStatus
    {
        Ready,
        Traveling,
        In_treatment,
        In_refueling,
        Need_treatment,
        Need_refueling
    }
    public enum Entites 
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
    }
}
