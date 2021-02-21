using BL.BLApi;
using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.simulator
{
    public class Garage
    {
        #region singelton

        Garage() { }
        static Garage() { }
        static Garage instance;
        static public Garage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Garage();
                }
                return instance;
            }
        }

        #endregion

        IBL source;

        /// <summary>
        /// the time it's take to treat a bus
        /// </summary>
        private readonly TimeSpan Treatment_time = new TimeSpan(0, 30, 0);//30 minuts

        /// <summary>
        /// the time it's take to refule a bus
        /// </summary>
        private readonly TimeSpan Refule_time = new TimeSpan(0, 5, 0);//5 minuts

        /// <summary>
        /// the amount of fule that the bus will have after refule
        /// </summary>
        private readonly int Max_fule_in_bus = 1200;

        public Action<BusProgress> Observer { get; set; } = (p) => { };

        /// <summary>
        /// lunch a thread that simulate a refuling prosess
        /// </summary>
        /// <param name="bus">the bus to refule</param>
        public void Refule(Bus bus)
        {
            if (bus.Stat != BusStatus.Ready ||bus.Stat != BusStatus.Need_refueling)
            {
                if (bus.Stat == BusStatus.In_refueling)
                {
                    throw new Exception
                }
            }
            Thread refuler = new Thread(() =>
            {
                SimulationClock clock = SimulationClock.Instance;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                source = BLFactory.GetBL("admin");

                //save the initial state of the bus so if the simulator will stop while refuling return the bus to a state as if the ride was never take place
                Bus backUp = new Bus()
                {
                    LicenseNumber = bus.LicenseNumber,
                    LicenesDate = bus.LicenesDate,
                    Kilometraz = bus.Kilometraz,
                    Fuel = bus.Fuel,
                    Stat = bus.Stat,
                    KmAfterTreat = bus.KmAfterTreat,
                    LastTreatDate = bus.LastTreatDate,
                    TimeUntilReady = bus.TimeUntilReady,
                    BusTrips = bus.BusTrips,
                };

                bus.Stat = BusStatus.In_refueling;
                source.UpdateBus(bus);//update the data source that the bus is in refuling now
                while (!clock.Cancel && stopwatch.Elapsed < clock.Rtime_to_Stime(Refule_time))
                {
                    try
                    {
                        //update observer
                        BusProgress progress = new BusProgress()
                        {
                            BusLicensNum = bus.LicenseNumber,
                            Activity = Activities.Refuling,
                            Progress = (float)(100 * stopwatch.Elapsed.TotalMilliseconds / clock.Rtime_to_Stime(Refule_time).TotalMilliseconds)//the presentege of the refule that pass allready
                        };
                        if(!clock.Cancel)
                            Observer(progress);

                        int sleep = (int)Math.Min((int)clock.Rtime_to_Stime(1000), (Refule_time - stopwatch.Elapsed).TotalMilliseconds);//the minimum between 1 second and the time that rimeins to the refuling prosess
                        sleep = Math.Max(sleep, 0);//if sleep turn out to be less then zero so set sleep to 0
                        Thread.Sleep(sleep);
                    }
                    catch (ThreadInterruptedException)
                    {
                        return;
                    }
                }
                //update the bus Fule state
                if (!clock.Cancel)
                {
                    bus.Fuel = Max_fule_in_bus;
                    //set the bus's new status
                    bus.Stat = bus.KmAfterTreat >= Bus.max_km_without_tratment
                    || bus.LastTreatDate < DateTime.Now - Bus.max_time_without_tratment
                    ? BusStatus.Need_treatment//if the bus need treatment
                    : BusStatus.Ready;//else

                    source.UpdateBus(bus);
                    //update observer
                    BusProgress progress = new BusProgress()
                    {
                        BusLicensNum = bus.LicenseNumber,
                        Activity = Activities.Refuling,
                        Progress = 100,//update the observer that the refuling proses has been finished
                        FinishedFlag = true
                    };
                    if (!clock.Cancel)
                        Observer(progress);
                }
                else
                {
                    source.UpdateBus(backUp);
                }
            });
            refuler.Name = "refuler " + bus.LicenseNumber;
            refuler.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        public void Treatment(Bus bus)
        {
            Thread treatment = new Thread(() => 
            {
                SimulationClock clock = SimulationClock.Instance;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                source = BLFactory.GetBL("admin");

                //save the initial state of the bus so if the simulator will stop while treatmenting return the bus to a state as if the ride was never take place
                Bus backUp = new Bus()
                {
                    LicenseNumber = bus.LicenseNumber,
                    LicenesDate = bus.LicenesDate,
                    Kilometraz = bus.Kilometraz,
                    Fuel = bus.Fuel,
                    Stat = bus.Stat,
                    KmAfterTreat = bus.KmAfterTreat,
                    LastTreatDate = bus.LastTreatDate,
                    TimeUntilReady = bus.TimeUntilReady,
                    BusTrips = bus.BusTrips,
                };

                bus.Stat = BusStatus.In_treatment;
                source.UpdateBus(bus);//update the data source that the bus is in treatment now
                while (!clock.Cancel && stopwatch.Elapsed < clock.Rtime_to_Stime(Treatment_time))
                {
                    try
                    {
                        //update observer
                        BusProgress progress = new BusProgress()
                        {
                            BusLicensNum = bus.LicenseNumber,
                            Activity = Activities.InTrartment,
                            Progress = (float)(100 * stopwatch.Elapsed.TotalMilliseconds / Treatment_time.TotalMilliseconds)//the presentege of the treatment that pass allready
                        };
                        if (!clock.Cancel)
                            Observer(progress);

                        int sleep = (int)Math.Min((int)clock.Rtime_to_Stime(1000), (Refule_time - stopwatch.Elapsed).TotalMilliseconds);//the minimum between 1 second and the time that rimeins to the refuling prosess
                        sleep = Math.Max(sleep, 0);//if sleep turn out to be less then zero so set sleep to 0
                        Thread.Sleep(sleep);
                    }
                    catch (ThreadInterruptedException)
                    {
                        return;
                    }
                }
                //update the bus Fule state
                if (!clock.Cancel)
                {
                    bus.KmAfterTreat = 0;
                    bus.LastTreatDate = DateTime.Now - DateTime.Now.TimeOfDay + clock.Time;//the current real world date with the time in the dey = clock.Time
                    bus.Stat = bus.Fuel >= Bus.min_fule_befor_warning ? BusStatus.Ready : BusStatus.Need_refueling;
                    source.UpdateBus(bus);
                    //update observer
                    BusProgress progress = new BusProgress()
                    {
                        BusLicensNum = bus.LicenseNumber,
                        Activity = Activities.Refuling,
                        Progress = 100,//update the observer that the refuling proses has been finished
                        FinishedFlag = true
                    };
                    if (!clock.Cancel)
                        Observer(progress);
                }
                else//if the simulator stoped will treatmenting
                {
                    source.UpdateBus(backUp);
                }
            });
            treatment.Name = "treatment " + bus.LicenseNumber;
            treatment.Start();
        }
        /// <summary>
        /// reset the buses status(stop from traveling) while closing the propgram
        /// </summary>
        public void ResetBuses()
        {
            source = BLFactory.GetBL("admin");
            foreach (Bus bus in source.GetAllBuses())
            {
                if (bus.Stat == BusStatus.Traveling)
                {
                    bus.Stat = BusStatus.Ready;
                }
                else if (bus.Stat == BusStatus.In_treatment)
                {
                    if (bus.KmAfterTreat >= Bus.max_KmAfterTreat_befor_warning || bus.LastTreatDate <= DateTime.Now - Bus.max_time_without_tratment)//if the bus need treatment
                    {
                        bus.Stat = BusStatus.Need_treatment;
                    }
                    else if (bus.Fuel < Bus.min_fule_befor_warning)//if the bus need refueling
                    {
                        bus.Stat = BusStatus.Need_refueling;
                    }
                    else
                    {
                        bus.Stat = BusStatus.Ready;
                    }
                }
                else if (bus.Stat == BusStatus.In_refueling)
                {
                    if (bus.KmAfterTreat >= Bus.max_KmAfterTreat_befor_warning || bus.LastTreatDate <= DateTime.Now - Bus.max_time_without_tratment)//if the bus need treatment
                    {
                        bus.Stat = BusStatus.Need_treatment;
                    }
                    else if (bus.Fuel < Bus.min_fule_befor_warning)//if the bus need refueling
                    {
                        bus.Stat = BusStatus.Need_refueling;
                    }
                    else
                    {
                        bus.Stat = BusStatus.Ready;
                    }
                }
                source.UpdateBus(bus);
            }
        }
    }
}
