using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    public interface IBL
    {

        #region Manager
        void AddManagar(string name, string password);
        bool GetManagar(string name, string password);
        void UpdateManagar(string name, string password,string oldName, string oldPassword);
        void DeleteManagar(string name, string password);

        #endregion

        #region Passenger
        void AddPassenger(string name, string password);
        Passenger GetPassenger(string name, string password);
        void UpdatePassenger(string name, string password, string newName, string newPassword);
        void DeletePassenger(string name, string password);

        #endregion

        #region Bus
        void AddBus(Bus bus);
        Bus GetBus(string licensNum);
        void UpdateBus(Bus bus);
        void DeleteBus(string licensNum);
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> pred);
        #endregion

        #region Line
        int AddLine(Line line);
        Line GetLine(int id);
        void UpdateLine(Line line);
        void DeleteLine(int id);
        IEnumerable<Line> GetAllLines();
        IEnumerable<Line> GetAllLinesBy(Predicate<Line> pred);
        #endregion

        #region LineStation
        /// <summary>
        /// adding a new line station
        /// </summary>
        /// <param name="lineStation">line Station of type BO </param>
        /// <param name="index">the location in the line (push the rest of the stations 1 step ahead)</param>
        void AddLineStation(int id, int StationNumber, int index);
        void UpdateLineStation(int lineNumber, int StationNumber);
        void DeleteLineStation(int lineNumber, int StationNumber); 
        #endregion

        #region Station
        void AddStation(Station station);
        Station GetStation(int code);
        void UpdateStation(Station station);
        void DeleteStation(int code);
        IEnumerable<Station> GetAllStations();
        IEnumerable<Station> GetAllStationsBy(Predicate<Station> pred);
        #endregion

        #region User Trip
        void AddUserTrip(UserTrip userTrip);
        UserTrip GetUserTrip(int id);
        void UpdateUserTrip(UserTrip userTrip);
        void DeleteUserTrip( int id);
        IEnumerable<UserTrip> GetAllUserTrips(string name);
        IEnumerable<UserTrip> GetAllUserTripsBy(string name,Predicate<UserTrip> pred);
        #endregion

        #region Line trip
        void AddLineTrip(LineTrip lineTrip);
        LineTrip GetLineTrip(int id);
        void UpdateLineTrip(LineTrip lineTrip);
        IEnumerable<LineTrip> GetAllLineTrips();
        IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate);

        #endregion
    }
}
