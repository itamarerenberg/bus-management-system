using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    public interface IBL
    {

        #region Manager
        void AddManagar(string name, string password);
        Manager GetManagar(string name, string password);
        void UpdateManagar(string name, string password,string oldName, string oldPassword);
        void DeleteManagar(string name, string password);

        #endregion

        #region Passenger
        void AddPassenger(string name, string password);
        Passenger GetPassenger(string name, string password);
        void UpdatePassenger(string name, string password);
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
        void AddLine(Line line);
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
        /// <param name="index">the location in the line (push the rest of the stations 1 step ahead). default = to the end of the line</param>
        void AddLineStation(LineStation lineStation, int index);
        void UpdateLineStation(LineStation lineStation, int index);
        void DeleteLineStation(LineStation lineStation, int index); 
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
        Line GetUserTrip(int id);
        void UpdateUserTrip(UserTrip userTrip);
        void DeleteUserTrip( int id);
        IEnumerable<UserTrip> GetAllUserTrips();
        IEnumerable<UserTrip> GetAllUserTripsBy(Predicate<UserTrip> pred);
        #endregion
    }
}
