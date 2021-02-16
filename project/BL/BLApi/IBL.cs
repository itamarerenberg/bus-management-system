using System;
using System.Collections.Generic;
using BL.simulator;
using BO;

namespace BLApi
{
    public interface IBL
    {

        #region Manager
        /// <summary>
        /// generates a user with Manager permission
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddManagar(string name, string password);
        /// <summary>
        /// looks for a user with Manager permission
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>
        /// <br>true: if a user with Manager permission exist.</br>  <br>the user doesn't exist or he doesn't have a Manager permission </br>
        /// </returns>
        bool GetManagar(string name, string password);
        /// <summary>
        /// update a user with Manager permission
        /// </summary>
        void UpdateManagar(string name, string password, string oldName, string oldPassword);
        /// <summary>
        /// Delete a user with Manager permission
        /// </summary>
        void DeleteManagar(string name, string password);

        #endregion

        #region Passenger
        /// <summary>
        /// generates a user with Passenger permission
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddPassenger(string name, string password);
        /// <summary>
        /// looks for a user with passenger permission  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>
        /// the passenger with his trips history
        /// </returns>
        Passenger GetPassenger(string name, string password);
        /// <summary>
        /// update a user with Passenger permission
        /// </summary>
        void UpdatePassenger(string name, string password, string newName, string newPassword);
        /// <summary>
        /// Delete a user with Passenger permission
        /// </summary>
        void DeletePassenger(string name, string password);
        /// <summary>
        /// Calculates the Times of Departure Lines from the selected station
        /// </summary>
        /// <returns>
        /// List of TimeTrips that contains "StartTime" and "Line number"
        /// </returns>
        List<TimeTrip> CalculateTimeTrip(LineStation lineStation, int lineNum, List<LineTrip> lineTrips);

        #endregion

        #region Bus
        void AddBus(Bus bus);

        /// <summary>
        /// get the bus by the license Number
        /// </summary>
        Bus GetBus(string licensNum);
        void UpdateBus(Bus bus);
        void DeleteBus(string licensNum);
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> pred);
        void Ride(Bus bus, float km);
        void Refuel(Bus bus);
        void Treatment(Bus bus);
        void AddRandomBus();
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
        IEnumerable<LineStation> GetAllLineStations();

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
        /// <summary>
        /// <br>remove the station and all</br>
        /// <br>the line stations of the station from all lines</br>
        /// <br>and all the adjecent stations with the deleding station</br>
        /// </summary>
        /// <param name="code"></param>
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
        List<Ride> GetRides(LineTrip lineTrip);
        #endregion

        #region simulator
        /// <summary>
        /// start the simulatorClock and the travel executer
        /// </summary>
        /// <param name="startTime">the time wich the simolator clock will start from</param>
        /// <param name="Rate">the rate of the simulator clock relative to real time</param>
        /// <param name="updateTime">will executet when the simulator time changes</param>
        /// /// <exception cref="IligalRateExeption">if rate is less then 1</exception>
        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime, Action<LineTiming> updateBus);

        /// <summary>
        /// stops the simulator clock and the travels executer and all the travels that in progres
        /// </summary>
        void StopSimulator();

        /// <summary>
        /// adds the station to the list of the stations that under truck
        /// </summary>
        void Add_stationPanel(int stationCode);

        /// <summary>
        /// removes the station from the list of the stations that under truck
        /// </summary>
        void Remove_stationPanel(int stationCode);

        /// <summary>
        /// <br>changes the rate of the simulator's clock</br>
        /// <br>for speed up insert positive number for 'change'</br>
        /// <br>for slow down insert negative number for 'change'</br>
        /// </summary>
        /// <param name="change">adds to the current rate of the simulator's clock</param>
        /// <exception cref="IligalRateExeption">if the change will make rate to be less the 1</exception>
        void Change_SimulatorRate(int change);

        #endregion

    }
}
