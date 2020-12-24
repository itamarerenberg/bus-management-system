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
    public enum BusStatusEnum
    {
        Traveling,
        Ready,
        In_treatment,
        In_refueling
    }
    public enum Entites 
    {
        AdjacentStations = -1,
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
