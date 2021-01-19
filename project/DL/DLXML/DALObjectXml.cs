using System;
using System.Collections.Generic;
using System.Linq;
using DLApi;
using DLXML;
using DO;
using System.Xml.Linq;

namespace DL
{
    sealed class DALObjectXml : IDL
    {
        #region singelton

        static readonly DALObjectXml instance = new DALObjectXml();
        static DALObjectXml() { }
        DALObjectXml() { }
        public static DALObjectXml Instance { get => instance; }

        #endregion

        #region Bus
        public void AddBus(Bus bus)
        {
            XElement Same_LicenseNum = (from b in DataSourceXML.Buses.Elements()
                                        where b.Element("LicenseNum").Value == bus.LicenseNum
                                        select b).FirstOrDefault();
            if (Same_LicenseNum == null)//if no bus have the same LicenseNum
            {
                DataSourceXML.Buses.Add(bus.to_new_xelement("Bus"));//add the bus to the data source
            }
            else//in case the bus is allready in the data base: checks if he is active
            {
                if (Same_LicenseNum.Element("IsActive").Value == true.ToString())//if the bus is activ
                {
                    throw new DuplicateExeption("bus with identical License's num allready exist");
                }
                Same_LicenseNum.Element("IsActive").Value = true.ToString();
            }
            DataSourceXML.Save("Buses");//Save Chenges
        }
        public Bus GetBus(string licenseNum)
        {
            XElement bus = (from b in DataSourceXML.Buses.Elements()//get the bus from the data source
                            where b.Element("LicenseNum").Value == licenseNum
                            select b).FirstOrDefault();

            if (bus == null)//if ther is no such bus in the data source
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }

            Cloning.xelement_to_object(bus, out Bus ret);//create a new bus's instence with the content of bus
            return ret;//                       ^^^^^^^
        }

        public void UpdateBus(Bus newBus)
        {
            XElement oldBus = (from b in DataSourceXML.Buses.Elements()//get the old bus from the data source acording to the licens num
                               where b.Element("LisenceNum").Value == newBus.LicenseNum
                               select b).FirstOrDefault();

            if (oldBus == null)//if ther is no such bus
            {
                throw new NotExistExeption("bus with License's num like 'bus' not exist");
            }
            Cloning.object_to_xelement(newBus, oldBus);//insert the diteils of the update bus to the xelement of the old bus
            DataSourceXML.Save("Buses");//save the changes
        }

        public void DeleteBus(string licenseNum)
        {
            XElement bus = (from b in DataSourceXML.Buses.Elements()//search in the data source to a bus with this licenseNum that active
                            where b.Element("LicenseNum").Value == licenseNum && b.Element("IsActive").Value == true.ToString()
                            select b).FirstOrDefault();
            if (bus != null)//if such bus founded
            {
                bus.Element("IsActive").Value = false.ToString();//set the bus to be unactive
            }
            else
            {
                throw new NotExistExeption("bus with this License's num not exist");
            }
            DataSourceXML.Save("Buses");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from b in DataSourceXML.Buses.Elements()//return all the buses
                   select Cloning.xelement_to_new_object<Bus>(b);
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from b in DataSourceXML.Buses.Elements()
                   let temp = Cloning.xelement_to_new_object<Bus>(b)//create a Bus's instence from b so it can send to predicate 
                   where predicate(temp) && temp.IsActive == true
                   select temp;
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
            DataSourceXML.Lines.Add(line.to_new_xelement("Line"));
            DataSourceXML.Save("Lines");
            return line.ID;//return the id that given to line from SerialNumbers.GetLineId
        }

        public Line GetLine(int id)
        {
            XElement line = (from l in DataSourceXML.Lines.Elements()//get the line from the sdata source
                             where l.Element("ID").Value == id.ToString() &&
                                   l.Element("IsActive").Value == true.ToString()
                             select l).FirstOrDefault();
            if (line == null)//if such line dosen't found
            {
                throw new NotExistExeption("line with this id not exist");
            }
            return Cloning.xelement_to_new_object<Line>(line);//return the line
        }

        public void UpdateLine(Line newLine)
        {
            XElement oldLine = (from l in DataSourceXML.Lines.Elements()//serch for the old line in the data source
                                where l.Element("ID").Value == newLine.ID.ToString() &&
                                      l.Element("IsActive").Value == true.ToString()
                                select l).FirstOrDefault();
            if (oldLine == null)//if ther is no such line
            {
                throw new NotExistExeption("line with id like 'line' not exist");
            }
            Cloning.object_to_xelement(newLine, oldLine);//insert the ditailes of the new line to the xelement of the old line
            DataSourceXML.Save("Lines");
        }

        public void DeleteLine(int id)
        {
            XElement line = (from l in DataSourceXML.Lines.Elements()//serch for the line in the data source where the line is active
                             where l.Element("ID").Value == id.ToString() &&
                                   l.Element("IsActive").Value == true.ToString()
                             select l).FirstOrDefault();
            if (line != null)//if the line found
            {
                line.Element("IsActive").Value = false.ToString();//set the line to be unactive
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from l in DataSourceXML.Lines.Elements()//retuns all the active buses in the data source
                   where l.Element("IsActive").Value == true.ToString()
                   select Cloning.xelement_to_new_object<Line>(l);
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return from l in DataSourceXML.Lines.Elements()
                   let temp = Cloning.xelement_to_new_object<Line>(l)//create a Line's instence from l so it can be send to predicate
                   where temp.IsActive == true && predicate(temp)
                   select temp;
        }

        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSourceXML.BusesOnTrip.Elements().FirstOrDefault(bot => int.Parse(bot.Element("ID").Value) == busOnTrip.ID) != null)
            {
                throw new DuplicateExeption("the bus is allready in driving");
            }
            else
            {
                DataSourceXML.BusesOnTrip.Add(busOnTrip.to_new_xelement("BusOnTrip"));//add the busBusesOnTrip
                DataSourceXML.Save("BusesOnTrip");
            }
        }

        public BusOnTrip GetBusOnTrip(int id)
        {
            XElement busOnTrip = (from xebot in DataSourceXML.BusesOnTrip.Elements()
                                  where int.Parse(xebot.Element("ID").Value) == id
                                  select xebot).FirstOrDefault();
            if (busOnTrip == null)//if no such busOnTrip
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            return XMLTools.xelement_to_new_object<BusOnTrip>(busOnTrip);
        }

        public void UpdateBusOnTrip(BusOnTrip newBusOnTrip)
        {
            XElement oldBusOnTrip = (from xebot in DataSourceXML.BusesOnTrip.Elements()
                                     where int.Parse(xebot.Element("ID").Value) == newBusOnTrip.ID
                                     select xebot).FirstOrDefault();
            if (oldBusOnTrip == null)//if ther is no such BusOnTrip
            {
                throw new NotExistExeption("the bus is not in driving");
            }
            XMLTools.object_to_xelement(newBusOnTrip, oldBusOnTrip);
            DataSourceXML.Save("BusesOnTrip");
        }

        public void DeleteBusOnTrip(int id)
        {
            XElement busOnTrip = (from xebot in DataSourceXML.BusesOnTrip.Elements()
                                  where int.Parse(xebot.Element("ID").Value) == id
                                  && bool.Parse(xebot.Element("IsActive").Value)
                                  select xebot).FirstOrDefault();

            if (busOnTrip != null)//if the BusOnTrip found
            {
                busOnTrip.Element("IsActive").Value = false.ToString();
                DataSourceXML.Save("BusOnTrip");
            }
            else
            {
                throw new NotExistExeption("line with this id not exist");
            }
        }

        public IEnumerable<BusOnTrip> GetAllBusesOnTrip()
        {
            return from xebot in DataSourceXML.BusesOnTrip.Elements()
                   where bool.Parse(xebot.Element("IsActive").Value)
                   select XMLTools.xelement_to_new_object<BusOnTrip>(xebot);
        }

        public IEnumerable<BusOnTrip> GetAllBusesOnTripBy(Predicate<BusOnTrip> predicate)
        {
            return from xebot in DataSourceXML.BusesOnTrip.Elements()
                   let bot = XMLTools.xelement_to_new_object<BusOnTrip>(xebot)
                   where predicate(bot)
                   select bot;
        }
        #endregion

        #region Station

        public void AddStation(Station station)
        {
            XElement tempStation = (from st in DataSourceXML.Stations.Elements()//serch for this station in the data source
                                       where int.Parse(st.Element("Code").Value) == station.Code
                                             && st.Element("IsActive").Value == true.ToString()
                                       select st).FirstOrDefault();
            if (tempStation == null)//if ther is no such station
            {
                DataSourceXML.Stations.Add(station.to_new_xelement("Station"));//add the station to the data source
            }
            else if (!bool.Parse(tempStation.Element("IsActive").Value))//in case the station is allready in the data base: if it's unactive
            {
                XMLTools.object_to_xelement(station, tempStation);//insert the station insted of the unactive station
            }
            else//else throw exption of duplicate
            {
                throw new DuplicateExeption("bus station with identical License's num allready exist");
            }
            DataSourceXML.Save("Stations");
        }

        public Station GetStation(int code)
        {
            XElement xeStation = (from st in DataSourceXML.Stations.Elements()//get the station from the data source wher it's active
                                   where bool.Parse(st.Element("IsActive").Value)
                                         && int.Parse(st.Element("Code").Value) == code
                                   select st).FirstOrDefault();
            if (xeStation == null)//if ther is no such station
            {
                throw new NotExistExeption("bus's station with this code not exist");
            }
            return XMLTools.xelement_to_new_object<Station>(xeStation);//create and return an instence of Station from xeStation
        }

        public void UpdateStation(Station newStation)
        {
            XElement oldStation = (from st in DataSourceXML.Stations.Elements()//get the old station from the data source
                                      where int.Parse(st.Element("Code").Value) == newStation.Code
                                            && st.Element("IsActive").Value == true.ToString()
                                      select st).FirstOrDefault();
            if (oldStation == null)//if ther is no such station 
            {
                throw new NotExistExeption("the station doesn't not exist");
            }
            XMLTools.object_to_xelement(newStation, oldStation);//insert the new station insted the old station
            DataSourceXML.Save("Stations");
        }

        public void DeleteStation(int code)
        {
            XElement station = (from st in DataSourceXML.Stations.Elements()//get the station from the data source
                                where st.Element("Code").Value == code.ToString()
                                      && st.Element("IsActive").Value == true.ToString()
                                select st).FirstOrDefault();
            if (station != null)//if the station found
            {
                station.Element("IsActive").Value = false.ToString();
            }
            else//if ther is no such station
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            DataSourceXML.Save("Stations");//save the changes
        }

        public IEnumerable<Station> GetAllStations()
        {
            return from st in DataSourceXML.Stations.Elements()//get and return all the active station from the data source 
                   where bool.Parse(st.Element("IsActive").Value)
                   select XMLTools.xelement_to_new_object<Station>(st);//create a new instance of station from 'st'
        }

        public IEnumerable<Station> GetAllStationBy(Predicate<Station> predicate)
        {
            return from st in DataSourceXML.Stations.Elements()
                   let temp = Cloning.xelement_to_new_object<Station>(st)//create a new Station's instance so it can send to predicate
                   where temp.IsActive == true
                         && predicate(temp)
                   select temp;
        }
        #endregion

        #region LineStation

        public void AddLineStation(LineStation lineStation)
        {
            XElement tempLineStation = (from ls in DataSourceXML.LineStations.Elements()//serch for this line station in the data source
                                        where ls.Element("LineId").Value == lineStation.LineId.ToString()
                                              && ls.Element("StationNumber").Value == lineStation.StationNumber.ToString()
                                        select ls).FirstOrDefault();
            if (tempLineStation == null)//if ther isn't such line station in the data source
            {
                DataSourceXML.LineStations.Add(lineStation.to_new_xelement("LineStation"));//add the line station
            }
            else if (!bool.Parse(tempLineStation.Element("IsActive").Value))//in case the Line station is allready in the data base: if the line station in the data base is unactive
            {
                XMLTools.object_to_xelement(lineStation, tempLineStation);//insert the new line station insted of the old one
            }
            else
            {
                throw new DuplicateExeption("the line station is allready exist");
            }
            DataSourceXML.Save("LineStation");//save the changes
        }

        public LineStation GetLineStation(int lineId, int stationNum)
        {
            XElement lineStation = (from ls in DataSourceXML.LineStations.Elements()//get the line station from the data source
                                    where ls.Element("LineId").Value == lineId.ToString()
                                          && ls.Element("StationNumber").Value == stationNum.ToString()
                                    select ls).FirstOrDefault();
            if (lineStation == null)//if ther isn't such station 
            {
                throw new NotExistExeption("the line doesn't exist or it doesn't have such a station");
            }
            return XMLTools.xelement_to_new_object<LineStation>(lineStation);//create and return a new instance of LineStation from 'lineStation'
        }

        public LineStation GetLineStationByIndex(int lineId, int index)
        {
            XElement lineStation = (from ls in DataSourceXML.LineStations.Elements()//serch in the data source for a LineStation where LineId == lineId and Index == index
                                    where int.Parse(ls.Element("LineId").Value) == lineId
                                          && int.Parse(ls.Element("LineStationIndex").Value) == index
                                    select ls).FirstOrDefault();
            if (lineStation == null)//if ther is no such LineStation in the data source
            {
                throw new NotExistExeption("the line doesn't exist or the index is out of range");
            }
            return XMLTools.xelement_to_new_object<LineStation>(lineStation);//create and return a new instance of LineStation from 'lineStation'
        }

        public void UpdateLineStation(LineStation newLineStation)
        {
            XElement oldLineStation = (from ls in DataSourceXML.LineStations.Elements()//search for the line station in the data source
                                       where ls.Element("LineId").Value == newLineStation.LineId.ToString()
                                             && ls.Element("StationNumber").Value == newLineStation.StationNumber.ToString()
                                       select ls).FirstOrDefault();
            if (oldLineStation == null)//if ther is no such station in th data source
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            XMLTools.object_to_xelement(newLineStation, oldLineStation);//insert the new line station insted of the old one
            DataSourceXML.Save("LineStations");//save changes
        }

        public void DeleteLineStation(int lineId, int stationNum)
        {
            XElement lineS = (from ls in DataSourceXML.LineStations.Elements()//serch for the line station in the data source
                              where ls.Element("LineId").Value == lineId.ToString()
                                    && ls.Element("StationNumber").Value == stationNum.ToString()
                              select ls).FirstOrDefault();
            if (lineS != null)//if found
            {
                lineS.Element("IsActive").Value = false.ToString();//set the line station to be unactive
            }
            else
            {
                throw new NotExistExeption("the line station doesn't exist");
            }
            DataSourceXML.Save("LineStations");
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from ls in DataSourceXML.LineStations.Elements()//return all the active stations in the data source
                   where bool.Parse(ls.Element("IsActive").Value)
                   select XMLTools.xelement_to_new_object<LineStation>(ls);
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            return from ls in DataSourceXML.LineStations.Elements()
                   let temp = Cloning.xelement_to_new_object<LineStation>(ls)//create a new instence of LineStation from 'ls' so it can send to peredicate
                   where predicate(temp)
                   select temp;
        }
        #endregion
        
        #region AdjacentStations

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            if (adjacentStations == null)
                return;
            XElement tempStations = (from adjSt in DataSourceXML.AdjacentStations.Elements()//search for the AdjacentStations in the data source
                                     where int.Parse(adjSt.Element("StationCode1").Value) == adjacentStations.StationCode1
                                           && int.Parse(adjSt.Element("StationCode2").Value) == adjacentStations.StationCode2
                                     select adjSt).FirstOrDefault();
            if (tempStations == null)//if the isn't such AdjacentStations in the data source
            {
                DataSourceXML.AdjacentStations.Add(adjacentStations.to_new_xelement("AdjacentStations"));
                DataSourceXML.Save("AdjacentStations");
            }
            //else do nothing
        }

        public AdjacentStations GetAdjacentStation(int? stationCode1, int? stationCode2)
        {
            XElement tempStations = (from adjSt in DataSourceXML.AdjacentStations.Elements()//get the AdjacentStations from the data source
                                     where int.Parse(adjSt.Element("StationCode1").Value) == stationCode1
                                           && int.Parse(adjSt.Element("StationCode2").Value) == stationCode2
                                     select adjSt).FirstOrDefault();
            if (tempStations == null)//if ther is no such AdjacentStations in the data source
                return null;
            return XMLTools.xelement_to_new_object<AdjacentStations>(tempStations);//create and return a new instance of AdjacentStations from 'tempStations'
        }

        public void UpdateAdjacentStations(AdjacentStations newAdjacentStations)
        {
            XElement oldAdjacentStations = (from adjSt in DataSourceXML.AdjacentStations.Elements()//get the old AdjacentStations from the data source
                                            where int.Parse(adjSt.Element("StationCode1").Value) == newAdjacentStations.StationCode1
                                                  && int.Parse(adjSt.Element("StationCode2").Value) == newAdjacentStations.StationCode2
                                            select adjSt).FirstOrDefault();
            if (oldAdjacentStations == null)//if ther is no such AdjacentStations in the data source
            {
                throw new NotExistExeption("the Adjacent Stations doesn't exist");
            }
            XMLTools.object_to_xelement(newAdjacentStations, oldAdjacentStations);//insert to the data source the new AdjacentStations insted of the old one
            DataSourceXML.Save("AdjacentStations");//save changes
        }

        public bool DeleteAdjacentStations(int? stationCode1, int? stationCode2)
        {
            XElement tempAdjacentStations = (from adjSt in DataSourceXML.AdjacentStations.Elements()//search for the AdjacentStations in the data source
                                             where int.Parse(adjSt.Element("StationCode1").Value) == stationCode1
                                                   && int.Parse(adjSt.Element("StationCode2").Value) == stationCode2
                                             select adjSt).FirstOrDefault();
            if (tempAdjacentStations == null)//if ther is no such AdjacentStations in the data source
                return false;
            //else
            tempAdjacentStations.Element("IsActive").Value = false.ToString();//set the AdjacentStations to be unactive
            DataSourceXML.Save("AdjacentStations");
            return true;
        }

        /// <returns>true: if the object deleted sucsesfuly, false: if the object dont exist or allready not active</returns>
        public bool DeleteAdjacentStations(AdjacentStations adjacentStations)
        {
            XElement temp = (from adjSt in DataSourceXML.AdjacentStations.Elements()//extracting the object from the data base
                             where int.Parse(adjSt.Element("StationCode1").Value) == adjacentStations.StationCode1
                                   && int.Parse(adjSt.Element("StationCode2").Value) == adjacentStations.StationCode2
                             select adjSt).FirstOrDefault();

            if (temp == null)//if this object dont exist
            {
                return false;
            }
            if (bool.Parse(temp.Element("IsActive").Value) == false)//if this object is allready not active
            {
                return false;
            }
            temp.Element("IsActive").Value = false.ToString();//set the object to be not active
            DataSourceXML.Save("AdjacentStations");//save changes
            return true;
        }

        ///<returns>all the objects where predicate returns true for them(active and not active)</returns>
        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adjSt in DataSourceXML.AdjacentStations.Elements()
                   let temp = Cloning.xelement_to_new_object<AdjacentStations>(adjSt)//create a AdjacentStations's instance from 'adjSt' so it can send to predicate 
                   where predicate(temp)
                   select temp;
        }


        #endregion
        //******************************************************************************
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
            if (temp == null)
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


