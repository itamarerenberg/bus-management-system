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
    public class SimulationClock
    {
        #region singelton

        static readonly SimulationClock instance = new SimulationClock();
        static SimulationClock() { }
        SimulationClock() { }
        public static SimulationClock Instance { get => instance; }

        #endregion

        private event Action<TimeSpan> observer;
        public Action<TimeSpan> Observer {
            get => observer;
            set => observer = value;
        }

        class Clock
        {
            public Clock(TimeSpan _time)
            {
                this.Time = _time;
            }
            public TimeSpan Time { get; }
            public int Seconds { get => Time.Seconds; }
            public int Minutes { get => Time.Minutes; }
            public int Hours { get => Time.Hours; }
        }
        Clock clock;
        public TimeSpan Time 
        {
            get => clock.Time;
            private set => clock = new Clock(value);
        }

        volatile int rate;
        public int Rate 
        { 
            get => rate;
            set => rate = value;
        }

        internal volatile bool Cancel;

        Thread clockThread;
        public void StartClock(TimeSpan startTime, int rate, Action<TimeSpan> _observer)
        {
            Cancel = false;
            Observer = _observer;

            if (rate < 1)
            {
                throw new IligalRateExeption("rate can not be less then 1");
            }

            Rate = rate;

            Stopwatch tempStopWach = new Stopwatch();
            tempStopWach.Start();//while waiting to the clockWorker to finish start count the time for for more acurecy
            if (clockThread != null)
            {
                clockThread.Interrupt();
                clockThread.Join();//wait for clockThread to finish is current task
            }
            clockThread = new Thread((initialTimeObject) => 
            {
                TimeSpan initialTime = (TimeSpan)initialTimeObject;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (!Cancel)
                {
                    try
                    {
                        clock = new Clock(initialTime + new TimeSpan(Stime_to_Rtime(stopwatch.ElapsedTicks)));
                        observer(new TimeSpan(clock.Hours, clock.Minutes, clock.Seconds));
                        Thread.Sleep((int)Rtime_to_Stime(1000));
                    }
                    catch (ThreadInterruptedException)//if the clock as been interrupted whill sleeping
                    {
                        break;
                    }
                }
            });
            clockThread.Start(tempStopWach.Elapsed + startTime);//start the Clock with initialTime = startTime plus the time it took to start the clock since the StartClock() function was called
        }

        public void StopClock()
        {
            Cancel = true;
        }

        /// <summary>
        /// <br>changes the rate of the simulation</br>
        /// <br>for speed up insert positive number for 'change'</br>
        /// <br>for slow down insert negative number for 'change'</br>
        /// </summary>
        /// <param name="change">adds to current Rate</param>
        public void Change_Rate(int change)
        {
            if(Rate + change < 1)
            {
                throw new IligalRateExeption("rate cant be less then 1");
            }
            Rate += change;
        }

        /// <summary>
        /// convert from real time to simulator time
        /// </summary>
        public long Rtime_to_Stime(long time)
        {
            return time / Rate;
        }

        public TimeSpan Rtime_to_Stime(TimeSpan time)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds: (int)(time.TotalMilliseconds / Rate));
        }

        /// <summary>
        /// convert from simulator time to real time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long Stime_to_Rtime(long time)
        {
            return time * Rate;
        }

        public TimeSpan Stime_to_Rtime(TimeSpan time)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds: (int)(time.TotalMilliseconds * Rate));
        }
    }
}
