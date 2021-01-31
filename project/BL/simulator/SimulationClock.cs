﻿using System;
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

        SimulationClock() { }
        SimulationClock instance;
        public SimulationClock Instance 
        {
            get
            {
                if(instance == null)
                {
                    instance = new SimulationClock();
                }
                return instance;
            }
        }

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
            public TimeSpan Time { get; set; }
            public int Seconds { get => Time.Seconds; }
            public int Minutes { get => Time.Minutes; }
            public int Hours { get => Time.Hours; }
        }
        Clock clock;
        public TimeSpan Time 
        {
            get => clock.Time;
            set => clock.Time = value;
        }


        internal volatile bool Cancel;

        BackgroundWorker clockWorker;
        void StartClock(TimeSpan startTime, int rate, Action<TimeSpan> _observer)
        {
            if(clockWorker == null)
            {
                clockWorker = new BackgroundWorker();
            }

            Cancel = false;
            Observer = _observer;

            Stopwatch tempStopWach = new Stopwatch();
            tempStopWach.Start();//while waiting to the clockWorker to finish start count the time for for more acurecy
            while (clockWorker.IsBusy) ;//waite for the clock forker to finish 

            clockWorker.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = (BackgroundWorker)sender;
                TimeSpan start = startTime;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while(!Cancel)
                {
                    clock = new Clock(startTime + new TimeSpan(stopwatch.ElapsedTicks * rate));
                    observer(new TimeSpan(clock.Hours, clock.Minutes, clock.Seconds));
                    Thread.Sleep(1000);
                }
                stopwatch.Stop();
            };//this function will execute in the BackgroundWorker thread

            startTime = startTime + new TimeSpan(tempStopWach.ElapsedTicks);//add to the start time the time pased until the clockWorker fnish is last task
            tempStopWach.Stop();

            clockWorker.RunWorkerAsync();
        }

        public void StopClock()
        {
            Cancel = true;
        }
    }
}
