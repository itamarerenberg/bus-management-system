using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03b_1038_0685
{
    public static class RandBus
    {
        private static readonly Random r = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// generating a bus by random values (some bus properties can be specified)
        /// </summary>
        /// <param name="fuel">fuel must have below or equal to 1200</param>
        /// <param name="km">the amount of km after the last treatment</param>
        /// <param name="time">the time from the last treatment</param>
        /// <returns>object of type Bus </returns>
        public static Bus RB(double fuel = -1, double km = -1, TimeSpan time = new TimeSpan())
        {
            DateTime SDate = new DateTime(2018 + r.Next(2), r.Next(1, 12), r.Next(1, 28));//random start date
            double Fuel = (fuel == -1 || fuel > 1200) ? r.Next(1200) : fuel;
            double Km = km == -1 ? r.Next(20000) : km;
            DateTime Time = time == new TimeSpan() ? SDate.AddDays(r.Next(300)): DateTime.Today - time;

            while (SDate > Time)//make sure the last treatment date is after the start date
            {
                SDate = new DateTime(2018 + r.Next(2), r.Next(1, 12), r.Next(1, 28));
            }

            return new Bus(r.Next(10000000, 99999999).ToString(), SDate ,Km ,r.Next(40000, 100000),Fuel, Time);
        }
        public static List<Bus> ListRB(int num, double fuel = -1, double km = -1, TimeSpan time = new TimeSpan())
        {
            List<Bus> list = new List<Bus> { };
            for (int i = 0; i < num; i++)
            {
                list.Add(RB(fuel, km, time));
            }
            return list;
        }

    }
}
