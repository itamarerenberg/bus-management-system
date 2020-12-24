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
        //#region singelton

        //static readonly DLObject instance = new DLObject();
        //static DLObject() { }
        //DLObject() { }
        //public static DLObject Instance { get => instance; }

        //#endregion

        //#region Bus
        //void IDL.AddBuss(Bus bus)
        //{
        //    Bus tempBus = DataSource.Buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum);
        //    if (tempBus == null)
        //    {
        //        DataSource.Buses.Add(bus);
        //    }
        //    //in case the bus is allready in the data base: checks if he is active
        //    else
        //    {
        //        if (tempBus.IsActive == true)
        //        {
        //            throw new DuplicateExeption("bus with identical License's num allready exist");
        //        }
        //        tempBus.IsActive = true;
        //    }
        //}
        //Bus IDL.GetBus(string licenseNum)
        //{
        //    Bus bus = DataSource.Buses.Find(b => b.LicenseNum == licenseNum && b.IsActive == true);
        //    if(bus == null)
        //    {
        //        throw new NotExistExeption("bus with this License's num not exist");
        //    }
        //    return bus;
        //}

        ///// <summary>
        ///// insert new bus insted of the corrent bus with identical LicenseNum
        ///// </summary>
        ///// <param name="bus">update bus</param>
        //void IDL.UpdateBus(Bus bus)
        //{
        //    if(!DataSource.Buses.Exists(b => b.LicenseNum == bus.LicenseNum && b.IsActive == true))//ככה לא נצטרך לחפש אותו שוב כדי לעדכן FirstOrDefault נראה לי שעדיף לעבוד עם 
        //    {
        //        throw new NotExistExeption("bus with License's num like 'bus' not exist");
        //    }
        //    DataSource.Buses[DataSource.Buses.FindIndex(b => b.LicenseNum == bus.LicenseNum)] = bus;
        //}

        //void IDL.DeleteBus(string licenseNum)
        //{
        //    //if (DataSource.Buses.RemoveAll(b => b.LicenseNum == licenseNum) == 0)
        //    //{
        //    //    throw new NotExistExeption("bus with this License's num not exist");
        //    //}

        //    Bus bus = DataSource.Buses.Find(b => b.LicenseNum == licenseNum && b.IsActive == true);//עשיתי לפי הבונוס שלא מוחקים אלא הופכים ללא פעיל
        //    if (bus != null)
        //    {
        //        bus.IsActive = false;
        //    }
        //    else
        //    {
        //        throw new NotExistExeption("bus with this License's num not exist");
        //    }
        //    GetType(licenseNum)
        //}
        //IEnumerable<Bus> IDL.GetAllBuses()
        //{
        //    return from bus in DataSource.Buses
        //           where bus.IsActive == true
        //           select bus.Clone();
        //}
        //IEnumerable<Bus> IDL.GetAllBusesBy(Predicate<Bus> predicate)
        //{
        //    return from bus in DataSource.Buses
        //           where predicate(bus) && bus.IsActive == true
        //           select bus.Clone();
        //}

        //#endregion

        //#region Line
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

        //Line IDL.GetLine(int id)
        //{
        //    Line line = DataSource.Lines.Find(l => l.ID == id && l.IsActive == true);
        //    if (line == null)
        //    {
        //        throw new NotExistExeption("line with this id not exist");
        //    }
        //    return line;
        //}

        ///// <summary>
        ///// insert new line insted of the corrent line with identical ID
        ///// </summary>
        ///// <param name="line">update line</param>
        //void IDL.UpdateLine(Line line)
        //{
        //    if (!DataSource.Lines.Exists(l => l.ID == line.ID))//כנ"ל 
        //    {
        //        throw new NotExistExeption("line with id like 'line' not exist");
        //    }
        //    DataSource.Lines[DataSource.Lines.FindIndex(l => l.ID == line.ID)] = line;
        //}

        //void IDL.DeleteLine(int id)
        //{
        //    //if (DataSource.Lines.RemoveAll(l => l.ID == id) == 0)//if RemoveAll() do's not found line with such id
        //    //{
        //    //    throw new NotExistExeption("line with this id not exist");
        //    //}

        //    Line line = DataSource.Lines.Find(l => l.ID == id && l.IsActive == true);//עשיתי לפי הבונוס שלא מוחקים אלא הופכים ללא פעיל
        //    if (line != null)
        //    {
        //        line.IsActive = false;
        //    }
        //    else
        //    {
        //        throw new NotExistExeption("line with this id not exist");
        //    }
        //}

        //IEnumerable<Line> IDL.GetAllLines()
        //{
        //    return from line in DataSource.Lines
        //           where line.IsActive == true
        //           select line.Clone();
        //}

        //IEnumerable<Line> IDL.GetAllLinesBy(Predicate<Line> predicate)
        //{
        //    return from line in DataSource.Lines
        //           where predicate(line) && line.IsActive == true
        //           select line.Clone();
        //}

        //#endregion

        //#region BusOnTrip
        //void IDL.AddBusOnTrip(BusOnTrip busOnTrip)
        //{
        //    if (DataSource.BusesOnTrip.FirstOrDefault(bot => bot.ID == busOnTrip.ID) != null)
        //    {
        //        throw new DuplicateExeption("the bus is allready in driving");
        //    }
        //    else
        //    {
        //        DataSource.BusesOnTrip.Add(busOnTrip);
        //    }
        //}
        //BusOnTrip IDL.GetBusOnTrip(int id)
        //{
        //    BusOnTrip busOnTrip = DataSource.BusesOnTrip.Find(bot=> bot.ID == id );
        //    if (busOnTrip == null)
        //    {
        //        throw new NotExistExeption("the bus is not in driving");
        //    }
        //    return busOnTrip;
        //}
        //void IDL.UpdateBusOnTrip(BusOnTrip busOnTrip)
        //{
        //    BusOnTrip oldBusOnTrip = DataSource.BusesOnTrip.Find(bot => bot.ID == busOnTrip.ID);
        //    if (oldBusOnTrip == null)
        //    {
        //        throw new NotExistExeption("the bus is not in driving");
        //    }
        //    oldBusOnTrip = busOnTrip;
        //}
        //IEnumerable<BusOnTrip> IDL.GetAllBusesOnTrip()
        //{
        //    return from bot in DataSource.BusesOnTrip
        //           select bot.Clone();
        //}

        //IEnumerable<BusOnTrip> IDL.GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        //{
        //    return from bot in DataSource.BusesOnTrip
        //           where predicate(bot)
        //           select bot.Clone();
        //}
        //#endregion

        //#region BusStation
        //void IDL.AddBusStation(BusStation busStation)
        //{
        //    if (DataSource.BusStations.FirstOrDefault(bs => bs.Code == bs.Code) != null)
        //    {
        //        throw new DuplicateExeption("bus's station with identical Code allready exist");
        //    }
        //    else
        //    {
        //        DataSource.BusStations.Add(busStation);
        //    }
        //}

        //BusStation IDL.GetBusStation(int code)
        //{
        //    BusStation busStation = DataSource.BusStations.Find(bs => bs.Code == code);
        //    if (busStation == null)
        //    {
        //        throw new NotExistExeption("bus's station with this code not exist");
        //    }
        //    return busStation;
        //}

        ///// <summary>
        ///// insert new busStation insted of the corrent BusStation with identical Code
        ///// </summary>
        ///// <param name="busStation">updated BusStation</param>
        //void IDL.UpdateBusStation(BusStation busStation)
        //{
        //    if (!DataSource.BusStations.Exists(bs => bs.Code == busStation.Code))
        //    {
        //        throw new NotExistExeption("bus's station with Code like 'busOnTrip' not exist");
        //    }
        //    DataSource.BusStations[DataSource.BusStations.FindIndex(bs => bs.Code == bs.Code)] = busStation;//!        }
        //}

        //IEnumerable<BusStation> IDL.GetAllBusStations()
        //{
            
        //}
        //IEnumerable<BusStation> IDL.GetAllBusStationBy(Predicate<BusStation> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion

        //#region LineStation
        //object text<T>(object obj)
        //{
        //    foreach (object type in typeof(T).GetFields())
        //    {
        //        if (typeof(Type) == typeof(List<obj.GetType() >))
        //        {

        //        }
        //    }
        //    return obj;
        //}
        //void IDL.GetLineStation(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.UpdateLineStation(LineStation iineStation)
        //{
        //    throw new NotImplementedException();
        //}
        //IEnumerable<LineStation> IDL.GetAllLineStations()
        //{
        //    throw new NotImplementedException();
        //}
        //IEnumerable<LineStation> IDL.GetAllLineStationBy(Predicate<LineStation> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion

        #region AdjacentStations
        AdjacentStations IDL.AddAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }
        void IDL.GetBackAdjacentStation(int stationCode)
        {
            throw new NotImplementedException();
        }
        void IDL.GetAheadAdjacentStation(int stationCode)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T type)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int id)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T type)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllBy<T>(Predicate<T> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        //#region LineTrip
        //LineTrip IDL.AddLineTrip(LineTrip lineTrip)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.GetLineTrip(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.UpdateLineTrip(LineTrip lineTrip)
        //{
        //    throw new NotImplementedException();
        //}
        //IEnumerable<LineTrip> IDL.GetAllLineTrips()
        //{
        //    throw new NotImplementedException();
        //}
        //IEnumerable<LineTrip> IDL.GetAllLineTripBy(Predicate<LineTrip> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion

        //#region User
        //User IDL.AddUser(User user)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.GetUser(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.UpdateUser(User user)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.DeleteUser(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion

        //#region UserTrip
        //UserTrip IDL.AddUserTrip(UserTrip userTrip)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.GetUserTrip(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //void IDL.UpdateUserTrip(UserTrip userTrip)
        //{
        //    throw new NotImplementedException();
        //}
        //IEnumerable<UserTrip> IDL.GetAllUserTrips()
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
    }
}
