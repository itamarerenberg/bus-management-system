using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BLApi;
using BL.BO;
using BL.simulator;
using BLApi;
using BO;
using DLApi;

namespace BL
{
    public class BLImpPassenger : IBL
    {
        #region singelton
        static readonly BLImpPassenger instance = new BLImpPassenger();
        static BLImpPassenger() { }
        BLImpPassenger() { }
        public static BLImpPassenger Instance { get => instance; }
        #endregion

        IDL dl = DLFactory.GetDL();

        #region Passenger

        public Passenger GetPassenger(string name, string password)
        {
            try
            {
                //validation
                DO.User user = dl.GetUser(name);
                if (user.Password != password)
                {
                    throw new InvalidPassword("invalid password");
                }
                Passenger passenger = (Passenger)user.CopyPropertiesToNew(typeof(Passenger));
                passenger.UserTrips = (from tripDO in dl.GetAllUserTripsBy(t => t.UserName == name)
                                       orderby tripDO.InTime
                                       select GetUserTrip(tripDO.TripId)).ToList();
                return passenger;
            }
            catch (DO.InvalidObjectExeption)
            {
                throw new InvalidID("invalid name");
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        public void AddPassenger(string name, string password)
        {
            DO.User user = new DO.User()
            {
                Name = name,
                Password = password,
                Admin = false,
                IsActive = true
            };
            dl.AddUser(user);
        }
        public void UpdatePassenger(string oldName, string oldPassword, string newName, string newPassword)
        {
            try
            {
                DO.User oldManagar = dl.GetUser(oldName);
                if (oldManagar.Password != oldPassword)
                    throw new InvalidInput("the old password is incorect");

                DO.User newManagar = new DO.User()
                {
                    Name = newName,
                    Password = newPassword,
                };
                dl.UpdateUser(newManagar);
            }
            catch (DO.NotExistExeption)
            {
                throw new InvalidPassword("the name is invalid");
            }
            catch (InvalidInput msg)
            {
                throw msg;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        public void DeletePassenger(string name, string password)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region User Trip
        public void AddUserTrip(UserTrip userTrip)
        {
            dl.AddUserTrip((DO.UserTrip)userTrip.CopyPropertiesToNew(typeof(DO.UserTrip)));
        }

        public UserTrip GetUserTrip(int id)
        {
            return (UserTrip)dl.GetUserTrip(id).CopyPropertiesToNew(typeof(UserTrip));
        }

        public void UpdateUserTrip(UserTrip userTrip)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserTrip(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserTrip> GetAllUserTrips(string name)
        {
            return from tripDO in dl.GetAllUserTrips()
                   where tripDO.UserName == name
                   orderby tripDO.InTime
                   select GetUserTrip(tripDO.TripId);
        }

        public IEnumerable<UserTrip> GetAllUserTripsBy(string name,Predicate<UserTrip> pred)
        {
            return from tripDO in dl.GetAllUserTrips()
                   where tripDO.UserName == name
                   let tripBO = GetUserTrip(tripDO.TripId)
                   where pred(tripBO)
                   orderby tripBO.InTime
                   select tripBO;

        }
        #endregion

        #region Bus

        public Bus GetBus(string licensNum)
        {
            try
            {
                return (Bus)dl.GetBus(licensNum).CopyPropertiesToNew(typeof(Bus));
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public IEnumerable<Bus> GetAllBuses()
        {
            return from BObus in dl.GetAllBuses()
                   select (Bus)BObus.CopyPropertiesToNew(typeof(Bus));
        }
        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> pred)
        {
            return from BObus in dl.GetAllBuses()
                   let bus = (Bus)BObus.CopyPropertiesToNew(typeof(Bus))
                   where pred(bus)
                   select bus;
        }
        #endregion

        #region Line

        public Line GetLine(int id)
        {
            try
            {
                Line line = (Line)dl.GetLine(id).CopyPropertiesToNew(typeof(Line));
                line.Stations = (from lineStationDO in dl.GetAllLineStationsBy(l => l.LineId == id)
                                 let lineStationBO = HelpMethods.GetLineStation(lineStationDO.LineId, lineStationDO.StationNumber)
                                 orderby lineStationBO.LineStationIndex
                                 select lineStationBO).ToList();
                return line;
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public IEnumerable<Line> GetAllLines()
        {
            try
            {
                return from lineDO in dl.GetAllLines()
                       select GetLine(lineDO.ID);
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> pred)
        {
            try
            {
                return from lineDO in dl.GetAllLines()
                       let lineBO = GetLine(lineDO.ID)
                       where pred(lineBO)
                       select lineBO;
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        #endregion

        #region Station       
        public Station GetStation(int code)
        {
            try
            {
                Station station = (Station)dl.GetStation(code).CopyPropertiesToNew(typeof(Station));
                station.LinesNums = (from lineNum in dl.GetAllLineStationsBy(s => s.LineId == code)
                                     orderby lineNum.LineId
                                     select lineNum.LineId).ToList();
                return station;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }      
        public IEnumerable<Station> GetAllStations()
        {
            return (from stationDO in dl.GetAllStations()
                    select (Station)stationDO.CopyPropertiesToNew(typeof(Station)));
        }
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> pred)
        {
            return from stationDO in dl.GetAllStations()
                   let stationBO = GetStation(stationDO.Code)
                   where pred(stationBO)
                   select stationBO;
        }

        #endregion

        #region Line trip

        public LineTrip GetLineTrip(int id)
        {
            try
            {
                return (LineTrip)dl.GetLineTrip(id).CopyPropertiesToNew(typeof(LineTrip));
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from LTripDO in dl.GetAllLineTrips()
                   select (LineTrip)LTripDO.CopyPropertiesToNew(typeof(LineTrip));
        }
        public IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            return from LTripDO in dl.GetAllLineTrips()
                   let LTripBO = (LineTrip)LTripDO.CopyPropertiesToNew(typeof(LineTrip))
                   where predicate(LTripBO)
                   select LTripBO;
        }

        #endregion

        #region not implement
        public void AddManagar(string name, string password)
        {
            throw new NotImplementedException();
        }

        public bool GetManagar(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateManagar(string name, string password, string oldName, string oldPassword)
        {
            throw new NotImplementedException();
        }

        public void DeleteManagar(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void AddBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(string licensNum)
        {
            throw new NotImplementedException();
        }

        public int AddLine(Line line, IEnumerable<Station> stations, List<int?> distances, List<int?> Times)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(int lineId, IEnumerable<Station> stations, List<int?> distances, List<int?> times)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int id)
        {
            throw new NotImplementedException();
        }

        public void AddLineStation(int lineNumber, int StationNumber, int index)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStation(int lineNumber, int StationNumber)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStation(int lineNumber, int StationNumber)
        {
            throw new NotImplementedException();
        }

        public void AddStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int code)
        {
            throw new NotImplementedException();
        }
        public void AddLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }
        public void UpdateLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }
        public void DeleteLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        void IBL.Ride(Bus bus, float km)
        {
            throw new NotImplementedException();
        }

        void IBL.Refuel(Bus bus)
        {
            throw new NotImplementedException();
        }

        void IBL.Treatment(Bus bus)
        {
            throw new NotImplementedException();
        }

        void IBL.AddRandomBus()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region simulator
        SimulationClock clock = SimulationClock.Instance;
        TravelsExecuter travelsExecuter = TravelsExecuter.Instance;
        /// <summary>
        /// start the simulatorClock and the travel executer
        /// </summary>
        /// <param name="startTime">the time wich the simolator clock will start from</param>
        /// <param name="Rate">the rate of the simulator clock relative to real time</param>
        /// <param name="updateTime">will executet when the simulator time changes</param>
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            clock.StartClock(startTime, rate, updateTime);
            travelsExecuter.StartExecute();
        }

        /// <summary>
        /// stops the simulator clock and the travels executer and all the travels that in progres
        /// </summary>
        public void StopSimulator()
        {
            clock.StopClock();//this stops the travels executer too
        }

        /// <summary>
        /// adds the station to the list of the stations that under truck
        /// </summary>
        public void Add_stationPanel(int stationCode, Action<LineTiming> updateBus)
        {
            travelsExecuter.Add_station_to_truck(stationCode);
        }

        /// <summary>
        /// removes the station from the list of the stations that under truck
        /// </summary>
        public void Remove_stationPanel(int stationCode)
        {
            travelsExecuter.Add_station_to_truck(stationCode);
        }

        public void Change_SimulatorRate(int change)
        {
            clock.Change_Rate(change);
        }

        #endregion
    }
}
