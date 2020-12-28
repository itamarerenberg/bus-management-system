using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        void IDL.AddBuss(Bus bus)
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
        Bus IDL.GetBus(string licenseNum)
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
        void IDL.UpdateBus(Bus newBus)
        {
            Bus oldBus = DataSource.Buses.Find(b => b.LicenseNum == newBus.LicenseNum && b.IsActive == true);
            if (oldBus == null)
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            oldBus = newBus;
        }

        void IDL.DeleteBus(string licenseNum)
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
        IEnumerable<Bus> IDL.GetAllBuses()
        {
            return from bus in DataSource.Buses
                   where bus.IsActive == true
                   select bus.Clone();
        }
        IEnumerable<Bus> IDL.GetAllBusesBy(Predicate<Bus> predicate)
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
        void IDL.AddLine(Line line)
        {
            line.ID = ++DataSource.serialLineID;
            DataSource.Lines.Add(line);
        }

        Line IDL.GetLine(int id)
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
        void IDL.UpdateLine(Line newLine)
        {
            Line oldLine = DataSource.Lines.Find(l => l.ID == newLine.ID && l.IsActive == true);
            if (oldLine == null)
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            oldLine = newLine;
        }

        void IDL.DeleteLine(int id)
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

        IEnumerable<Line> IDL.GetAllLines()
        {
            return from line in DataSource.Lines
                   where line.IsActive == true
                   select line.Clone();
        }

        IEnumerable<Line> IDL.GetAllLinesBy(Predicate<Line> predicate)
        {
            return from line in DataSource.Lines
                   where predicate(line) && line.IsActive == true
                   select line.Clone();
        }

        #endregion

        #region BusOnTrip
        void IDL.AddBusOnTrip(BusOnTrip busOnTrip)
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
        BusOnTrip IDL.GetBusOnTrip(int id)
        {
            BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == id);
            if (busOnTrip == null)
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            return busOnTrip.Clone();
        }
        void IDL.UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            BusOnTrip oldBusOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == busOnTrip.ID);
            if (oldBusOnTrip == null)
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            oldBusOnTrip = busOnTrip;
        }
        void IDL.DeleteBusOnTrip(int id)
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

        IEnumerable<BusOnTrip> IDL.GetAllBusesOnTrip()
        {
            return from bot in DataSource.BusesOnTrip
                   where bot.IsActive == true
                   select bot.Clone();
        }

        IEnumerable<BusOnTrip> IDL.GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        {
            return from bot in DataSource.BusesOnTrip
                   where predicate(bot) && bot.IsActive == true
                   select bot.Clone();
        }
        #endregion

        #region BusStation
        void IDL.AddBusStation(BusStation busStation)
        {
            BusStation tempBusStation = DataSource.BusStations.FirstOrDefault(b => b.Code == busStation.Code);
            if (tempBusStation == null)
            {
                DataSource.BusStations.Add(busStation);
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

        BusStation IDL.GetBusStation(int code)
        {
            BusStation busStation = DataSource.BusStations.Find(bs => bs.Code == code && bs.IsActive == true);
            if (busStation == null)
            {
                throw new NotExistExeption("bus's station with this code not exist");
            }
            return busStation.Clone();
        }

        /// <summary>
        /// insert new busStation insted of the corrent BusStation with identical Code
        /// </summary>
        /// <param name="busStation">updated BusStation</param>
        void IDL.UpdateBusStation(BusStation newBusStation)
        {
            BusStation oldBusStation = DataSource.BusStations.Find(bs => bs.Code == newBusStation.Code && bs.IsActive == true);
            if (oldBusStation == null)
            {
                throw new NotExistExeption("the station doesn't not exist");
            }
            oldBusStation = newBusStation;
        }

        IEnumerable<BusStation> IDL.GetAllBusStations()
        {
            return from station in DataSource.BusStations
                   where station.IsActive == true
                   select station.Clone();
        }
        IEnumerable<BusStation> IDL.GetAllBusStationBy(Predicate<BusStation> predicate)
        {
            return from station in DataSource.BusStations
                   where predicate(station) && station.IsActive == true
                   select station.Clone();
        }
        #endregion

        #region LineStation

        void IDL.AddLineStation(LineStation lineStation)
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
        LineStation IDL.GetLineStation(int lineId, int stationNum)
        {
            LineStation lineStation = DataSource.LineStations.Find(l => l.LineId == lineId && l.StationNumber == stationNum && l.IsActive == true);
            if (lineStation == null)
            {
                throw new NotExistExeption("the line doesn't exist or it doesn't have such a station");
            }
            return lineStation.Clone();
        }
        LineStation IDL.GetLineStationByIndex(int lineId, int index)
        {
            LineStation lineStation = DataSource.LineStations.Find(l => l.LineId == lineId && l.LineStationIndex == index && l.IsActive == true);
            if (lineStation == null)
            {
                throw new NotExistExeption("the line doesn't exist or the index is out of range");
            }
            return lineStation.Clone();
        }
        void IDL.UpdateLineStation(LineStation newLineStation)
        {
            LineStation oldLineStation = DataSource.LineStations.FirstOrDefault(l => l.LineId == newLineStation.LineId);
            if (oldLineStation == null)
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            oldLineStation = newLineStation;
        }
        IEnumerable<LineStation> IDL.GetAllLineStations()
        {
            return from lineStation in DataSource.LineStations
                   where lineStation.IsActive == true
                   select lineStation.Clone();
        }
        IEnumerable<LineStation> IDL.GetAllLineStationBy(Predicate<LineStation> predicate)
        {
            return from lineStation in DataSource.LineStations
                   where predicate(lineStation) && lineStation.IsActive == true
                   select lineStation.Clone();
        }
        #endregion

        #region AdjacentStations

        void IDL.AddAdjacentStations(AdjacentStations adjacentStations)
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

        AdjacentStations IDL.GetBackAdjacentStation(int stationCode)
        {
            AdjacentStations tempStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode2 == stationCode);
            if (tempStations == null)
            {
                throw new NotExistExeption("the Adjacent Station doesn't exist");
            }
            return tempStations.Clone();
        }
        AdjacentStations IDL.GetAheadAdjacentStation(int stationCode)
        {
            AdjacentStations tempStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == stationCode);
            if (tempStations == null)
            {
                throw new NotExistExeption("the Adjacent Station doesn't exist");
            }
            return tempStations.Clone();
        }
        void IDL.UpdateAdjacentStations(AdjacentStations newAdjacentStations)
        {
            AdjacentStations oldAdjacentStations = DataSource.AdjacentStations.FirstOrDefault(s => s.StationCode1 == newAdjacentStations.StationCode1 && s.StationCode2 == newAdjacentStations.StationCode2);
            if (oldAdjacentStations == null)
            {
                throw new NotExistExeption("the Adjacent Station doesn't exist");
            }
            oldAdjacentStations = newAdjacentStations;
        }

        #endregion

        #region LineTrip
        void IDL.AddLineTrip(LineTrip lineTrip)
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
        LineTrip IDL.GetLineTrip(int id)
        {
            LineTrip lineTrip = DataSource.LineTrips.Find(l => l.ID == id && l.IsActive == true);
            if (lineTrip == null)
            {
                throw new NotExistExeption("line trip with this id doesn't exist");
            }
            return lineTrip.Clone();
        }
        void IDL.UpdateLineTrip(LineTrip newLineTrip)
        {
            LineTrip oldLineTrip = DataSource.LineTrips.Find(l => l.ID == newLineTrip.ID && l.IsActive == true);
            if (oldLineTrip == null)
            {
                throw new NotExistExeption("the line trip doesn't exist");
            }
            oldLineTrip = newLineTrip;
        }
        IEnumerable<LineTrip> IDL.GetAllLineTrips()
        {
            return from lineTrip in DataSource.LineTrips
                   where lineTrip.IsActive == true
                   select lineTrip.Clone();
        }
        IEnumerable<LineTrip> IDL.GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            return from lineTrip in DataSource.LineTrips
                   where predicate(lineTrip) && lineTrip.IsActive == true
                   select lineTrip.Clone();
        }
        #endregion

        #region User
        void IDL.AddUser(User user)
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
        User IDL.GetUser(string name, string password)
        {
            User tempUser = DataSource.Users.FirstOrDefault(l => l.Name == name && l.IsActive == true);
            if (tempUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            return tempUser.Clone();
        }
        void IDL.UpdateUser(User newUser)
        {
            User oldUser = DataSource.Users.Find(l => l.Name == newUser.Name && l.IsActive == true);
            if (oldUser == null)
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            oldUser = newUser;
        }
        void IDL.DeleteUser(string name)
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
        void IDL.AddUserTrip(UserTrip userTrip)
        {
            UserTrip tempUserTrip = DataSource.UsersTrips.FirstOrDefault(u => u.TripId == userTrip.TripId);
            if (tempUserTrip == null)
            {
                DataSource.UsersTrips.Add(userTrip);
            }
            //in case the user trip is allready in the data base: checks if he is active
            else
            {
                if (tempUserTrip.IsActive == true)
                {
                    throw new DuplicateExeption("the user trip is allready exist");
                }
                tempUserTrip.IsActive = true;
            }
        }
        UserTrip IDL.GetUserTrip(int id)
        {
            UserTrip tempUserTrip = DataSource.UsersTrips.FirstOrDefault(u => u.TripId == id && u.IsActive == true);
            if (tempUserTrip == null)
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            return tempUserTrip.Clone();
        }
        void IDL.UpdateUserTrip(UserTrip newUserTrip)
        {
            UserTrip oldUserTrip = DataSource.UsersTrips.Find(u => u.TripId == newUserTrip.TripId && u.IsActive == true);
            if (oldUserTrip == null)
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            oldUserTrip = newUserTrip;
        }
        IEnumerable<UserTrip> IDL.GetAllUserTrips()
        {
            return from userTrip in DataSource.UsersTrips
                   where userTrip.IsActive == true
                   select userTrip.Clone();
        }
        IEnumerable<UserTrip> IDL.GetAllUserTripsBy(Predicate<UserTrip> predicate)
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


