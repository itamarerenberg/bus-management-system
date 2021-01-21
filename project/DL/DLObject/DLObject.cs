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
                                        where b.LicenseNum == bus.LicenseNum
                                        select b).FirstOrDefault();
            if (Same_LicenseNum == null)//if no bus have the same LicenseNum
            {
                DataSource.Buses.Add(bus);//add bus to label Buses
            }
            else//in case the bus is allready in the data base: checks if he is active
            {
                if (Same_LicenseNum.IsActive)//if the bus is activ
                {
                    throw new DuplicateExeption("bus with identical License's num allready exist");
                }
                Same_LicenseNum = bus;//if the 'Same_LicenseNum' is unactive then override it with the new bus
            }
        }

        public Bus GetBus(string licenseNum)
        {
            Bus bus = (from b in DataSource.Buses
                       where b.LicenseNum == licenseNum
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
                               where b.LicenseNum == newBus.LicenseNum
                               select b).FirstOrDefault();

            if (oldBus == null)//if the old bus dosn't found
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            oldBus = newBus;//set the old bus to be the update bus
        }

        public void DeleteBus(string licenseNum)
        {
            Bus bus = (from b in DataSource.Buses
                            where b.LicenseNum == licenseNum && b.IsActive
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
                line.ID = SerialNumbers.GetLineId;
            }
            catch (Exception e)//!
            {
                throw e;
            }
            DataSource.Lines.Add(line);
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
            oldLine = newLine;//override the old line with the new one
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
            //List<BusOnTrip> BusesOnTripList =
            if (DataSource.BusesOnTrip.FirstOrDefault(bot => bot.ID == busOnTrip.ID) != null)
            {
                throw new DuplicateExeption("the bus is allready in driving");
            }
            else
            {
                DataSource.BusesOnTrip.Add(busOnTrip);
            }
        }

        public BusOnTrip GetBusOnTrip(int id)
        {
            BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == id);
            if (busOnTrip == null)
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            return busOnTrip.Clone();
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            BusOnTrip oldBusOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == busOnTrip.ID);
            if (oldBusOnTrip == null)
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            oldBusOnTrip = busOnTrip;
        }

        public void DeleteBusOnTrip(int id)
        {
            BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(l => l.ID == id && l.IsActive == true);
            if (busOnTrip != null)
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
            return from bot in DataSource.BusesOnTrip
                   where bot.IsActive == true
                   select bot.Clone();
        }

        public IEnumerable<BusOnTrip> GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        {
            return from bot in DataSource.BusesOnTrip
                   where predicate(bot) && bot.IsActive == true
                   select bot.Clone();
        }
        #endregion

        #region Station


        public void AddStation(Station busStation)
        {
            DataSource.LoadData();
            XElement tempBusStation = (from bs in DataSource.dsRoot.Element("Stations").Elements()
                                       where bs.Element("Code").Value == busStation.Code.ToString()
                                             && bs.Element("IsActive").Value == true.ToString()
                                       select bs).FirstOrDefault();      
            if (tempBusStation == null)
            {
                DataSource.SaveObj(busStation, "Stations");
            }
            else//in case the bus station is allready in the data base: checks if he is active
            {
                if (tempBusStation.Element("IsActive").Value == true.ToString())
                {
                    throw new DuplicateExeption("bus station with identical License's num allready exist");
                }
                Cloning.object_to_xelement(busStation, tempBusStation);
            }
            DataSource.Save();
        }

        public Station GetStation(int code)
        {
            DataSource.LoadData();
            XElement busStation = (from bs in DataSource.dsRoot.Element("Stations").Elements()//get all the activ station
                                   where bool.Parse(bs.Element("IsActive").Value)
                                         && int.Parse(bs.Element("Code").Value) == code
                                   select bs).FirstOrDefault();
            if (busStation == null)
            {
                throw new NotExistExeption("bus's station with this code not exist");
            }
            return Cloning.xelement_to_new_object<Station>(busStation);
        }

        public void UpdateStation(Station newBusStation)
        {
            DataSource.LoadData();
            XElement oldBusStation = (from bs in DataSource.dsRoot.Element("Stations").Elements()
                                      where bs.Element("Code").Value == newBusStation.Code.ToString()
                                            && bs.Element("IsActive").Value == true.ToString()
                                      select bs).FirstOrDefault();
            if (oldBusStation == null)
            {
                throw new NotExistExeption("the station doesn't not exist");
            }
            Cloning.object_to_xelement(newBusStation, oldBusStation);
            DataSource.Save();
        }

        public void DeleteStation(int code)
        {
            DataSource.LoadData();
            XElement station = (from bs in DataSource.dsRoot.Element("Stations").Elements()
                               where bs.Element("Code").Value == code.ToString()
                                     && bs.Element("IsActive").Value == true.ToString()
                               select bs).FirstOrDefault();
            if (station != null)
            {
                station.Element("IsActive").Value = false.ToString();
            }
            else
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            DataSource.Save();
        }

        public IEnumerable<Station> GetAllStations()
        {
            DataSource.LoadData();
            return from bs in DataSource.dsRoot.Element("Stations").Elements()
                   where bool.Parse(bs.Element("IsActive").Value)
                   select Cloning.xelement_to_new_object<Station>(bs);
        }

        public IEnumerable<Station> GetAllStationBy(Predicate<Station> predicate)
        {
            DataSource.LoadData();
            return from bs in DataSource.dsRoot.Element("Stations").Elements()
                   let temp = Cloning.xelement_to_new_object<Station>(bs)
                   where temp.IsActive == true
                         && predicate(temp)
                   select temp;
        }
        #endregion

        #region LineStation

        public void AddLineStation(LineStation lineStation)
        {
            LineStation tempLineStation = (from ls in DataSource.LineStations
                                           where ls.LineId == lineStation.LineId
                                              && ls.StationNumber == lineStation.StationNumber
                                           select ls).FirstOrDefault();
            if (tempLineStation == null)//if ther no such line station in the data sorce allready
            {
                DataSource.LineStations.Add(lineStation);
            }
            //in case the Line station is allready in the data base: checks if he is active
            else
            {
                if (tempLineStation.IsActive)
                {
                    throw new DuplicateExeption("the line station is allready exist");
                }
                tempLineStation = lineStation;//override the old unactive lineStation with the new line station
            }
        }

        public LineStation GetLineStation(int lineId, int stationNum)
        {
            LineStation lineStation = (from ls in DataSource.LineStations
                                       where ls.LineId == lineId
                                             && ls.StationNumber == stationNum
                                       select ls).FirstOrDefault();
            if (lineStation == null)//if the line station dont exist
            {
                throw new NotExistExeption("the line doesn't exist or it doesn't have such a station");
            }
            return lineStation.Clone();
        }

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
            oldLineStation = newLineStation;//override the old lineStation
        }

        public void DeleteLineStation(int lineId, int stationNum)
        {
            DataSource.LineStations.RemoveAll(ls => ls.LineId == lineId && ls.StationNumber == stationNum);
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return DataSource.LineStations.Select(ls => ls.Clone());
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            return from ls in DataSource.LineStations
                   where predicate(ls)
                   select ls.Clone();
        }
        #endregion

        #region AdjacentStations

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
                DataSource.AdjacentStations.Add(tempStations);
            }            
            //else do nothing (it's not an exeption) 
        }

        public AdjacentStations GetAdjacentStation(int? stationCode1, int? stationCode2)
        {
            AdjacentStations tempStations = (from adjSt in DataSource.AdjacentStations
                                             where adjSt.StationCode1 == stationCode1
                                             && adjSt.StationCode2 == stationCode2
                                             select adjSt.Clone()).FirstOrDefault();
            return tempStations;
        }

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
            oldAdjacentStations = newAdjacentStations;//override the old AdjacentStations with the up to date one
        }

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

        ///<returns>all the objects where predicate returns true for them(active and not active)</returns>
        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adjSt in DataSource.AdjacentStations
                   where predicate(adjSt)
                   select adjSt.Clone();
        }
        

        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            DataSource.LoadData();
            lineTrip.ID = SerialNumbers.GetLineTripId;
            DataSource.SaveObj(lineTrip, "LineTrips");
            DataSource.Save();
        }

        public LineTrip GetLineTrip(int id)
        {
            DataSource.LoadData();
            XElement temp = (from lt in DataSource.dsRoot.Element("LineTrips").Elements()
                             where lt.Element("ID").Value == id.ToString()
                             select lt).FirstOrDefault();
            if(temp == null)
            {
                throw new NotExistExeption("the Line trip doesn't exist");
            }
            return Cloning.xelement_to_new_object<LineTrip>(temp);
        }

        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            DataSource.LoadData();
            XElement oldLineTrip = (from lt in DataSource.dsRoot.Element("LineTrips").Elements()
                                    where lt.Element("ID").Value == newLineTrip.ID.ToString()
                                    select lt).FirstOrDefault();
                
            if (oldLineTrip == null)
            {
                throw new NotExistExeption("the line trip doesn't exist");
            }
            Cloning.object_to_xelement(newLineTrip, oldLineTrip);
            DataSource.Save();
        }

        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            DataSource.LoadData();
            return from lt in DataSource.dsRoot.Element("LineTrips").Elements()
                   where lt.Element("IsActive").Value == true.ToString()
                   select Cloning.xelement_to_new_object<LineTrip>(lt);
        }

        public IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            DataSource.LoadData();
            return from lt in DataSource.dsRoot.Element("LineTrips").Elements()
                   let temp = Cloning.xelement_to_new_object<LineTrip>(lt)
                   where predicate(temp)
                   select temp;
        }
        #endregion

        #region User
        public void AddUser(User user)
        {
            User tempUser = DataSource.Users.FirstOrDefault(l => l.Name == user.Name);
            if (tempUser == null)
            {
                DataSource.Users.Add(user);
            }
            //in case the user is allready in the data base: checks if he is active
            else
            {
                if (tempUser.IsActive == true)
                {
                    throw new DuplicateExeption("the user is allready exist");
                }
                tempUser.IsActive = true;
            }
        }

        public User GetUser(string name)
        {
            User tempUser = DataSource.Users.FirstOrDefault(l => l.Name == name && l.IsActive == true);
            if (tempUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            return tempUser.Clone();
        }

        public void UpdateUser(User newUser)
        {
            User oldUser = DataSource.Users.Find(l => l.Name == newUser.Name && l.IsActive == true);
            if (oldUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            oldUser = newUser;
        }

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
        public void AddUserTrip(UserTrip userTrip)
        {
            userTrip.TripId = SerialNumbers.GetUserTripId;//get a serial number from SerialNumbers for the id
            DataSource.UsersTrips.Add(userTrip);//add to the data source
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
            oldUserTrip = newUserTrip;
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

        #region generic
        //void IDL.Add<T>(T newEntity)
        //{
        //    int? index = null;
        //    foreach (var entity in Enum.GetValues(typeof(Entites)))// find the newEntity's type
        //    {
        //        if (entity.ToString() == typeof(T).Name)
        //        {
        //            index = (int)entity;
        //            break;
        //        }
        //    }
        //    if (index == null)
        //    {
        //        throw new InvalidObjectExeption("the object type doesn't exist");
        //    }
        //    if (index != 0)
        //    {
        //        List<T> entityList = DataSource.dsList[(int)index] as List<T>;// get the appropriate list of the T type
        //        var dataBaseEntity = entityList.FirstOrDefault(ob => ob.GetType().GetProperties().First().GetValue(ob) == newEntity.GetType().GetProperties().First().GetValue(newEntity));//check if the key elemnt is already exist
        //        if (dataBaseEntity == null)
        //        {
        //            entityList.Add(newEntity);
        //        }
        //        //in case the entity is allready in the data base: checks if he is active
        //        else
        //        {
        //            if ((bool)dataBaseEntity.GetType().GetProperty("IsActive").GetValue(dataBaseEntity) == true) //isActive == true
        //            {
        //                throw new DuplicateExeption("the object is already exist");
        //            }
        //            dataBaseEntity.GetType().GetProperty("IsActive").SetValue(dataBaseEntity, true);
        //        }
        //    }
        //    else // the T type == AdjacentStations
        //    {
        //        List<T> entityList = DataSource.dsList[(int)index] as List<T>;// get the appropriate list of the T type
        //        var dataBaseEntity = entityList.FirstOrDefault(
        //            ob => ob.GetType().GetProperty("StationCode1").GetValue(ob) == newEntity.GetType().GetProperty("StationCode1").GetValue(newEntity)//the StationCode1 is the same
        //            && ob.GetType().GetProperty("StationCode2").GetValue(ob) == newEntity.GetType().GetProperty("StationCode2").GetValue(newEntity));//the StationCode2 is the same
        //        if (dataBaseEntity == null)
        //        {
        //            entityList.Add(newEntity);
        //        }
        //        else
        //        {
        //            throw new DuplicateExeption("AdjacentStations with identical stations are already exist");
        //        }
        //    }
        //}
        //T IDL.Get<T>(string id)
        //{
        //    int? index = null;
        //    foreach (var entity in Enum.GetValues(typeof(Entites)))// find the newEntity's type
        //    {
        //        if (entity.ToString() == typeof(T).Name)
        //        {
        //            index = (int)entity;
        //            break;
        //        }
        //    }
        //    if (index == null)
        //    {
        //        throw new InvalidObjectExeption("the object type doesn't exist");
        //    }
        //    if (index != 0)
        //    {
        //        List<T> entityList = DataSource.dsList[(int)index] as List<T>;// get the appropriate list of the T type
        //        var dataBaseEntity = entityList.FirstOrDefault(ob => ob.GetType().GetProperties().First().GetValue(ob).ToString() == id//check if the key elemnt is exist
        //                                                           && (bool)ob.GetType().GetProperty("IsActive").GetValue(ob) == true);//check if IsActive == true
        //        if (dataBaseEntity == null)
        //        {
        //            throw new NotExistExeption("the object doesn't exist");
        //        }
        //        return dataBaseEntity.Clone();
        //    }
        //    else // the T type == AdjacentStations
        //    {
        //        throw new InvalidObjectExeption("this method is not support AdjacentStations entity");
        //    }
        //}
        //AdjacentStations IDL.Get(string id1, string id2)
        //{
        //    List<AdjacentStations> entityList = DataSource.dsList[0] as List<AdjacentStations>;
        //    var dataBaseEntity = entityList.FirstOrDefault(
        //        ob => ob.GetType().GetProperty("StationCode1").GetValue(ob).ToString() == id1 //the StationCode1 is the same
        //        && ob.GetType().GetProperty("StationCode2").GetValue(ob).ToString() == id2);//the StationCode2 is the same
        //    if (dataBaseEntity == null)
        //    {
        //        throw new NotExistExeption("the object doesn't exist");
        //    }
        //    else
        //    {
        //        return dataBaseEntity.Clone();
        //    }
        //}
        //public void Update<T>(T newEntity)
        //{
        //    int? index = null;
        //    foreach (var entity in Enum.GetValues(typeof(Entites)))// find the newEntity's type
        //    {
        //        if (entity.ToString() == typeof(T).Name)
        //        {
        //            index = (int)entity;
        //            break;
        //        }
        //    }
        //    if (index == null)
        //    {
        //        throw new InvalidObjectExeption("the object type doesn't exist");
        //    }
        //    if (index != -1)
        //    {
        //        List<T> entityList = DataSource.dsList[(int)index] as List<T>;// get the appropriate list of the T type
        //        var dataBaseEntity = entityList.FirstOrDefault(ob => ob.GetType().GetProperties().First().GetValue(ob) == newEntity.GetType().GetProperties().First().GetValue(newEntity));//check if the key elemnt is already exist
        //        if (dataBaseEntity == null)
        //        {
        //            dataBaseEntity = newEntity;
        //        }
        //        //in case the entity is allready in the data base: checks if he is active
        //        else
        //        {
        //            if ((bool)dataBaseEntity.GetType().GetProperty("IsActive").GetValue(dataBaseEntity) == true) //isActive == true
        //            {
        //                throw new DuplicateExeption("the object is already exist");
        //            }
        //            dataBaseEntity.GetType().GetProperty("IsActive").SetValue(dataBaseEntity, true);
        //        }
        //    }
        //    else // the T type == AdjacentStations
        //    {
        //        List<T> entityList = DataSource.dsList[(int)index] as List<T>;// get the appropriate list of the T type
        //        var dataBaseEntity = entityList.FirstOrDefault(
        //            ob => ob.GetType().GetProperty("StationCode1").GetValue(ob) == newEntity.GetType().GetProperty("StationCode1").GetValue(newEntity)//the StationCode1 is the same
        //            && ob.GetType().GetProperty("StationCode2").GetValue(ob) == newEntity.GetType().GetProperty("StationCode2").GetValue(newEntity));//the StationCode2 is the same
        //        if (dataBaseEntity == null)
        //        {
        //            entityList.Add(newEntity);
        //        }
        //        else
        //        {
        //            throw new DuplicateExeption("AdjacentStations with identical stations are already exist");
        //        }
        //    }
        //}
        //public void Delete<T>(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<T> GetAll<T>()
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<T> GetAllBy<T>(Predicate<T> predicate)
        //{
        //    throw new NotImplementedException();
        #endregion

    }
}


