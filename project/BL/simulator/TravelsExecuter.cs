using BL.BLApi;
using BLApi;
using BO;
using DLApi;
using PriorityQueue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
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

        /// <summary>
        /// <br>this fild is save how much time before the start</br>
        /// <br>time of the travel the travel executer will lunch the</br>
        /// <br>backgrownd worker of this travel</br>
        /// <br>(in simulator time)</br>
        /// </summary>
        TimeSpan timeToExcuteTravel = new TimeSpan(0, 20, 0);

        Action<LineTiming> observer;
        List<int> stationsInTrack = new List<int>();//list of all the station that in tracking

        IBL source;

        RidesSchedule schedule = RidesSchedule.Instance;

        SimulationClock clock;

        BackgroundWorker travelsExecuterWorker;
        Thread travelsExecuterThread;
        public void StartExecute(Action<LineTiming> _observer = null)
        {
            if(source == null)
            {
                source = BLFactory.GetBL("admin");
            }

            clock = SimulationClock.Instance;
            observer = _observer != null ? _observer : (LineTiming) => { };//if _observer is null then set observer to be an Action<LineTiming> that do nothing

            //load the lineTrips and lines
            schedule.Set_lineTrips(source.GetAllLineTrips());

            if (travelsExecuterWorker == null)
            {
                travelsExecuterWorker = new BackgroundWorker();
                travelsExecuterWorker.DoWork += (object sender, DoWorkEventArgs args) =>
                {
                    travelsExecuterThread = Thread.CurrentThread;
                    if (travelsExecuterThread.Name == null)
                    {
                        travelsExecuterThread.Name = "travelsExecuter";
                    }
                    while (!clock.Cancel)
                    {
                        try
                        {
                            Thread.Sleep(clock.Rtime_to_Stime(schedule.time_until_next_ride()));
                        }
                        catch (Exception)
                        {
                            return;
                        } 
                        executeTravel(schedule.GetNextRide());//execute the travel
                    }
                };
            }
            if (travelsExecuterWorker.IsBusy)
            {
                travelsExecuterThread.Interrupt();
                Thread.Sleep(50);
            }
            travelsExecuterWorker.RunWorkerAsync();
        }

        private void executeTravel(Ride ride)
        {
            BackgroundWorker newTravel = new BackgroundWorker();
            newTravel.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = "newTravel";
                }

                //get the line of this line trip
                Line line = source.GetLine(ride.LineId);

                string lastStationName = source.GetStation(line.LastStation.StationNumber).Name;//get the name of the last station in this line
                int lineNumber = line.LineNumber;

                LineStation[] stations = line.Stations.ToArray();
                Dictionary<int, LineTiming> underTruck = new Dictionary<int, LineTiming>();//save the code of all the stations in this line that under truck together with the apropiate lineTiming  

                //wate for the start of the travel
                while(ride.StartTime > clock.Time)
                {
                    //sleep the minimum between 1 second to the time until the start time
                    double sleep = Math.Min(clock.Stime_to_Rtime((long)(ride.StartTime - clock.Time).TotalMilliseconds), 1000);
                    Thread.Sleep((int)sleep);

                    if (clock.Cancel)//this part can take a lot of time so chak any second the cancel state
                    {
                        break;
                    }

                    foreach (int station in stationsInTrack)//update the observer for all the stations that under truck
                    {
                        if (!underTruck.ContainsKey(station))
                        {
                            underTruck[station] =//add to the under truck dictionery
                                new LineTiming()
                                {
                                    LineNum = line.LineNumber,
                                    LineId = line.ID,
                                    LastStation = lastStationName,
                                    StartTime = ride.StartTime,
                                    StationCode = station
                                };
                        }
                        LineTiming timing = underTruck[station];
                        timing.ArrivalTime -= new TimeSpan(0, 0, 0, (int)clock.Stime_to_Rtime((long)sleep));//substract the time passed during the sleep from the arival time
                        observer(timing);//update the observer 
                    }
                }

                for (int current = 0; current < stations.Length && !clock.Cancel; current++)
                {
                    //calculate the arival time according to the position of the bus now
                    TimeSpan calcTime = TimeSpan.Zero;//ride.StartTime - clock.Time;//the travel executs 'timeToExcuteTravel' time before the actual start time of the trip
                    for (int i = current; i < stations.Length; i++)
                    {
                        calcTime += stations[i].PrevToCurrent != null? stations[i].PrevToCurrent.Time :TimeSpan.Zero;
                        if (stationsInTrack.Contains(stations[i].StationNumber))//if this station under truck then update the arival time to this station
                        {
                            if (!underTruck.ContainsKey(stations[i].StationNumber))//if this station new in stationsInTrack(added after the trip started)
                            {
                                underTruck[stations[i].StationNumber] =//add to the under truck dictionery
                                new LineTiming()
                                {
                                    LineNum = line.LineNumber,
                                    LineId = line.ID,
                                    LastStation = lastStationName,
                                    StartTime = ride.StartTime,
                                    StationCode = stations[i].StationNumber
                                };
                            }
                            underTruck[stations[i].StationNumber].ArrivalTime = calcTime;//update the Arrival time
                            observer(underTruck[stations[i].StationNumber]);//update the observer
                        }
                    }

                    //calculate the sleep time (the time until the next station)
                    long currentToNext_time = (stations[current].CurrentToNext != null ? (long)stations[current].CurrentToNext.Time.TotalMilliseconds : 0L);//if its the last station in the line then the 'CurrentToNext' fild will be null
                    long sleepTime = clock.Rtime_to_Stime(currentToNext_time);
                    sleepTime *= (new Random()).Next(10, 200) / 100;//real time is between 10% and 200% of the avrege time(the time that save in the LineStation)
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    //while the bus is between stations update the observer any 1 second about the arrival time
                    while(stopwatch.Elapsed.TotalMilliseconds < sleepTime)
                    {
                        double sleep = Math.Min(sleepTime - stopwatch.ElapsedMilliseconds, 1000);
                        Thread.Sleep((int)sleep);

                        if(clock.Cancel)//this part can take a lot of time so check any second the cancel state
                        {
                            break;
                        }

                        foreach (int station in stationsInTrack)//update the observer for all the stations that under truck
                        {
                            if(!underTruck.ContainsKey(station))
                            {
                                underTruck[station] =//add to the under truck dictionery
                                new LineTiming()
                                {
                                    LineNum = line.LineNumber,
                                    LineId = line.ID,
                                    LastStation = lastStationName,
                                    StartTime = ride.StartTime,
                                    StationCode = station
                                };
                            }
                            LineTiming timing = underTruck[station];
                            timing.ArrivalTime -= new TimeSpan(0, 0, 0, (int)clock.Stime_to_Rtime((long)sleep));//substract the time passed during the sleep from the arival time
                            observer(timing);//update the observer 
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

        /// <summary>
        /// return the time in the day that 'duretionBack' before 'pivotTime'
        /// </summary>
        private TimeSpan time_back(TimeSpan pivotTime, TimeSpan duretionBack)
        {
            TimeSpan result = pivotTime - duretionBack;
            while(result < TimeSpan.Zero)
            {
                result += new TimeSpan(1, 0, 0, 0);
            }
            return result;
        }

        /// <summary>
        /// returns the duration from 'pivotTime' to 'dstTime' as times in the day
        /// </summary>
        private TimeSpan timeUntil(TimeSpan pivotTime, TimeSpan dstTime)
        {
            TimeSpan result = dstTime - pivotTime;
            while (result < TimeSpan.Zero)
            {
                result += new TimeSpan(1, 0, 0);
            }
            return result;
        }
    }
}
