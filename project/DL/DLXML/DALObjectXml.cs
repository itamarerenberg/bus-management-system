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
            var ret = from st in DataSourceXML.Stations.Elements()//get and return all the active station from the data source 
                      where bool.Parse(st.Element("IsActive").Value)
                      select XMLTools.xelement_to_new_object<Station>(st);//create a new instance of station from 'st'
            DataSourceXML.Save("Stations");
            return ret;
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

            var ret = from ls in DataSourceXML.LineStations.Elements()//return all the active stations in the data source
                      where bool.Parse(ls.Element("IsActive").Value)
                      select XMLTools.xelement_to_new_object<LineStation>(ls);
            DataSourceXML.Save("LineStations");
            return ret;
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            var ret = from ls in DataSourceXML.LineStations.Elements()
                   let temp = Cloning.xelement_to_new_object<LineStation>(ls)//create a new instence of LineStation from 'ls' so it can send to peredicate
                   where predicate(temp)
                   select temp;
            DataSourceXML.Save("LineStations");
            return ret;
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
        
        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            lineTrip.ID = SerialNumbers.GetLineTripId;//get the serial id for this line trip
            DataSourceXML.LineTrips.Add(lineTrip.to_new_xelement("LineTrip"));
            DataSourceXML.Save("LineTrips");
        }

        public LineTrip GetLineTrip(int id)
        {
            XElement temp = (from lt in DataSourceXML.LineTrips.Elements()//serch for this line trip in the data source
                             where lt.Element("ID").Value == id.ToString()
                             select lt).FirstOrDefault();
            if (temp == null)//if the is no such line trip
            {
                throw new NotExistExeption("the Line trip doesn't exist");
            }
            //else
            return XMLTools.xelement_to_new_object<LineTrip>(temp);//create and return a new LineTrip instance from temp
        }

        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            XElement oldLineTrip = (from lt in DataSourceXML.LineTrips.Elements()//get the old line trip from the data source
                                    where lt.Element("ID").Value == newLineTrip.ID.ToString()
                                    select lt).FirstOrDefault();

            if (oldLineTrip == null)//if the is no such line trip in the data source
            {
                throw new NotExistExeption("the line trip doesn't exist");
            }
            //else
            XMLTools.object_to_xelement(newLineTrip, oldLineTrip);//override the old lineTrip with the new one in the data source 
            DataSourceXML.Save("LineTrips");//save the changes
        }

        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from lt in DataSourceXML.LineTrips.Elements()//return all the active lineTrips from the data source
                   where lt.Element("IsActive").Value == true.ToString()
                   select Cloning.xelement_to_new_object<LineTrip>(lt);
        }

        public IEnumerable<LineTrip> GetAllLineTripBy(Predicate<LineTrip> predicate)
        {
            return from lt in DataSourceXML.LineTrips.Elements()//get all the line trips where predicate() return true
                   let temp = Cloning.xelement_to_new_object<LineTrip>(lt)//create an instance of lineTrip from lt so it can bew send to predicate
                   where predicate(temp)
                   select temp;
        }
        #endregion

        #region User
        public void AddUser(User user)
        {
            XElement tempUser = (from u in DataSourceXML.Users.Elements()//serch for this user in the data source
                             where bool.Parse(u.Element("IsActive").Value)
                             select u).FirstOrDefault();
            if (tempUser == null)//if ther is no such user allready in the data source
            {
                DataSourceXML.Users.Add(user.to_new_xelement("User"));//add the user
                DataSourceXML.Save("Users");
            }
            //in case the user is allready in the data base: checks if he is active
            else if (!bool.Parse(tempUser.Element("IsActive").Value))//if the user in the data base is unactive
            {
                DataSourceXML.Save("Users");
                XMLTools.object_to_xelement(user, tempUser);//override the unactive user with the new user
            }
            else//if the user in the data base is active
            {
                throw new DuplicateExeption("the user is allready exist");
            }
           
        }

        public User GetUser(string name)
        {
            XElement tempUser = (from u in DataSourceXML.Users.Elements()
                             where u.Element("Name").Value == name
                             && bool.Parse(u.Element("IsActive").Value)
                             select u).FirstOrDefault();
            if (tempUser == null)//if the is no such user in the data source
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            //else
            return XMLTools.xelement_to_new_object<User>(tempUser);//create and return a new instance of User from 'tempUser' 
        }

        public void UpdateUser(User newUser)
        {
            XElement oldUser = (from u in DataSourceXML.Users.Elements()//serch for the old user in the data source
                                where u.Element("Name").Value == newUser.Name
                                && bool.Parse(u.Element("IsActive").Value)
                                select u).FirstOrDefault();
            if (oldUser == null)//if thr is no such user in the data source
            {
                throw new NotExistExeption("the user doesn't exist");
            }
            XMLTools.object_to_xelement(newUser, oldUser);//override the old user with the new user
            DataSourceXML.Save("Users");//save the changes
        }

        public void DeleteUser(string name)
        {
            XElement user = (from u in DataSourceXML.Users.Elements()//serch for the user in the data source
                             where u.Element("Name").Value == name
                                   && bool.Parse(u.Element("IsActive").Value)
                             select u).FirstOrDefault();
            if (user != null)//if the user is found
            {
                user.Element("IsActive").Value = false.ToString();//set the user to be unactive
            }
            else//if the user not found
            {
                throw new NotExistExeption("the user doesn't exist");
            }
        }
        #endregion

        #region UserTrip
        public void AddUserTrip(UserTrip userTrip)
        {
            userTrip.TripId = SerialNumbers.GetUserTripId;//get a serial number from SerialNumbers for the id
            DataSourceXML.UsersTrips.Add(userTrip.to_new_xelement("LineTrip"));//add the userTrip to the data source
            DataSourceXML.Save("UserTrips");//save the changes
        }

        public UserTrip GetUserTrip(int id)
        {
            XElement tempUserTrip = (from ut in DataSourceXML.UsersTrips.Elements()
                                     where int.Parse(ut.Element("TripId").Value) == id
                                     select ut).FirstOrDefault();//.FirstOrDefault(u => u.TripId == id && u.IsActive == true);
            if (tempUserTrip == null)//if thr is no such user trip in the data source
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            //else
            return XMLTools.xelement_to_new_object<UserTrip>(tempUserTrip);//create and return anew instance of UserTrip from tempUserTrip
        }

        public void UpdateUserTrip(UserTrip newUserTrip)
        {
            XElement oldUserTrip = (from ut in DataSourceXML.UsersTrips.Elements()//serch for the old user trip in the data source 
                                    where int.Parse(ut.Element("TripId").Value) == newUserTrip.LineId
                                          && bool.Parse(ut.Element("IsActive").Value)
                                    select ut).FirstOrDefault();
            if (oldUserTrip == null)//if thr is no such user trip in the data source
            {
                throw new NotExistExeption("the user trip doesn't exist");
            }
            //else
            XMLTools.object_to_xelement(newUserTrip, oldUserTrip);//override the old UserTrip with the new UserTrip
            DataSourceXML.Save("UsersTrips");//save changes
        }

        public IEnumerable<UserTrip> GetAllUserTrips()
        {
            return from ut in DataSourceXML.UsersTrips.Elements()//return all the active user trips
                   where bool.Parse(ut.Element("IsActive").Value)
                   select XMLTools.xelement_to_new_object<UserTrip>(ut);
        }

        public IEnumerable<UserTrip> GetAllUserTripsBy(Predicate<UserTrip> predicate)
        {
            return from xeut in DataSourceXML.UsersTrips.Elements()
                   let ut = XMLTools.xelement_to_new_object<UserTrip>(xeut)//create an instance of UserTrip from 'xeut' so it can send to predicate
                   where predicate(ut)
                   select ut;
        }

        #endregion
    }
}


