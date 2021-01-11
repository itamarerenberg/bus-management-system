using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BLApi;
using BLApi;
using BO;
using DLApi;

namespace BL
{
    public class BLImpAdmin : IBL//צריך להפוך לסינגלטון
    {

        #region singelton
        static readonly BLImpAdmin instance = new BLImpAdmin();
        static BLImpAdmin() { }
        BLImpAdmin() { }
        public static BLImpAdmin Instance { get => instance; } 
        #endregion

        IDL dl = DLFactory.GetDL();
        
        #region Manager
        public bool GetManagar(string name, string password)
        {
            try
            {
                //validation
                DO.User user = dl.GetUser(name);

                if (user.Admin == false)
                    throw new InvalidInput("the user doesn't have an administrator access");

                if (user.Password != password)
                    throw new InvalidPassword("invalid password");

                return true;
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
        public void AddManagar(string name, string password)
        {
            try
            {
                DO.User tempUser = new DO.User()
                {
                    Name = name,
                    Password = password,
                    Admin = true,
                    IsActive = true
                };
                dl.AddUser(tempUser);
            }
            catch (DO.DuplicateExeption msg)
            {
                throw msg;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        
        public void UpdateManagar(string oldName, string oldPassword, string newName, string newPassword)
        {
            try
            {
                DO.User oldManagar = dl.GetUser(oldName);
                if (oldManagar.Password != oldPassword)
                    throw new InvalidInput("the old password is incorect");
                if (oldManagar.Admin == false)
                    throw new InvalidInput("the user doesn't have an administrator access");
                DO.User newManagar = new DO.User()
                {
                    Name = newName,
                    Password = newPassword,
                    Admin = true,
                };
                dl.UpdateUser(newManagar);
            }
            catch (DO.NotExistExeption)
            {
                throw new InvalidPassword("the password or name are invalid");
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
        public void DeleteManagar(string name, string password)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Bus

        public void AddBus(Bus bus)
        {
            //check if the langth of the LicenseNum fit to the LicensDate
            if (bus.LicenesDate.Year > 2018)
            {
                if (bus.LicensNumber.Length != 8)
                {
                    throw new InvalidInput("Licens number is not fit to the licens date");
                }
            }
            else
            {
                if (bus.LicensNumber.Length != 7)
                {
                    throw new InvalidInput("Licens number is not fit to the licens date");
                }
            }
            try
            {
                dl.AddBus((DO.Bus)bus.CopyPropertiesToNew(typeof(DO.Bus)));
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }

        }
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
        public void UpdateBus(Bus bus)
        {
            try
            {
                DO.Bus tempBus = (DO.Bus)bus.CopyPropertiesToNew(typeof(DO.Bus));
                dl.UpdateBus(tempBus);
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public void DeleteBus(string licensNum)
        {
            try
            {
                dl.DeleteBus(licensNum);
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
        public int AddLine(Line line)
        {
            try
            {
                int id = dl.AddLine((DO.Line)line.CopyPropertiesToNew(typeof(DO.Line)));//creats DO line from BO line

                if (line.Stations != null)
                {
                    foreach (LineStation lStation in line.Stations)
                    {
                        dl.AddLineStation((DO.LineStation)lStation.CopyPropertiesToNew(typeof(DO.LineStation)));//creats DO Line Station from BO Line Station
                        dl.AddAdjacentStations((DO.AdjacentStations)lStation.PrevToCurrent.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                        dl.AddAdjacentStations((DO.AdjacentStations)lStation.CurrentToNext.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                    }
                }
                return id;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
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
                throw msg;
            }
        }
        public void UpdateLine(Line line)
        {
            try
            {
                dl.UpdateLine((DO.Line)line.CopyPropertiesToNew(typeof(DO.Line)));//creats DO line from BO line
                foreach (LineStation lStation in line.Stations)
                {   
                    dl.UpdateLineStation((DO.LineStation)lStation.CopyPropertiesToNew(typeof(DO.LineStation)));//creats DO Line Station from BO Line Station
                    if(lStation.CurrentToNext == null) { break; } //if is the last station => break
                    dl.UpdateAdjacentStations((DO.AdjacentStations)lStation.CurrentToNext.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                }
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        public void DeleteLine(int id)
        {
            Line line = GetLine(id);
            try
            {
                foreach (LineStation lStation in line.Stations)
                {
                    DeleteLineStation(id, lStation.StationNumber);
                }
                dl.DeleteLine(id);
            }
            catch (Exception msg)
            {
                throw msg;
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
                throw msg;
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
                throw msg;
            }
        }
        #endregion

        #region LineStation
        public void AddLineStation(int lineId ,int stationNumber, int index)
        {
            Line line = GetLine(lineId);
            if (index < 0 || index > line.Stations.Count)
            {
                throw new IndexOutOfRangeException("the index is out of range");
            }
            DO.LineStation lineStationDO = new DO.LineStation()
            {
                LineId = lineId,
                StationNumber = stationNumber,
                LineStationIndex = index,
                PrevStation = index == 0 ? null : (int?)line.Stations[index - 1].StationNumber,
                NextStation = index == line.Stations.Count ? null : (int?)line.Stations[index].StationNumber,
                IsActive = true
            };

            //generating the BO line station
            LineStation lineStation = (LineStation)lineStationDO.CopyPropertiesToNew(typeof(LineStation));//copy from DO line station          
            lineStation = (LineStation)GetStation(stationNumber).CopyPropertiesToNew(typeof(LineStation));//copy from BO station          

            if (HelpMethods.AddAdjacentStations(lineStationDO.PrevStation, lineStationDO.NextStation))//if success to add "Adjacent Stations" (it's mean the line station is not the first station)
                lineStation.PrevToCurrent = HelpMethods.GetAdjacentStations(lineStationDO.PrevStation, lineStationDO.NextStation);

            if(HelpMethods.AddAdjacentStations(lineStationDO.NextStation, lineStationDO.PrevStation))//if success to add "Adjacent Stations" (it's mean the line station is not the last station)
                lineStation.CurrentToNext = HelpMethods.GetAdjacentStations(lineStationDO.NextStation, lineStationDO.PrevStation);

            //updating the stations's index
            for (int i = index; i < line.Stations.Count; i++)//updates the indexes of the stations
            {
                line.Stations[i].LineStationIndex++;
            }
            line.Stations.Insert(index, lineStation);

            if(lineStation.PrevToCurrent != null)//the added station is not in the first location
            {
                HelpMethods.DeleteAdjacentStations(line.Stations[index - 1].CurrentToNext);
                line.Stations[index - 1].CurrentToNext = line.Stations[index].PrevToCurrent;
            }

            if (lineStation.CurrentToNext != null)//the added station is not in the last location
            {
                HelpMethods.DeleteAdjacentStations(line.Stations[index + 1].PrevToCurrent);
                line.Stations[index + 1].PrevToCurrent = line.Stations[index].CurrentToNext;
            }

            dl.AddLineStation(lineStationDO);
        }
        public void UpdateLineStation(int lineNumber, int StationNumber)
        {
            throw new NotImplementedException();
        }
        public void DeleteLineStation(int lineNumber, int StationNumber)
        {
            Line line = GetLine(lineNumber);
            LineStation lineStation = HelpMethods.GetLineStation(lineNumber, StationNumber);

            if (lineStation.PrevToCurrent != null)//the deleted station is not in the first location
            {
                HelpMethods.DeleteAdjacentStations(line.Stations[lineStation.LineStationIndex - 1].CurrentToNext);
            }
            if (lineStation.CurrentToNext != null)//the deleted station is not in the last location
            {
                HelpMethods.DeleteAdjacentStations(line.Stations[lineStation.LineStationIndex + 1].PrevToCurrent);
            }
            if (lineStation.PrevToCurrent != null && lineStation.CurrentToNext != null)//the deleted station is niether in the first and last
            {
                HelpMethods.AddAdjacentStations(lineStation.PrevToCurrent.StationCode1, lineStation.CurrentToNext.StationCode2);
                AdjacentStations newAdjacentStations = HelpMethods.GetAdjacentStations(lineStation.PrevToCurrent.StationCode1, lineStation.CurrentToNext.StationCode2);
                line.Stations[lineStation.LineStationIndex - 1].CurrentToNext = newAdjacentStations;
                line.Stations[lineStation.LineStationIndex + 1].PrevToCurrent = newAdjacentStations;
            }
            for (int i = line.Stations.Count - 1; i > lineStation.LineStationIndex; i--)
            {
                line.Stations[i].LineStationIndex--;
            }
            line.Stations.RemoveAt(lineStation.LineStationIndex);
            dl.DeleteLineStation(lineNumber, StationNumber);
        }

        #endregion

        #region Line trip
        public void AddLineTrip(LineTrip lineTrip)
        {
            try
            {
                dl.AddLineTrip((DO.LineTrip)lineTrip.CopyPropertiesToNew(typeof(DO.LineTrip)));
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
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
        public void UpdateLineTrip(LineTrip lineTrip)
        {
            try
            {
                dl.UpdateLineTrip((DO.LineTrip)lineTrip.CopyPropertiesToNew(typeof(DO.LineTrip)));
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

        #region Passenger
        public Passenger GetPassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void AddPassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void UpdatePassenger(string name, string password, string newName, string newPassword)
        {
            throw new NotImplementedException();
        }
        public void DeletePassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Station

        /// <summary>
        /// adds new station to the data base
        /// </summary>
        /// <param name="station">the station to add</param>
        /// <exception cref="LocationOutOfRange">if the location of the station is not in the allowable range</exception>
        public void AddStation(BO.Station station)
        {
            const double max_latitude = 33.3;
            const double min_latitude = 31;
            const double max_longitude = 35.5;
            const double min_longitude = 34.3;


            //check if the location is valid
            if(station.Longitude > max_longitude || station.Longitude < min_longitude ||
               station.Latitude > max_latitude || station.Latitude < min_latitude)
            {
                throw new LocationOutOfRange($"unvalid location, location range is: longitude: [{min_longitude} - {max_longitude}] ,latitude: [{min_latitude} - {max_latitude}]");
            }

            //creates a DO.Station to add to dl
            DO.Station DOstation = (DO.Station)station.CopyPropertiesToNew(typeof(DO.Station));

            try
            {
                dl.AddStation(DOstation);
            }
            catch(DO.DuplicateExeption)//if station with identical code allready exist
            {
                throw new DuplicateExeption("station with identical code allready exist");
            }
        }
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
        public void UpdateStation(Station station)
        {
            throw new NotImplementedException();
        }
        public void DeleteStation(int code)
        {
            try
            {
                dl.DeleteStation(code);
                foreach (DO.LineStation lineS in dl.GetAllLineStationsBy(s => s.StationNumber == code))
                {
                    dl.DeleteLineStation(lineS.LineId, lineS.StationNumber);
                }
                foreach (DO.AdjacentStations AjaS in dl.GetAllAdjacentStationsBy(a => a.StationCode1 == code || a.StationCode2 == code))
                {
                    dl.DeleteAdjacentStations(AjaS);
                }
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        public IEnumerable<Station> GetAllStations()
        {
            return (from stationDO in dl.GetAllStations()
                    select (BO.Station)stationDO.CopyPropertiesToNew(typeof(BO.Station)));
        }
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> pred)
        {
            return from stationDO in dl.GetAllStations()
                   let stationBO = GetStation(stationDO.Code)
                   where pred(stationBO)
                   select stationBO;
        }
        #endregion

        #region User Trip
        public void AddUserTrip(UserTrip userTrip)
        {
            throw new NotImplementedException();
        }

        public UserTrip GetUserTrip(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<UserTrip> GetAllUserTripsBy(string name,Predicate<UserTrip> pred)
        {
            throw new NotImplementedException();
        } 
        #endregion

    }
}
