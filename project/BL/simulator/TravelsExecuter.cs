using BL.BLApi;
using BL.BO;
using BLApi;
using BO;
using DLApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.simulator
{
    class TravelsExecuter
    {
        #region singelton

        TravelsExecuter() 
        {
            source = BLFactory.GetBL("admin");//get a BL instance to load data from the dal(its more convenient then to use directly with dal)
        }
        static TravelsExecuter(){}
        static TravelsExecuter instance;
        static public TravelsExecuter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TravelsExecuter();
                }
                return instance;
            }
        }

        #endregion

        Action<LineTiming> observer;
        List<int> stationsInTrack = new List<int>();//list of all the station that in tracking

        IBL source;

        private List<LineTrip> lineTrips;
        private List<Line> lines;

        SimulationClock clock;

        BackgroundWorker travelsExecuterWorker;
        public void StartExecute(Action<LineTiming> _observer = null)
        {
            if(source == null)
            {
                source = BLFactory.GetBL("admin");
            }

            clock = SimulationClock.Instance;
            observer = _observer != null ? _observer : (LineTiming) => { };//if _observer is null then set observer to be an Action<LineTiming> that do nothing

            //load the lineTrips and lines
            lineTrips = source.GetAllLineTrips().ToList();
            lines = source.GetAllLines().ToList();

            lineTrips.OrderBy(lt => lt.StartTime - clock.Time);//order the travels by the most neer to present
            if(travelsExecuterWorker == null)
            {
                travelsExecuterWorker = new BackgroundWorker();
            }

            travelsExecuterWorker.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                for (int i = 0; !clock.Cancel; i = (i + 1) % lineTrips.Count())//run while !clock.cancel in cycels(0, 1, 2,..., n, 0, 1, 2,...)
                {
                    int timeToNextTravel = (int)((lineTrips[i].StartTime - clock.Time).TotalMilliseconds);
                    Thread.Sleep(timeToNextTravel / clock.Rate);
                    
                    executeTravel(lineTrips[i]);//execute the travel
                }
            };
        }

        private void executeTravel(LineTrip lineTrip)
        {
            BackgroundWorker newTravel = new BackgroundWorker();
            newTravel.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                //get the line of this line trip
                Line line = lines.FirstOrDefault(l => l.ID == lineTrip.LineId);
                string lastStationName = source.GetStation(line.LastStation.StationNumber).Name;//get the name of the last station in this line
                LineStation[] stations = line.Stations.ToArray();
                Dictionary<int, LineTiming> underTruck = new Dictionary<int, LineTiming>();//save the code of all the stations in this line that under truck together with the apropiate lineTiming  

                for (int current = 0; current < stations.Length && !clock.Cancel; current++)
                {
                    //calculate the arival time according to the position of the bus now
                    TimeSpan calcTime = TimeSpan.Zero;
                    for (int i = current; i < stations.Length; i++)
                    {
                        calcTime += stations[i].PrevToCurrent.Time;
                        if (stationsInTrack.Contains(stations[i].StationNumber))//if this station under truck then update the arival time to this station
                        {
                            if (underTruck[stations[i].StationNumber] == null)//if this station new in stationsInTrack(added after the trip started)
                            {
                                underTruck[stations[i].StationNumber] =//add to the under truck dictionery
                                new LineTiming()
                                {
                                    lineNum = line.LineNumber,
                                    LineId = line.ID,
                                    LastStation = lastStationName,
                                    StatrtTime = lineTrip.StartTime,
                                    StationCode = stations[i].StationNumber
                                };
                            }
                            underTruck[stations[i].StationNumber].ArrivalTime = calcTime;//update the Arrival time
                            observer(underTruck[stations[i].StationNumber]);//update the observer
                        }
                    }

                    //calculate the sleep time (the time until the next station)
                    double sleepTime = stations[current].CurrentToNext.Time.TotalMilliseconds / clock.Rate;
                    sleepTime *= (new Random()).Next(10, 200) / 100d;//real time is between 10% and 200% of the avrege time(the time that save in the LineStation)
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    //while the bus is between stations update the observer any 1 second about the arrival time
                    while(stopwatch.Elapsed.TotalMilliseconds < sleepTime)
                    {
                        double sleep = Math.Min(sleepTime - stopwatch.ElapsedMilliseconds, 1000);
                        Thread.Sleep((int)sleep);

                        if(clock.Cancel)//this part can take a lot of time so chak any second the cancel state
                        {
                            break;
                        }

                        foreach (var timing in underTruck)//update the observer for all the stations that under truck
                        {
                            timing.Value.ArrivalTime -= new TimeSpan(0, 0, 0, (int)sleep * clock.Rate);//substract the time passed during the sleep from the arival time
                            observer(timing.Value);//update the observer 
                        }
                    }
                    stopwatch.Stop();
                }
            };
            newTravel.RunWorkerAsync();
        }

        /// <summary>
        /// add the stationCode to the list of the stations under truck
        /// </summary>
        public void Add_station_to_truck(int stationCode)
        {
            stationsInTrack.Add(stationCode);
        }

        /// <summary>
        /// removes the stationCode from the list of the station under truck
        /// </summary>
        public void Stop_truck_station(int stationCode)
        {
            stationsInTrack.Remove(stationCode);
        }
    }
}
