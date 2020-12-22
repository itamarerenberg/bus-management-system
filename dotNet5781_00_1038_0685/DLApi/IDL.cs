﻿using System;
using System.Collections.Generic;

using DO;

namespace DLApi
{
    public interface IDL
    {
        #region Bus
        Bus AddBuss(Bus bus);
        void GetBus(int id);
        void UpdateBus(Bus bus);
        void DeleteBus(int id);
        IEnumerable<Bus> GetAllBuses();
        #endregion

        #region Line
        Line AddBusLine(Line busLine);
        void GetBusLine(int id);
        void UpdateBusLine(Line busLine);
        void DeleteBusLine(int id);
        IEnumerable<Line> GetAllBusLines();
        IEnumerable<Line> GetAllBusLinesBy(Predicate<Line> predicate);

        #endregion

        #region BusOnTrip
        BusOnTrip AddBusOnTrip(BusOnTrip busOnTrip);
        void GetBusOnTrip(int id);
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
