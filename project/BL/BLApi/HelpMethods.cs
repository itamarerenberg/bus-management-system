using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DLApi;
using System.Device.Location;
using System.Threading;

namespace BL.BLApi
{
    public static class HelpMethods
    {
        static IDL dl = DLFactory.GetDL();
        private static readonly Random r = new Random(DateTime.Now.Millisecond);
        static readonly TimeSpan time_refuling = TimeSpan.FromSeconds(12);
        static readonly TimeSpan time_treatment = TimeSpan.FromSeconds(144);

        #region AdjacentStations
        /// <summary>
        /// generating new DO Adjacent Stations
        /// </summary>
        /// <param name="stationCode1"></param>
        /// <param name="stationCode2"></param>
        /// <returns>true: if success.  false: if any of the given parameters is null</returns>
        public static bool AddAdjacentStations(int? stationCode1, int? stationCode2, TimeSpan time = new TimeSpan())
        {
            if (stationCode1 == null || stationCode2 == null)
                return false;

            //getting the DO stations
            DO.Station tempS1 = dl.GetStation((int)stationCode1);
            DO.Station tempS2 = dl.GetStation((int)stationCode2);

            //getting the locations by Latitude and Longitude of the stations
            var locationStation1 = new GeoCoordinate(tempS1.Latitude, tempS1.Longitude);
            var locationStation2 = new GeoCoordinate(tempS2.Latitude, tempS2.Longitude);

            //calculating the distance between the stations
            double distance = locationStation1.GetDistanceTo(locationStation2);//distance in meters

            //generating DO Adjacent Stations
            DO.AdjacentStations adjacentStationsDO = new DO.AdjacentStations()
            {
                StationCode1 = (int)stationCode1,
                StationCode2 = (int)stationCode2,
                Distance = distance                          
            };
            if (time != new TimeSpan())
                adjacentStationsDO.Time = TimeSpan.FromSeconds(r.Next(5, 15) * distance);//calculating the time by distance driving around 20 - 50 kmh

            try
            {
                dl.AddAdjacentStations(adjacentStationsDO);
            }
            catch (Exception)
            {
                return false;
            } 
            return true;
        }
        public static bool AddAdjacentStations(AdjacentStations adjacentStations)
        {
            try
            {
                dl.AddAdjacentStations((DO.AdjacentStations)adjacentStations.CopyPropertiesToNew(typeof(DO.AdjacentStations)));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static AdjacentStations GetAdjacentStations(int? stationCode1,int? stationCode2)
        {
            if (stationCode1 == null || stationCode2 == null)
            {
                return null;
            }
            try
            {
                AdjacentStations adjacentStations = (AdjacentStations)dl.GetAdjacentStation(stationCode1,stationCode2).CopyPropertiesToNew(typeof(AdjacentStations));

                //get DO stations
                DO.Station tempS1 = dl.GetStation(adjacentStations.StationCode1);
                DO.Station tempS2 = dl.GetStation(adjacentStations.StationCode2);

                //get the locations by Latitude and Longitude of the station
                var locationStation1 = new GeoCoordinate(tempS1.Latitude, tempS1.Longitude);
                var locationStation2 = new GeoCoordinate(tempS2.Latitude, tempS2.Longitude);

                //calculating the distance between the stations
                adjacentStations.Distance = locationStation1.GetDistanceTo(locationStation2);//distance in meters

                return adjacentStations;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        
        //void UpdateAdjacentStations(AdjacentStations adjacentStations);
        public static bool DeleteAdjacentStations(AdjacentStations adjacentStations)
        {
            return dl.DeleteAdjacentStations(adjacentStations.StationCode1, adjacentStations.StationCode2);
        }
        #endregion

        #region Line station
        public static LineStation GetLineStation(int lineId, int stationNum)
        {
            DO.LineStation lineStationDO = dl.GetLineStation(lineId, stationNum);
            LineStation lineStation = (LineStation)lineStationDO.CopyPropertiesToNew(typeof(LineStation));

            lineStation.PrevToCurrent = GetAdjacentStations(lineStationDO.PrevStation, stationNum);
            lineStation.CurrentToNext = GetAdjacentStations(stationNum, lineStationDO.NextStation);

            return lineStation;
        }
        public static IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> pred)
        {
            try
            {
                return from lineStationDO in dl.GetAllLineStations()
                       let lineStationBO = GetLineStation(lineStationDO.LineId,lineStationDO.StationNumber)
                       where pred(lineStationBO)
                       select lineStationBO;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }
        #endregion
        public static void Ride(this Bus bus, float km)
        {
            //check if the input is valid
            if (km > 1200)
            {
                throw new ArgumentException("can not preform a ride over then 1200 km");
            }

            //check if busy
            if (bus.IsBusy)
            {
                throw new Busy("the bus is busy");
            }
            if (bus.Stat == BusStatus.Need_treatment)
            {
                throw new NeedTreatment("the bus need tratment");
            }
            //check if the last treatment was less then one year
            if (DateTime.Now - bus.LastTreatDate > new TimeSpan(365, 0, 0, 0))
            {
                bus.Stat = BusStatus.Need_treatment;
                throw new NeedTreatment("the bus need treatment");
            }

            //crheck if this ride will cose to pass the 20,000km from last treatment
            if (bus.KmAfterTreat + km > 20000)
            {
                throw new Danger("this ride will over the 20,000 km from the last treatment");
            }

            //check if there is enough fule for the ride
            if (bus.Fuel < km)
            {
                throw new NotEnoughFule("there is not enough fuel for this ride");
            }

            //update the km end the fule
            bus.Fuel -= km;
            bus.KmAfterTreat += km;
            bus.Kilometraz += km;
            bus.Stat = BusStatus.Traveling;
            int time = (int)((km / new Random().Next(20, 50)) * 6);//in seconds
            bus.TimeUntilReady = new TimeSpan(0, 0, time);
        }

        public static void Refule(this Bus bus)
        {
            //check if busy
            if (bus.IsBusy)
            {
                throw new ArgumentException("you cannot refuel the bus while driving or treatmenting");
            }

            bus.Stat = BusStatus.In_refueling;//set the stat to refueling
            bus.TimeUntilReady = time_refuling;//set Time_until_ready to 'time_refuling' secondes
            bus.Fuel = 1200;//set the Fule to 1200
        }

        public static void Treatment(this Bus bus)
        {
            //check if busy
            if (bus.IsBusy)
            {
                throw new ArgumentException("you cannot refuel the bus while driving or treatmenting");
            }

            bus.LastTreatDate = DateTime.Now;//set LastTreatDate to now
            bus.KmAfterTreat = 0;//set KmAfterTreat to 0
            bus.Stat = BusStatus.In_treatment;//set stat to be IN_TREATMENT
            bus.TimeUntilReady = time_treatment;//set Time_until_ready to 'time_treatment' seconds
        }

        /// <summary>
        /// telling for how long the buss status will nod be "ready"
        /// </summary>
        public static void  Set_TimeUntilReady(this Bus bus, TimeSpan time)
        {
            bus.TimeUntilReady = time;
            
                new Thread(() =>
                {
                    while (bus.TimeUntilReady > new TimeSpan(0, 0, 0))
                    { 
                        Thread.Sleep(1000);
                        bus.TimeUntilReady = new TimeSpan(hours: 0, minutes: 0, seconds: (int)bus.TimeUntilReady.TotalSeconds - 1);//subtruct 1 from Seconds_until_ready 
                    }
                    Thread.Sleep(1000);
                    bus.Stat = (DateTime.Now - bus.LastTreatDate > new TimeSpan(365, 0, 0, 0)) ? BusStatus.Need_treatment : BusStatus.Ready;//change stat to "READY" or "IN_TREATMENT" 
                }).Start();

        }

        public static Bus RandomBus(float fuel = -1, double km = -1, TimeSpan time = new TimeSpan())
        {
            DateTime LDate = new DateTime(2018 + r.Next(2), r.Next(1, 12), r.Next(1, 28));//random Licenes date
            float RFuel = (fuel == -1 || fuel > 1200) ? r.Next(1200) : fuel;
            double Km = km == -1 ? r.Next(20000) : km;
            DateTime Time = time == new TimeSpan() ? DateTime.Today.AddDays(r.Next(-400, 0)) : DateTime.Today - time;//LastTreatDate

            while (LDate > Time)//make sure the last treatment date is after the start date
            {
                LDate = new DateTime(2018 + r.Next(2), r.Next(1, 12), r.Next(1, 28));
            }
            //r.Next(10000000, 99999999).ToString(), SDate, Km, r.Next(40000, 100000), Fuel, Time
            return new Bus() {
                LicenseNumber = r.Next(10000000, 99999999).ToString(),
                LicenesDate = LDate,
                Kilometraz = r.Next(40000, 100000),
                LastTreatDate = Time,
                Fuel = RFuel,
                KmAfterTreat = Km,
                Stat = DateTime.Today - Time < new TimeSpan(365, 0, 0, 0) ? BusStatus.Ready : BusStatus.Need_treatment
            };
        }
        #region Bus treatment

        #endregion
    }
}
