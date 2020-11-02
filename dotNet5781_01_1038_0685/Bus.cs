using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_1038_0685
{
    class Bus
    {
        public string Id { get; private set; }
        public DateTime Start_date { get; private set; }
        public DateTime Last_treat_date { get; private set; }
        public int Km { get; private set; }
        public int Fuel { get; private set; }
        public bool Danger { get; private set; }

        //construcror
        public Bus(string id, DateTime date, int km = 0, int fuel = 0)
        {
            Id = id;
            Start_date = date;
            Km = km;
            Fuel = fuel;
            Danger = false;
        }

        TimeSpan Time_dif(DateTime dt1, DateTime dt2)
        {
            int days = (dt1.Year - dt2.Year) * 365;//years difrence
            days += (dt1.Month - dt2.Month) * 30;//Month dif
            days += (dt1.Day - dt1.Day);
            return new TimeSpan(days, 0, 0, 0);
        }

        public bool Ride(int km)
        {
            
            if (!(Time_dif(Last_treat_date, DateTime.Now) > new TimeSpan(365, 0, 0, 0) || Km + km >= 20000) && Fuel >= km)
            {
                Km += km;
                Fuel -= km;
                return true;
            }
            return false;
        }
        public void Refuel()
        {
            Fuel = 1200;
        }
        public void Treatment()
        {
            Last_treat_date = DateTime.Now;
            Km = 0;
            Danger = false;
        }
    }
}
