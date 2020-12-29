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
                if (user.Password != password)
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
                    Admin = true
                };
                dl.AddUser(tempUser);
            }
            catch (DO.DuplicateExeption msg)
            {
                throw msg.InnerException;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void UpdateManagar(string name, string password)
        {
            throw new NotImplementedException();
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
                Bus tempBus = (Bus)dl.GetBus(licensNum).CopyPropertiesToNew(typeof(Bus));
                return tempBus;
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

        public Line GetLine(int id);
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
        public void DeleteLine(Line id);
        public IEnumerable<Line> GetAllLines();
        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> pred);
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
        public void DeleteLineStation(LineStation lineStation, int index); 
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
            int max_longitude = 30;
            int min_longitude = -30;
            int max_latitude = 30;
            int min_latitude = -30;

            //check if the location is valid
            if(station.Longitude > max_longitude || station.Longitude < min_longitude ||
               station.Latitude > max_latitude || station.Latitude < min_latitude)
            {
                throw new LocationOutOfRange($"unvalid location, location range is: longitude: [{min_longitude} - {max_longitude}] ,latitude: [{min_latitude} - {max_latitude}]");
            }

            //creates a DO.BusStation to add to dl
            DO.BusStation DOstation = (DO.BusStation)station.CopyPropertiesToNew(typeof(DO.BusStation));

            try
            {
                dl.AddBusStation(DOstation);
            }
            catch(DO.DuplicateExeption)//if station with identical code allready exist
            {
                throw new DuplicateExeption("station with identical code allready exist");
            }
        }

        public Station GetStation()

        public void DeleteStation(int Code)
        {
            Station station = 
        }
        #endregion
    }
}
