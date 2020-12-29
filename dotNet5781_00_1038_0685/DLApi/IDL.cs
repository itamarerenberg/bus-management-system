using System;
using System.Collections.Generic;

using DO;

namespace DLApi
{
    public interface IDL
    {
        #region Bus
        /// <summary>
        /// Add DO bus
        /// </summary>
        /// <exception cref="DuplicateExeption">
        /// </exception>
        void AddBus(Bus bus);
        /// <summary>
        /// Get DO bus
        /// </summary>
        /// <exception cref="NotExistExeption">
        /// </exception>
        Bus GetBus(string licenseNum);
        /// <summary>
        /// Update DO bus
        /// </summary>
        /// <exception cref="NotExistExeption">
        /// </exception>
        void UpdateBus(Bus bus);
        /// <summary>
        /// Delete DO bus
        /// </summary>
        /// <exception cref="NotExistExeption">
        /// </exception>
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
        void DeleteBusOnTrip(int id);
        IEnumerable<BusOnTrip> GetAllBusesOnTrip();
        IEnumerable<BusOnTrip> GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate);
        #endregion

        #region Station
        /// <exception cref="DuplicateExeption">if the Station all ready exist</exception>
        void AddStation(Station busStation);
        Station GetStation(int code);
        void UpdateStation(Station busStation);
        IEnumerable<Station> GetAllStations();
        IEnumerable<Station> GetAllStationBy(Predicate<Station> predicate);
        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        LineStation GetLineStation(int lineId, int stationNum);
        LineStation GetLineStationByIndex(int lineId, int index);
        void UpdateLineStation(LineStation lineStation);
        IEnumerable<LineStation> GetAllLineStations();
        IEnumerable<LineStation> GetAllLineStationBy(Predicate<LineStation> predicate);
        #endregion

        #region AdjacentStations
        void AddAdjacentStations(AdjacentStations adjacentStations);
        AdjacentStations GetAdjacentStation(int stationCode1, int stationCode2);
        AdjacentStations GetBackAdjacentStation(int stationCode);
        AdjacentStations GetAheadAdjacentStation(int stationCode);
        void UpdateAdjacentStations(AdjacentStations adjacentStations);
        void DeleteAdjacentStations(AdjacentStations adjacentStations);
        #endregion

        #region LineTrip
        void AddLineTrip(LineTrip lineTrip);
        LineTrip GetLineTrip(int id);
        void UpdateLineTrip(LineTrip lineTrip);
        IEnumerable<LineTrip> GetAllLineTrips();
        IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate);
        #endregion

        #region User
        void AddUser(User user);
        User GetUser(string name, string password);
        void UpdateUser(User user);
        void DeleteUser(string name);
        #endregion

        #region UserTrip
        void AddUserTrip(UserTrip userTrip);
        UserTrip GetUserTrip(int id);
        void UpdateUserTrip(UserTrip userTrip);
        IEnumerable<UserTrip> GetAllUserTrips();
        IEnumerable<UserTrip> GetAllUserTripsBy(Predicate<UserTrip> predicate);
        #endregion

        #region Generic
        //void Add<T>(T type);
        //T Get<T>(string id);
        //AdjacentStations Get(string id1, string id2);
        //void Update<T>(T type);
        //void Delete<T>(int id);
        //IEnumerable<T> GetAll<T>();
        //IEnumerable<T> GetAllBy<T>(Predicate<T> predicate);
        #endregion
    }
}
