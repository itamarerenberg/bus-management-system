using BO;
using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.simulator
{
    /// <summary>
    /// Schedule of Rides in 24 hours cycle
    /// </summary>
    internal class RidesSchedule
    {
        #region singelton

        RidesSchedule(){ }
        static RidesSchedule() { }
        static RidesSchedule instance;
        static public RidesSchedule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RidesSchedule();
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// if the first ride in the queue is more then 'scheduleTimeOut' late from the current Simulator time then reorder the rides
        /// </summary>
        readonly TimeSpan scheduleTimeOut = new TimeSpan(0, 0, 1);
        /// <summary>
        /// how much time before the startTime of the ride to execute the ride
        /// </summary>
        readonly TimeSpan timeToExecute = new TimeSpan(0, 20, 0);

        PriorityQueue<Ride> Rides { get; set; }
        SimulationClock clock = SimulationClock.Instance;

        /// <summary>
        /// gets a colection of line trips and generets rides according to the lineTrips and insert them in the schedule
        /// </summary>
        public void Set_lineTrips(IEnumerable<LineTrip> lineTrips)
        {
            Rides = new PriorityQueue<Ride>((l, r) => { return l.StartTime > r.StartTime; });
            build_schedule(lineTrips);
            reorder_rides();//oreder the rides so the first ride is the colesest ride to the current time
        }

        /// <summary>
        /// calculats all the rides in 24 hourse cycle end insert them in to the Ride's priority_Queue
        /// </summary>
        /// <param name="lineTrips"></param>
        private void build_schedule(IEnumerable<LineTrip> lineTrips)
        {
            //for each lineTrip: calulate all the rides of the line trip in 24 hours cycle and enque the ride to the Rides priority_Queue
            foreach(var lt in lineTrips)
            {
                if(lt.FinishAt <= lt.StartTime)
                {
                    lt.FinishAt += new TimeSpan(days: 1, 0, 0, 0);//if lt.FinishAt < lt.StartTime then apparently the finish time is in the day after the startTime
                }
                for(TimeSpan rideTime = lt.StartTime; rideTime <= lt.FinishAt; rideTime += lt.Frequency)//this is all the rides of this line trip in one day
                {
                    Rides.Enqueue(new Ride()
                    {
                        LineId = lt.LineId,
                        LineTripId = lt.ID,
                        StartTime = rideTime,
                    });
                }
            }
        }

        public Ride GetNextRide()
        {
            //if the next ride's start time was in more then 'scheduleTimeOut' then reorder the rides in the queue to be up to date with the current time
            if (Rides.Top().StartTime < clock.Time - scheduleTimeOut)
            {
                reorder_rides();
            }
            Ride result = Rides.Dequeue();//dequeue the closest ride in the queue
            Ride temp = new Ride() //create new LineTiming to insert back to the queue for the next dey
            {
                LineId = result.LineId,
                StartTime = result.StartTime + new TimeSpan(days: 1, 0, 0, 0)//this ride is for the dey after the dey of 'result' 
            };
            Rides.Enqueue(temp);
            return result;
        }

        private void reorder_rides()
        {
            //the current time (+ 1 minuts to be save)
            TimeSpan currentTime = clock.Time + new TimeSpan(0, 1, 0);
            while(Rides.Top().StartTime < clock.Time)
            {
                Ride temp = Rides.Dequeue();
                temp.StartTime += (new TimeSpan(days: 1, 0, 0, 0));
                Rides.Enqueue(temp);
            }
        }

        public TimeSpan time_until_next_ride()
        {
            if (Rides.Top().StartTime < clock.Time - scheduleTimeOut)
            {
                reorder_rides();
            }
            Ride next_ride = Rides.Top();
            TimeSpan result = next_ride.StartTime - clock.Time - timeToExecute;//the time until the start time of the next ride minus 'timeToExecute'
            return result > TimeSpan.Zero ? result : TimeSpan.Zero;//if the ride is late by a short time then still execut the ride now
        }
    }
}
