﻿using System;
using System.Collections.Generic;
using BL.BO;
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

        /// <summary>
        /// get the bus with this licensNum
        /// </summary>
        Bus GetBus(string licensNum);
        void UpdateBus(Bus bus);
        void DeleteBus(string licensNum);
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> pred);
        #endregion

        #region Line
        /// <returns>the Serial number that given to the new line at the data layer</returns>
        int AddLine(Line line, IEnumerable<Station> stations, List<int?> distances, List<int?> Times);
        Line GetLine(int id);
        void UpdateLine(int lineId, IEnumerable<Station> stations, List<int?> distances, List<int?> times);
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
        /// <summary>
        /// adds new station to the data base
        /// </summary>
        /// <param name="station">the station to add</param>
        /// <exception cref="LocationOutOfRange">if the location of the station is not in the allowable range</exception>
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
        void DeleteLineTrip(LineTrip lineTrip);
        IEnumerable<LineTrip> GetAllLineTrips();
        IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate);

        #endregion

        #region simulator
        /// <summary>
        /// start the simulatorClock and the travel executer
        /// </summary>
        /// <param name="startTime">the time wich the simolator clock will start from</param>
        /// <param name="Rate">the rate of the simulator clock relative to real time</param>
        /// <param name="updateTime">will executet when the simulator time changes</param>
        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime);

        /// <summary>
        /// stops the simulator clock and the travels executer and all the travels that in progres
        /// </summary>
        void StopSimulator();

        /// <summary>
        /// adds the station to the list of the stations that under truck
        /// </summary>
        void Add_stationPanel(int stationCode, Action<LineTiming> updateBus);

        /// <summary>
        /// removes the station from the list of the stations that under truck
        /// </summary>
        void Remove_stationPanel(int stationCode);

        #endregion

    }
}
