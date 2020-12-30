using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BLApi;
using BLApi;
using BO;
using DLApi;
//using static BL.BLApi.HelpMethods;// למה זה לא עובד בלי זה ??? הא איתמר?? (המחלקה הסטטית של הפרופרטי עובדת בלי) תבדוק את זה אם תוכל 

namespace BL
{
    public class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();
        
        #region Manager
        public Manager GetManagar(string name, string password)
        {
            try
            {
                //validation
                DO.User user = dl.GetUser(name, password);
                if (user.Password != password)//זה לא יקרא לעולם
                {
                    throw new InvalidPassword("invalid password");
                }

            }
            catch (DO.InvalidObjectExeption)
            {
                throw new InvalidID("invalid ID");
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
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
                DO.User oldManagar = dl.GetUser(oldName, oldPassword);
                if (oldManagar.Admin == false)
                    throw new InvalidInput("the user doesn't have an administrator access");
                DO.User newManagar = new DO.User()
                {
                    Name = newName,
                    Password = newPassword,
                    Admin = true,
                    IsActive = true
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
                DO.Bus tempBus = new DO.Bus()
                {
                    LicenseNum = bus.LicensNumber,
                    LicenesDate = bus.LicenesDate,
                    Kilometraz = bus.Kilometraz,
                    Fule = bus.Fule,
                    Stat = (DO.BusStatus)Enum.Parse(typeof(DO.BusStatus), bus.Stat.ToString())
                };
                dl.AddBus(tempBus);
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
        public void AddLine(Line line)
        {
            try
            {
                dl.AddLine((DO.Line)line.CopyPropertiesToNew(typeof(DO.Line)));//creats DO line from BO line

                foreach (LineStation lStation in line.Stations)
                {
                    dl.AddLineStation((DO.LineStation)lStation.CopyPropertiesToNew(typeof(DO.LineStation)));//creats DO Line Station from BO Line Station
                    dl.AddAdjacentStations((DO.AdjacentStations)lStation.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                }
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public Line GetLine(int id)
        {
            try
            {
                Line line = (Line)dl.GetLine(id).CopyPropertiesToNew(typeof(Line));
                line.Stations = (from lineStationDO in dl.GetAllLineStationBy(l => l.LineId == id)
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
                throw msg.InnerException;
            }
        }
        public void DeleteLine(int id)
        {
            Line line = GetLine(id);
            try
            {
                foreach (LineStation lStation in line.Stations)
                {
                    dl.DeleteLineStation(lStation.LineId, lStation.StationNumber);
                }
                dl.DeleteLine(id);
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

        #region LineStation
        public void AddLineStation(LineStation lineStation, int index = -1)
        {
            if (index < -1)
            {
                throw new InvalidInput("the index couldn't be a negative number");
            }
            if (index == -1)//adding to the end of the line 
            {
                //BO uapdates
                Line line = GetLine(lineStation.LineId);
                lineStation.LineStationIndex = line.Stations.Count();//gets the index of the end of the line
                line.Stations.Add(lineStation);

                //DO uapdates
                dl.AddAdjacentStations((DO.AdjacentStations)lineStation.PrevToCurrent.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                UpdateLine(line);
            }
            if (index == 0)//adding to the first of the line
            {
                //BO uapdates
                lineStation.LineStationIndex = 0;
                Line line = GetLine(lineStation.LineId);
                for (int i = 0; i < line.Stations.Count; i++)//updates the indexes of the stations
                {
                    line.Stations[i].LineStationIndex++;
                }
                line.Stations.Insert(0, lineStation);

                //DO uapdates
                dl.AddAdjacentStations((DO.AdjacentStations)lineStation.CurrentToNext.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations from BO Line Station
                UpdateLine(line);
            }
            else
            {
                //BO uapdates
                lineStation.LineStationIndex = index;
                Line line = GetLine(lineStation.LineId);
                for (int i = index; i < line.Stations.Count; i++)//updates the indexes of the stations
                {
                    line.Stations[i].LineStationIndex++;
                }
                line.Stations.Insert(index, lineStation);

                //DO uapdates
                dl.AddAdjacentStations((DO.AdjacentStations)lineStation.CurrentToNext.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations for the back Line Station
                dl.AddAdjacentStations((DO.AdjacentStations)lineStation.PrevToCurrent.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//creats DO AdjacentStations for the next Line Station
                dl.DeleteAdjacentStations((DO.AdjacentStations)line.Stations[index - 1].CurrentToNext.CopyPropertiesToNew(typeof(DO.AdjacentStations)));//delete the conactions between Stations[index - 1] and Stations[index + 1]
                UpdateLine(line);
            }
        }
        public void UpdateLineStation(LineStation lineStation, int index);
        public void DeleteLineStation(LineStation lineStation, int index)
        {

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
        public void UpdatePassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void DeletePassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        #endregion

        //******************

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
                station.GetLines = (from lineS in dl.GetAllLineStationBy(s => s.LineId == code)
                                    orderby lineS.LineId
                                    select GetLine(lineS.LineId)).ToList();
                return station;
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public void DeleteStation(int code)
        {
            try
            {
                dl.DeleteStation(code);
                foreach (LineStation lineS in HelpMethods.GetAllLineStationsBy(s => s.StationNumber == code))
                {
                    DeleteLineStation(lineS, lineS.StationNumber);
                }
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public IEnumerable<Station> GetAllStations()
        {
            return from stationDO in dl.GetAllStations()
                   select GetStation(stationDO.Code);
        }
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> pred)
        {
            return from stationDO in dl.GetAllStations()
                   let stationBO = GetStation(stationDO.Code)
                   where pred(stationBO)
                   select stationBO;
        }
        #endregion


    }
}
