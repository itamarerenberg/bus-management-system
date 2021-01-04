using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLApi;
using DO;
using DS;

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
            Bus tempBus = DataSource.Buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum);
            if (tempBus == null)
            {
                DataSource.Buses.Add(bus);
            }
            //in case the bus is allready in the data base: checks if he is active
            else
            {
                if (tempBus.IsActive == true)
                {
                    throw new DuplicateExeption("bus with identical License's num allready exist");
                }
                tempBus.IsActive = true;
            }
        }
        public Bus GetBus(string licenseNum)
        {
            Bus bus = DataSource.Buses.Find(b => b.LicenseNum == licenseNum && b.IsActive == true);
            if (bus == null)
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
            return bus.Clone();
        }

        /// <summary>
        /// insert new bus insted of the corrent bus with identical LicenseNum
        /// </summary>
        /// <param name="bus">updated bus</param>
        public void UpdateBus(Bus newBus)
        {
            Bus oldBus = DataSource.Buses.Find(b => b.LicenseNum == newBus.LicenseNum && b.IsActive == true);
            if (oldBus == null)
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            oldBus = newBus;
        }

        public void DeleteBus(string licenseNum)
        {
            //if (DataSource.Buses.RemoveAll(b => b.LicenseNum == licenseNum) == 0)
            //{
            //    throw new NotExistExeption("bus with this License's num not exist");
            //}

            Bus bus = DataSource.Buses.Find(b => b.LicenseNum == licenseNum && b.IsActive == true);//עשיתי לפי הבונוס שלא מוחקים אלא הופכים ללא פעיל
            if (bus != null)
            {
                bus.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from bus in DataSource.Buses
                   where bus.IsActive == true
                   select bus.Clone();
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSource.Buses
                   where predicate(bus) && bus.IsActive == true
                   select bus.Clone();
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
            try
            {
                line.ID = SerialNumbers.GetLineId;
            }
            catch (Exception e)//!
            {
                throw e;
            }
            DataSource.Lines.Add(line);
        }

        public Line GetLine(int id)
        {
            Line line = DataSource.Lines.Find(l => l.ID == id && l.IsActive == true);
            if (line == null)
            {
                throw new NotExistExeption("line with this id not exist");
            }
            return line.Clone();
        }

        /// <summary>
        /// insert new line insted of the corrent line with identical ID
        /// </summary>
        /// <param name="line">update line</param>
        public void UpdateLine(Line newLine)
        {
            Line oldLine = DataSource.Lines.Find(l => l.ID == newLine.ID && l.IsActive == true);
            if (oldLine == null)
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            oldLine = newLine;
        }

        public void DeleteLine(int id)
        {
            Line line = DataSource.Lines.Find(l => l.ID == id && l.IsActive == true);
            if (line != null)
            {
                line.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from line in DataSource.Lines
                   where line.IsActive == true
                   select line.Clone();
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return from line in DataSource.Lines
                   where predicate(line) && line.IsActive == true
                   select line.Clone();
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
            Station tempBusStation = DataSource.Stations.FirstOrDefault(b => b.Code == busStation.Code);
            if (tempBusStation == null)
            {
                DataSource.Stations.Add(busStation);
            }
            //in case the bus station is allready in the data base: checks if he is active
            else
            {
                if (tempBusStation.IsActive == true)
                {
                    throw new DuplicateExeption("bus station with identical License's num allready exist");
                }
                tempBusStation.IsActive = true;
            }
        }

        public Station GetStation(int code)
        {
            Station busStation = DataSource.Stations.Find(bs => bs.Code == code && bs.IsActive == true);
            if (busStation == null)
            {
                throw new NotExistExeption("bus's station with this code not exist");
            }
            return busStation.Clone();
        }

        /// <summary>
        /// insert new busStation insted of the corrent Station with identical Code
        /// </summary>
        /// <param name="busStation">updated Station</param>
        public void UpdateStation(Station newBusStation)
        {
            Station oldBusStation = DataSource.Stations.Find(bs => bs.Code == newBusStation.Code && bs.IsActive == true);
            if (oldBusStation == null)
            {
                throw new NotExistExeption("the station doesn't not exist");
            }
            oldBusStation = newBusStation;
        }

        public void DeleteStation(int code)
        {
            Station station = DataSource.Stations.Find(s => s.Code == code && s.IsActive == true);
            if (station != null)
            {
                station.IsActive = false;
            }
            else
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
        }

        public IEnumerable<Station> GetAllStations()
        {
            return from station in DataSource.Stations
                   where station.IsActive == true
                   select station.Clone();
        }

        public IEnumerable<Station> GetAllStationBy(Predicate<Station> predicate)
        {
            return from station in DataSource.Stations
                   where predicate(station) && station.IsActive == true
                   select station.Clone();
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


