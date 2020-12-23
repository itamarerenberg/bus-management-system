using System;
using System.Collections.Generic;

using DO;

namespace DLApi
{
    public interface IDL
    {
        #region Bus
        void AddBuss(Bus bus);
        Bus GetBus(string licenseNum);
        void UpdateBus(Bus bus);
        void DeleteBus(string licenseNum);
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);
        #endregion

        #region Line
        void AddLine(Line line);
        Line GetLine(int id);
        void UpdateLine(Line Line);
        void DeleteLine(int id);
        IEnumerable<Line> GetAllLines();
        IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate);

        #endregion

        #region BusOnTrip
        void AddBusOnTrip(BusOnTrip busOnTrip);
        BusOnTrip GetBusOnTrip(int id);
        void UpdateBusOnTrip(BusOnTrip busOnTrip);
        IEnumerable<BusOnTrip> GetAllBusesOnTrip();
        IEnumerable<BusOnTrip> GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate);
        #endregion

        #region BusStation
        BusStation AddBusStation(BusStation busStation);
        void GetBusStation(int id);
        void UpdateBusStation(BusStation busStation);
        IEnumerable<BusStation> GetAllBusStations();
        IEnumerable<BusStation> GetAllBusStationBy(Predicate<BusStation> predicate);
        #endregion

        #region LineStation
        LineStation AddLineStation(LineStation iineStation);
        void GetLineStation(int id);
        void UpdateLineStation(LineStation iineStation);
        IEnumerable<LineStation> GetAllLineStations();
        IEnumerable<LineStation> GetAllLineStationBy(Predicate<LineStation> predicate);
        #endregion

        #region AdjacentStations
        AdjacentStations AddAdjacentStations(int stationCode1, int stationCode2);
        void GetBackAdjacentStation(int stationCode);
        void GetAheadAdjacentStation(int stationCode);
        void UpdateAdjacentStations(int stationCode1, int stationCode2);
        #endregion

        #region LineTrip
        LineTrip AddLineTrip(LineTrip lineTrip);
        void GetLineTrip(int id);
        void UpdateLineTrip(LineTrip lineTrip);
        IEnumerable<LineTrip> GetAllLineTrips();
        IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate);
        #endregion

        #region User
        User AddUser(User user);
        void GetUser(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
        #endregion

        #region UserTrip
        UserTrip AddUserTrip(UserTrip userTrip);
        void GetUserTrip(int id);
        void UpdateUserTrip(UserTrip userTrip);
        IEnumerable<UserTrip> GetAllUserTrips();
        #endregion

    }
}
