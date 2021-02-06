using System;
using System.Collections.Generic;
using System.Linq;
using DLApi;
using DLXML;
using DO;
using DS;
using System.Xml.Linq;
namespace DL
{
    sealed class DLObject : IDL
    {
        #region singelton

        static readonly DLObject instance = new DLObject();
        static DLObject() { }
        DLObject() { }
        public static DLObject Instance { get => instance; }

        #endregion


        #region Bus
        public void AddBus(Bus bus)
        {
            Bus Same_LicenseNum = (from b in DataSource.Buses
                                        where b.LicenseNumber == bus.LicenseNumber
                                        select b).FirstOrDefault();
            if (Same_LicenseNum == null)//if no bus have the same LicenseNum
            {
                DataSource.Buses.Add(bus.Clone());//add bus to label Buses
            }
            else//in case the bus is allready in the data base: checks if he is active
            {
                if (Same_LicenseNum.IsActive)//if the bus is activ
                {
                    throw new DuplicateExeption("bus with identical License's num allready exist");
                }
                Same_LicenseNum = bus.Clone();//if the 'Same_LicenseNum' is unactive then override it with the new bus
            }
        }

        public Bus GetBus(string licenseNum)
        {
            Bus bus = (from b in DataSource.Buses
                       where b.LicenseNumber == licenseNum
                       select b).FirstOrDefault();

            if (bus == null)//if the bus not found
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
            return bus.Clone(); 
        }

        public void UpdateBus(Bus newBus)
        {
            Bus oldBus = (from b in DataSource.Buses
                               where b.LicenseNumber == newBus.LicenseNumber
                               select b).FirstOrDefault();

            if (oldBus == null)//if the old bus dosn't found
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            oldBus = newBus.Clone();//set the old bus to be the update bus
        }

        public void DeleteBus(string licenseNum)
        {
            Bus bus = (from b in DataSource.Buses
                            where b.LicenseNumber == licenseNum && b.IsActive
                            select b.Clone()).FirstOrDefault();
            if (bus != null)//if ther is such bus
            {
                bus.IsActive = false;//set the bus to be unavtive
            }
            else
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            var ret = from b in DataSource.Buses
                      where b.IsActive
                      select b.Clone();
            return ret;
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from b in DataSource.Buses
                   where predicate(b) && b.IsActive
                   select b.Clone();
        }

        #endregion

        #region Line
        public int AddLine(Line line)
        {
            try
            {
                line.ID = DataSource.SerialLineID;
            }
            catch (Exception e)//!
            {
                throw e;
            }
            DataSource.Lines.Add(line.Clone());
            return line.ID;
        }

        public Line GetLine(int id)
        {
            Line line = (from l in DataSource.Lines
                             where l.ID == id &&
                                   l.IsActive
                             select l).FirstOrDefault();
            if (line == null)
            {
                throw new NotExistExeption("line with this id not exist");
            }
            return line.Clone();
        }

        public void UpdateLine(Line newLine)
        {
            Line oldLine = (from l in DataSource.Lines
                                where l.ID == newLine.ID 
                                      && l.IsActive
                                select l).FirstOrDefault();
            if (oldLine == null)//if the old line dont exist in the data source
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            oldLine = newLine.Clone();//override the old line with the new one
        }

        public void DeleteLine(int id)
        {
            Line line = (from l in DataSource.Lines
                             where l.ID == id
                                   && l.IsActive
                             select l).FirstOrDefault();
            if (line != null)
            {
                line.IsActive = false;//set the line to be unactive
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from l in DataSource.Lines
                   where l.IsActive
                   select l.Clone();
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return from l in DataSource.Lines
                   where l.IsActive && predicate(l)
                   select l.Clone();
        }

        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSource.BusesOnTrip.FirstOrDefault(bot => bot.ID == busOnTrip.ID) != null)//check if this trip exist allready
            {
                throw new DuplicateExeption("the bus is allready in driving");
            }
            else
            {
                DataSource.BusesOnTrip.Add(busOnTrip);//add to the data source
            }
        }

        public BusOnTrip GetBusOnTrip(int id)
        {
            BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == id);//serche for the trip in the data source
            if (busOnTrip == null)//if ther is no such trip in the data source
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            return busOnTrip.Clone();
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            BusOnTrip oldBusOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == busOnTrip.ID);//serch for the old trip in the data source
            if (oldBusOnTrip == null)//if not found
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            oldBusOnTrip = busOnTrip;//override the old trip with the new one
        }

        public void DeleteBusOnTrip(int id)
        {
            BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(l => l.ID == id && l.IsActive == true);//serch for the trip in the data source
            if (busOnTrip != null)//if found
            {
                busOnTrip.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        public IEnumerable<BusOnTrip> GetAllBusesOnTrip()
        {
            return from bot in DataSource.BusesOnTrip//return all the activ trips
                   where bot.IsActive == true
                   select bot.Clone();
        }

        public IEnumerable<BusOnTrip> GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        {
            return from bot in DataSource.BusesOnTrip//return all the trips that predicate return true for them
                   where predicate(bot)
                   select bot.Clone();
        }
        #endregion

        #region Station

        /// <summary>
        /// add station to the data source</br>
        /// if ther is station with same code DuplicateExeption will be throw
        /// </summary>
        /// <param name="busStation">station to add</param>
        public void AddStation(Station busStation)
        {
            Station tempBusStation = (from bs in DataSource.Stations//serch for such station in the data source
                                       where bs.Code == busStation.Code
                                             && bs.IsActive
                                       select bs).FirstOrDefault();      
            if (tempBusStation == null)//if not found
            {
                DataSource.Stations.Add(busStation.Clone());//add to the data source
            }
            else//in case the bus station is allready in the data base: checks if he is active
            {
                if (tempBusStation.IsActive)
                {
                    throw new DuplicateExeption("bus station with identical License's num allready exist");
                }
                tempBusStation = busStation.Clone();//override the unactiv station with the new one
            }
        }

        /// <summary>
        /// return a clone of the station with Code = 'code' in the data source</br>
        /// if ther is no such station NotExistExeption will be throw
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Station GetStation(int code)
        {
            Station busStation = (from bs in DataSource.Stations//serch for the station in the data source
                                   where bs.IsActive
                                         && bs.Code == code
                                   select bs).FirstOrDefault();
            if (busStation == null)//if the station not found
            {
                throw new NotExistExeption("bus's station with this code not exist");
            }
            return busStation.Clone();
        }

        /// <summary>
        /// override the station with the same code in the data sorce</br>
        /// if the old station dont exist NotExistExeption will be throw
        /// </summary>
        public void UpdateStation(Station newBusStation)
        {
            Station oldBusStation = (from bs in DataSource.Stations
                                      where bs.Code == newBusStation.Code
                                      select bs).FirstOrDefault();
            if (oldBusStation == null)
            {
                throw new NotExistExeption("the station doesn't not exist");
            }
            oldBusStation = newBusStation.Clone();//override the old station with the new one
        }

        /// <summary>
        /// unactivate the station with Code = 'code' from the data source</br>
        /// if the station dont exist or allready unactive NotExistExeption will be throw
        /// </summary>
        /// <param name="code"></param>
        public void DeleteStation(int code)
        {
            Station station = (from bs in DataSource.Stations
                               where bs.Code == code
                                     && bs.IsActive
                               select bs).FirstOrDefault();
            if (station != null)
            {
                station.IsActive = false;
            }
            else//if the station is not exist or allready unactiv
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
        }

        /// <summary>
        /// returns a clones of all the active stations
        /// </summary>
        public IEnumerable<Station> GetAllStations()
        {
            return from bs in DataSource.Stations//return all the active stations
                   where bs.IsActive
                   select bs.Clone();
        }

        /// <summary>
        /// return aclone of all the stations that predicate returns true for them
        /// </summary>
        public IEnumerable<Station> GetAllStationBy(Predicate<Station> predicate)
        {
            return from bs in DataSource.Stations//return all the stations that predicate returns true for them
                   where predicate(bs)
                   select bs.Clone();
        }
        #endregion

        #region LineStation

        /// <summary>
        /// add new line station to the data source.</br>
        /// if ther is allready line station with identical line id and station number:</br>
        /// if it active DuplicateExeption will be throw.</br>
        /// else, overrides the unactive line station with the new one
        /// </summary>
        /// <param name="lineStation">line station to add</param>
        public void AddLineStation(LineStation lineStation)
        {
            LineStation tempLineStation = (from ls in DataSource.LineStations
                                           where ls.LineId == lineStation.LineId
                                              && ls.StationNumber == lineStation.StationNumber
                                           select ls).FirstOrDefault();
            if (tempLineStation == null)//if ther no such line station in the data sorce allready
            {
                DataSource.LineStations.Add(lineStation.Clone());
            }
            //in case the Line station is allready in the data base: checks if he is active
            else
            {
                if (tempLineStation.IsActive)
                {
                    throw new DuplicateExeption("the line station is allready exist");
                }
                tempLineStation = lineStation.Clone();//override the old unactive lineStation with the new line station
            }
        }

        /// <summary>
        /// returns a clone of the line station in the data source with LineId = 'lineId' and StationNumber = 'stationNum'</br>
        /// if the line station dont exist NotExistExeption will be throw
        /// </summary>
        public LineStation GetLineStation(int lineId, int stationNum)
        {
            LineStation lineStation = (from ls in DataSource.LineStations//serch for the line station in the data source
                                       where ls.LineId == lineId
                                             && ls.StationNumber == stationNum
                                       select ls).FirstOrDefault();
            if (lineStation == null)//if the line station dont exist
            {
                throw new NotExistExeption("the line doesn't exist or it doesn't have such a station");
            }
            return lineStation.Clone();
        }

        /// <summary>
        /// returns a clone of line station with LineId == 'lineId' and LineStationIndex == 'index'
        /// if the line station dont exist NotExistExeption will be throw
        /// </summary>
        public LineStation GetLineStationByIndex(int lineId, int index)
        {
            LineStation lineStation = (from ls in DataSource.LineStations//serch for the line station in the data source
                                       where ls.LineId == lineId
                                             && ls.LineStationIndex == index
                                       select ls).FirstOrDefault();
            if (lineStation == null)//if such line station don't exist in the data source
            {
                throw new NotExistExeption("the line doesn't exist or the index is out of range");
            }
            return lineStation.Clone();
        }

        /// <summary>
        /// overrides the old line station with LineId = 'lineId' and StationNumber = 'stationNum' with the new line station</br>
        /// </summary>
        /// <param name="newLineStation">up to date line station</param>
        public void UpdateLineStation(LineStation newLineStation)
        {
            LineStation oldLineStation = (from ls in DataSource.LineStations
                                          where ls.LineId == newLineStation.LineId
                                                && ls.StationNumber == newLineStation.StationNumber
                                          select ls).FirstOrDefault();
            if (oldLineStation == null)//if the old Line Station dont exist in the data source
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            oldLineStation = newLineStation.Clone();//override the old lineStation
        }

        public void UpdateLineStation(Action<LineStation> action, int lineId, int stationNumber)
        {
            LineStation oldLineStation = (from ls in DataSource.LineStations
                                          where ls.LineId == lineId
                                                && ls.StationNumber == stationNumber
                                          select ls).FirstOrDefault();
            if (oldLineStation == null)//if the old Line Station dont exist in the data source
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            action(oldLineStation);//do 'action' on the old line station
        }

        /// <summary>
        /// <br>delete the line station with LineId = 'lineId' and StationNumber = 'stationNum'</br>
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="stationNum"></param>
        public void DeleteLineStation(int lineId, int stationNum)
        {
            DataSource.LineStations.RemoveAll(ls => ls.LineId == lineId && ls.StationNumber == stationNum);
        }

        /// <summary>
        /// returns a clone of all the line stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return DataSource.LineStations.Select(ls => ls.Clone());
        }

        /// <summary>
        /// return a clone of all the line station that predicate returns tru for them
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            return from ls in DataSource.LineStations
                   where predicate(ls)
                   select ls.Clone();
        }
        #endregion

        #region AdjacentStations
        /// <summary>
        /// <br>add a new adjacent stations to the data source</br>
        /// </summary>
        /// <param name="adjacentStations">new adjacent stations to add</param>
        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            if (adjacentStations == null)
                return;
            AdjacentStations tempStations = (from adjSt in DataSource.AdjacentStations
                                             where adjSt.StationCode1 == adjacentStations.StationCode1
                                                   && adjSt.StationCode2 == adjacentStations.StationCode2
                                             select adjSt).FirstOrDefault();
            if (tempStations == null)//if ther is no such AdjacentStations
            {
                DataSource.AdjacentStations.Add(tempStations.Clone());
            }            
            //else do nothing (it's not an exeption) 
        }

        /// <summary>
        /// returns a clone of the adjacent stations with StationCode1 = 'stationCode1' and StationCode2 = 'stationCode2'
        /// </summary>
        public AdjacentStations GetAdjacentStation(int? stationCode1, int? stationCode2)
        {
            AdjacentStations tempStations = (from adjSt in DataSource.AdjacentStations
                                             where adjSt.StationCode1 == stationCode1
                                             && adjSt.StationCode2 == stationCode2
                                             select adjSt.Clone()).FirstOrDefault();
            return tempStations;
        }

        /// <summary>
        /// overrids old adjacent stations with same stationCode1 and stationCode2
        /// </summary>
        /// <param name="newAdjacentStations">up to date station</param>
        public void UpdateAdjacentStations(AdjacentStations newAdjacentStations)
        {
            AdjacentStations oldAdjacentStations = (from adjSt in DataSource.AdjacentStations
                                                    where adjSt.StationCode1 == newAdjacentStations.StationCode1
                                                          && adjSt.StationCode2 == newAdjacentStations.StationCode2
                                                    select adjSt).FirstOrDefault();
            if (oldAdjacentStations == null)
            {
                throw new NotExistExeption("the Adjacent Stations doesn't exist");
            }
            oldAdjacentStations = newAdjacentStations.Clone();//override the old AdjacentStations with the up to date one
        }

        /// <returns>
        /// <br>true: if the adjecent stations was in the data source and deleted succefuly</br>
        /// <br>false: otherwais </br>
        /// </returns>
        public bool DeleteAdjacentStations(int? stationCode1, int? stationCode2)
        {
            //if ther is such AdjacentStations in the data source then the method 'RemoveAll()' returns more then 0
            return (0 < DataSource.AdjacentStations.RemoveAll(adjst => adjst.StationCode1 == stationCode1 && adjst.StationCode2 == stationCode2));
        }

        /// <returns>true: if the object deleted sucsesfuly, false: if the object dont exist or allready not active</returns>
        public bool DeleteAdjacentStations(AdjacentStations adjacentStations)
        {
            AdjacentStations temp = (from adjSt in DataSource.AdjacentStations//extracting the object from the data base
                             where adjSt.StationCode1 == adjacentStations.StationCode1
                                   && adjSt.StationCode2 == adjacentStations.StationCode2
                             select adjSt).FirstOrDefault();

            if(temp == null)//if this object dont exist
            {
                return false;
            }

            DataSource.AdjacentStations.Remove(temp);
            return true;
        }

        ///<returns>a clones of all the objects where predicate returns true for them(active and not active)</returns>
        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adjSt in DataSource.AdjacentStations
                   where predicate(adjSt)
                   select adjSt.Clone();
        }
        

        #endregion

        #region LineTrip

        /// <summary>
        /// add new line trip to the data source
        /// </summary>
        /// <param name="lineTrip">new line trip to add</param>
        /// <returns>the serial number of this line trip</returns>
        public int AddLineTrip(LineTrip lineTrip)
        {
            lineTrip.ID = DataSource.SerialLineTripID;
            DataSource.LineTrips.Add(lineTrip.Clone());
            return lineTrip.ID;
        }

        /// <summary>
        /// <br>returns a clone of a line trip withe ID = 'id' from the data source</br>
        /// <br>if such line trip dont exist NotExistExeption will be throw</br>
        /// </summary>
        public LineTrip GetLineTrip(int id)
        {
            LineTrip temp = (from lt in DataSource.LineTrips//serch for the line trip in the data source
                             where lt.ID == id
                             select lt).FirstOrDefault();
            if(temp == null)//if not found
            {
                throw new NotExistExeption("the Line trip doesn't exist");
            }
            return temp.Clone();
        }

        /// <summary>
        /// <br></br>overrides the old line trip with same id</br>
        /// <br>if such line trip dont exist NotExistExeption will be throw</br>
        /// </summary>
        /// <param name="newLineTrip"></param>
        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            LineTrip oldLineTrip = (from lt in DataSource.LineTrips//serch for the old line trip in the data source
                                    where lt.ID == newLineTrip.ID
                                    select lt).FirstOrDefault();
                
            if (oldLineTrip == null)//if not found
            {
                throw new NotExistExeption("the line trip doesn't exist");
            }
            oldLineTrip = newLineTrip.Clone();//override the old line trip with the new one
        }

        public void DeleteLineTrip(LineTrip lineTrip)
        {
            LineTrip lineT = (from l in DataSource.LineTrips
                         where l.ID == lineTrip.ID
                               && l.LineId == lineTrip.LineId
                               && l.IsActive
                         select l).FirstOrDefault();
            if (lineT != null)
            {
                lineT.IsActive = false;//set the line trip to be unactive
            }
            else
            {
                throw new NotExistExeption("the line Trip doesn't exist");
            }
        }


        /// <summary>
        /// returns a clones of all the active line trips in the data source
        /// </summary>
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from lt in DataSource.LineTrips
                   where lt.IsActive
                   select lt.Clone();
        }

        /// <summary>
        /// returns a clones of all the line trips that predicate return true for them
        /// </summary>
        public IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            return from lt in DataSource.LineTrips
                   where predicate(lt)
                   select lt.Clone();
        }
        #endregion

        #region User

        /// <summary>
        /// <br>adds new user to data source</br>
        /// <br>if ther is allready unactive user with the same name then the new user will override th unactive one</br>
        /// </summary>
        /// <param name="user">new user to add</param>
        /// <exception cref="DuplicateExeption">if ther is an active user with the same Name allready in the data source</exception>
        public void AddUser(User user)
        {
            User tempUser = DataSource.Users.FirstOrDefault(l => l.Name == user.Name);//serch for user with the same name
            if (tempUser == null)//if not found
            {
                DataSource.Users.Add(user.Clone());
            }
            //in case the user is allready in the data base: checks if he is active
            else
            {
                if (tempUser.IsActive == true)
                {
                    throw new DuplicateExeption("the user is allready exist");
                }
                tempUser = user.Clone();//override the unactive user
            }
        }

        /// <summary>
        /// returns a clone of the user with Name = 'name'
        /// </summary>
        /// <exception cref="NotExistExeption">if the user dont exist or un active</exception>
        public User GetUser(string name)
        {
            User tempUser = DataSource.Users.FirstOrDefault(l => l.Name == name && l.IsActive == true);
            if (tempUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            return tempUser.Clone();
        }

        /// <summary>
        /// overrides the user with the same name in the data source with the up to date user
        /// </summary>
        /// <exception cref="NotExistExeption">if ther is no such user in the data source</exception>
        public void UpdateUser(User newUser)
        {
            User oldUser = DataSource.Users.Find(l => l.Name == newUser.Name);
            if (oldUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            oldUser = newUser.Clone();
        }

        /// <summary>
        /// unactivate the user in the data source with Name = 'name'
        /// </summary>
        /// <exception cref="NotExistExeption">if ther is no such user in the data source or the user is all ready unactive</exception>
        public void DeleteUser(string name)
        {
            User user = DataSource.Users.Find(l => l.Name == name && l.IsActive == true);
            if (user != null)
            {
                user.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("the user doesn't exist");
            }
        }
        #endregion

        #region UserTrip

        public int AddUserTrip(UserTrip userTrip)
        {
            userTrip.TripId = DataSource.SerialUserTripID;//get a serial number from SerialNumbers for the id
            DataSource.UsersTrips.Add(userTrip.Clone());//add to the data source
            return userTrip.TripId;
        }

        public UserTrip GetUserTrip(int id)
        {
            UserTrip tempUserTrip = DataSource.UsersTrips.FirstOrDefault(u => u.TripId == id && u.IsActive == true);
            if (tempUserTrip == null)
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            return tempUserTrip.Clone();
        }

        public void UpdateUserTrip(UserTrip newUserTrip)
        {
            UserTrip oldUserTrip = DataSource.UsersTrips.Find(u => u.TripId == newUserTrip.TripId && u.IsActive == true);
            if (oldUserTrip == null)
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            oldUserTrip = newUserTrip.Clone();
        }

        public IEnumerable<UserTrip> GetAllUserTrips()
        {
            return from userTrip in DataSource.UsersTrips
                   where userTrip.IsActive == true
                   select userTrip.Clone();
        }

        public IEnumerable<UserTrip> GetAllUserTripsBy(Predicate<UserTrip> predicate)
        {
            return from userTrip in DataSource.UsersTrips
                   where predicate(userTrip) && userTrip.IsActive == true
                   select userTrip.Clone();
        }

        #endregion
    }
}


