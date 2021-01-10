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
            DataSource.LoadData();
            XElement Same_LicenseNum = (from b in DataSource.dsRoot.Element("Buses").Elements()
                                        where b.Element("LicenseNum").Value == bus.LicenseNum
                                        select b).FirstOrDefault();
            if (Same_LicenseNum == null)//if no bus have the same LicenseNum
            {
                DataSource.SaveObj(bus, "Buses");//add bus to label Buses
            }
            else//in case the bus is allready in the data base: checks if he is active
            {
                if (Same_LicenseNum.Element("IsActive").Value == true.ToString())//if the bus is activ
                {
                    throw new DuplicateExeption("bus with identical License's num allready exist");
                }
                Same_LicenseNum.Element("IsActive").Value = true.ToString();
                DataSource.Save();//Save Chenges
            }
        }
        public Bus GetBus(string licenseNum)
        {
            DataSource.LoadData();
            XElement bus = (from b in DataSource.dsRoot.Elements("Buses")
                       where b.Element("LicenseNum").Value == licenseNum
                       select b).FirstOrDefault();

            if (bus == null)
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }

            Cloning.xelement_to_object(bus, out Bus ret);
            return ret;//                       ^^^^^^^
        }

        public void UpdateBus(Bus newBus)
        {
            DataSource.LoadData();
            XElement oldBus = (from b in DataSource.dsRoot.Element("Buses").Elements()
                               where b.Element("LisenceNum").Value == newBus.LicenseNum
                               select b).FirstOrDefault();

            if (oldBus == null)
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            Cloning.object_to_xelement(newBus, oldBus);
            DataSource.Save();
        }

        public void DeleteBus(string licenseNum)
        {
            DataSource.LoadData();
            XElement bus = (from b in DataSource.dsRoot.Element("Buses").Elements()
                            where b.Element("LicenseNum").Value == licenseNum && b.Element("IsActive").Value == true.ToString()
                            select b).FirstOrDefault();
            if (bus != null)
            {
                bus.Element("IsActive").Value = false.ToString();
            }
            else
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
            DataSource.Save();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            DataSource.LoadData();
            return from b in DataSource.dsRoot.Element("Buses").Elements()
            where b.Element("IsActive").Value == true.ToString()
            select Cloning.xelement_to_new_object<Bus>(b);
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            DataSource.LoadData();
            return from b in DataSource.dsRoot.Element("Buses").Elements()
                   let temp = Cloning.xelement_to_new_object<Bus>(b)
                   where predicate(temp) && temp.IsActive == true
                   select temp;
        }

        #endregion

        #region Line
        //void IDL.AddLine(Line line)
        //{
        //    Line tempLine = DataSource.Lines.FirstOrDefault(l => l.ID == line.ID);
        //    if (tempLine == null)
        //    {
        //        DataSource.Lines.Add(line);
        //    }
        //    //in case the Line is allready in the data base: checks if he is active
        //    else
        //    {
        //        if (tempLine.IsActive == true)
        //        {
        //            throw new DuplicateExeption("line with identical ID allready exist");
        //        }
        //        tempLine.IsActive = true;
        //    }
        //}
        public void AddLine(Line line)
        {
            DataSource.LoadData();
            try
            {
                line.ID = SerialNumbers.GetLineId;
            }
            catch (Exception e)//!
            {
                throw e;
            }
            DataSource.SaveObj(line, "Lines");
            DataSource.Save();
        }

        public Line GetLine(int id)
        {
            DataSource.LoadData();
            XElement line = (from l in DataSource.dsRoot.Element("Lines").Elements()
                             where l.Element("ID").Value == id.ToString() &&
                                   l.Element("IsActive").Value == true.ToString()
                             select l).FirstOrDefault();
            if (line == null)
            {
                throw new NotExistExeption("line with this id not exist");
            }
            return Cloning.xelement_to_new_object<Line>(line);
        }

        public void UpdateLine(Line newLine)
        {
            DataSource.LoadData();
            XElement oldLine = (from l in DataSource.dsRoot.Element("Lines").Elements()
                                where l.Element("ID").Value == newLine.ID.ToString() &&
                                      l.Element("IsActive").Value == true.ToString()
                                select l).FirstOrDefault();
            if (oldLine == null)
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            Cloning.object_to_xelement(newLine, oldLine);//insert the ditailes of the new line to the xelement of the old line
            DataSource.Save();
        }

        public void DeleteLine(int id)
        {
            DataSource.LoadData();
            XElement line = (from l in DataSource.dsRoot.Element("Lines").Elements()
                             where l.Element("ID").Value == id.ToString() &&
                                   l.Element("IsActive").Value == true.ToString()
                             select l).FirstOrDefault();
            if (line != null)
            {
                line.Element("IsActive").Value = false.ToString();
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
            DataSource.Save();
        }

        public IEnumerable<Line> GetAllLines()
        {
            DataSource.LoadData();
            return from l in DataSource.dsRoot.Element("Lines").Elements()
                   where l.Element("IsActive").Value == true.ToString()
                   select Cloning.xelement_to_new_object<Line>(l);
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            DataSource.LoadData();
            return from l in DataSource.dsRoot.Element("Lines").Elements()
                   let temp = Cloning.xelement_to_new_object<Line>(l)
                   where temp.IsActive == true && predicate(temp)
                   select temp;
        }

        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
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
            XElement busStation = (from bs in DataSource.dsRoot.Element("Stations").Elements()
                                   where bs.Element("IsActive").Value == true.ToString()
                                         && bs.Element("Code").Value == code.ToString()
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
                   where bs.Element("IsActive").Value == true.ToString()
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
            LineStation tempLineStation = DataSource.LineStations.FirstOrDefault(l => l.LineId == lineStation.LineId);
            if (tempLineStation == null)
            {
                DataSource.LineStations.Add(lineStation);
            }
            //in case the Line station is allready in the data base: checks if he is active
            else
            {
                if (tempLineStation.IsActive == true)
                {
                    throw new DuplicateExeption("the line station is allready exist");
                }
                tempLineStation.IsActive = true;
            }
        }

        public LineStation GetLineStation(int lineId, int stationNum)
        {
            LineStation lineStation = DataSource.LineStations.Find(l => l.LineId == lineId && l.StationNumber == stationNum && l.IsActive == true);
            if (lineStation == null)
            {
                throw new NotExistExeption("the line doesn't exist or it doesn't have such a station");
            }
            return lineStation.Clone();
        }

        public LineStation GetLineStationByIndex(int lineId, int index)
        {
            LineStation lineStation = DataSource.LineStations.Find(l => l.LineId == lineId && l.LineStationIndex == index && l.IsActive == true);
            if (lineStation == null)
            {
                throw new NotExistExeption("the line doesn't exist or the index is out of range");
            }
            return lineStation.Clone();
        }

        public void UpdateLineStation(LineStation newLineStation)
        {
            LineStation oldLineStation = DataSource.LineStations.FirstOrDefault(l => l.LineId == newLineStation.LineId);
            if (oldLineStation == null)
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            oldLineStation = newLineStation;
        }

        public void DeleteLineStation(int lineId, int stationNum)
        {
            LineStation lineS = DataSource.LineStations.Find(l => l.LineId == lineId && l.StationNumber == stationNum && l.IsActive == true);
            if (lineS != null)
            {
                lineS.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in DataSource.LineStations
                   where lineStation.IsActive == true
                   select lineStation.Clone();
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            return from lineStation in DataSource.LineStations
                   where predicate(lineStation) && lineStation.IsActive == true
                   select lineStation.Clone();
        }
        #endregion

        #region AdjacentStations

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            AdjacentStations tempStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == adjacentStations.StationCode1 && s.StationCode2 == adjacentStations.StationCode2);
            if (tempStations == null)
            {
                DataSource.AdjacentStations.Add(adjacentStations);
            }
            else
            {
                throw new DuplicateExeption("the adjacent stations is allready exist");
            }
        }
        public AdjacentStations GetAdjacentStation(int? stationCode1, int? stationCode2)
        {
            AdjacentStations tempStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == stationCode1 && s.StationCode2 == stationCode2);
            if (tempStations == null)
                return null;
            return tempStations.Clone();
        }

        public void UpdateAdjacentStations(AdjacentStations newAdjacentStations)
        {
            AdjacentStations oldAdjacentStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == newAdjacentStations.StationCode1 && s.StationCode2 == newAdjacentStations.StationCode2);
            if (oldAdjacentStations == null)
            {
                throw new NotExistExeption("the Adjacent Stations doesn't exist");
            }
            oldAdjacentStations = newAdjacentStations;
        }

        public bool DeleteAdjacentStations(int? stationCode1, int? stationCode2)
        {
            AdjacentStations tempAdjacentStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == stationCode1 && s.StationCode2 == stationCode2);
            if (tempAdjacentStations == null)
                return false;
            //DataSource.AdjacentStations.Remove(tempAdjacentStations);
            tempAdjacentStations.IsActive = false;
            return true;
        }
        public bool DeleteAdjacentStations(AdjacentStations adjacentStations)
        {
            if (!DataSource.AdjacentStations.Contains(adjacentStations))
                return false;

            adjacentStations.IsActive = false;
            return true;
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adjacentStations in DataSource.AdjacentStations
                   where predicate(adjacentStations) && adjacentStations.IsActive == true
                   select adjacentStations.Clone();
        }
        

        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            LineTrip tempLineTrip = DataSource.LineTrips.FirstOrDefault(l => l.ID == lineTrip.ID);
            if (tempLineTrip == null)
            {
                DataSource.LineTrips.Add(lineTrip);
            }
            //in case the Line trip is allready in the data base: checks if he is active
            else
            {
                if (tempLineTrip.IsActive == true)
                {
                    throw new DuplicateExeption("line trip with identical ID allready exist");
                }
                tempLineTrip.IsActive = true;
            }
        }

        public LineTrip GetLineTrip(int id)
        {
            LineTrip lineTrip = DataSource.LineTrips.Find(l => l.ID == id && l.IsActive == true);
            if (lineTrip == null)
            {
                throw new NotExistExeption("line trip with this id doesn't exist");
            }
            return lineTrip.Clone();
        }

        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            LineTrip oldLineTrip = DataSource.LineTrips.Find(l => l.ID == newLineTrip.ID && l.IsActive == true);
            if (oldLineTrip == null)
            {
                throw new NotExistExeption("the line trip doesn't exist");
            }
            oldLineTrip = newLineTrip;
        }

        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from lineTrip in DataSource.LineTrips
                   where lineTrip.IsActive == true
                   select lineTrip.Clone();
        }

        public IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            return from lineTrip in DataSource.LineTrips
                   where predicate(lineTrip) && lineTrip.IsActive == true
                   select lineTrip.Clone();
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


