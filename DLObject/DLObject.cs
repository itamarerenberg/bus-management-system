using System;
using System.Collections.Generic;
using System.Linq;
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
            if(DataSource.Buses.FirstOrDefault(b=>b.LicenseNum == bus.LicenseNum) != null)
            {
                throw new DuplicateExeption("bus with identical License's num allready exist");
            }
            else
            {
                DataSource.Buses.Add(bus);
            }
        }
        Bus IDL.GetBus(string licenseNum)
        {
            Bus bus = DataSource.Buses.Find(b => b.LicenseNum == licenseNum);
            if(bus == null)
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
            return bus;
        }

        /// <summary>
        /// insert new bus insted of the corrent bus with identical LicenseNum
        /// </summary>
        /// <param name="bus">update bus</param>
        void IDL.UpdateBus(Bus bus)
        {
            if(!DataSource.Buses.Exists(b => b.LicenseNum == bus.LicenseNum))
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            DataSource.Buses[DataSource.Buses.FindIndex(b => b.LicenseNum == bus.LicenseNum)] = bus;
        }

        void IDL.DeleteBus(string licenseNum)
        {
            if (DataSource.Buses.RemoveAll(b => b.LicenseNum == licenseNum) == 0)
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
        }
        IEnumerable<Bus> IDL.GetAllBuses()
        {
            return from bus in DataSource.Buses
                   select bus.Clone();
        }
        IEnumerable<Bus> IDLGetAllBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSource.Buses
                   where predicate(bus)
                   select bus.Clone();
        }

        #endregion

        #region Line
        void IDL.AddLine(Line line)
        {
            if (DataSource.Lines.FirstOrDefault(l => l.ID == line.ID) != null)
            {
                throw new DuplicateExeption("line with identical ID allready exist");
            }
            else
            {
                DataSource.Lines.Add(line);
            }
        }

        Line IDL.GetLine(int id)
        {
            Line line = DataSource.Lines.Find(l => l.ID == id);
            if (line == null)
            {
                throw new NotExistExeption("line with this id not exist");
            }
            return line;
        }

        /// <summary>
        /// insert new line insted of the corrent line with identical ID
        /// </summary>
        /// <param name="line">update line</param>
        void IDL.UpdateLine(Line line)
        {
            if (!DataSource.Lines.Exists(l => l.ID == line.ID))
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            DataSource.Lines[DataSource.Lines.FindIndex(l => l.ID == line.ID)] = line;
        }

        void IDL.DeleteLine(int id)
        {
            if (DataSource.Lines.RemoveAll(l => l.ID == id) == 0)//if RemoveAll() do's not found line with such id
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        IEnumerable<Line> IDL.GetAllLines()
        {
            return from line in DataSource.Lines
                   select line.Clone();
        }

        IEnumerable<Line> IDL.GetAllLinesBy(Predicate<Line> predicate)
        {
            return from line in DataSource.Lines
                   where predicate(line)
                   select line.Clone();
        }

        #endregion

        #region BusOnTrip
        void IDL.AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSource.Lines.FirstOrDefault(bot => bot.ID == bot.ID) != null)
            {
                throw new DuplicateExeption("line with identical ID allready exist");
            }
            else
            {
                DataSource.Lines.Add(line);
            }
        }
        void IDL.GetBusOnTrip(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            throw new NotImplementedException();
        }
        IEnumerable<BusOnTrip> IDL.GetAllBusesOnTrip()
        {
            throw new NotImplementedException();
        }
        IEnumerable<BusOnTrip> IDL.GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BusStation
        BusStation IDL.AddBusStation(BusStation busStation)
        {
            throw new NotImplementedException();
        }
        void IDL.GetBusStation(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateBusStation(BusStation busStation)
        {
            throw new NotImplementedException();
        }
        IEnumerable<BusStation> IDL.GetAllBusStations()
        {
            throw new NotImplementedException();
        }
        IEnumerable<BusStation> IDL.GetAllBusStationBy(Predicate<BusStation> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LineStation
        LineStation IDL.AddLineStation(LineStation iineStation)
        {
            throw new NotImplementedException();
        }
        void IDL.GetLineStation(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateLineStation(LineStation iineStation)
        {
            throw new NotImplementedException();
        }
        IEnumerable<LineStation> IDL.GetAllLineStations()
        {
            throw new NotImplementedException();
        }
        IEnumerable<LineStation> IDL.GetAllLineStationBy(Predicate<LineStation> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

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
        #endregion

        #region LineTrip
        LineTrip IDL.AddLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }
        void IDL.GetLineTrip(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }
        IEnumerable<LineTrip> IDL.GetAllLineTrips()
        {
            throw new NotImplementedException();
        }
        IEnumerable<LineTrip> IDL.GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region User
        User IDL.AddUser(User user)
        {
            throw new NotImplementedException();
        }
        void IDL.GetUser(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
        void IDL.DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region UserTrip
        UserTrip IDL.AddUserTrip(UserTrip userTrip)
        {
            throw new NotImplementedException();
        }
        void IDL.GetUserTrip(int id)
        {
            throw new NotImplementedException();
        }
        void IDL.UpdateUserTrip(UserTrip userTrip)
        {
            throw new NotImplementedException();
        }
        IEnumerable<UserTrip> IDL.GetAllUserTrips()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
