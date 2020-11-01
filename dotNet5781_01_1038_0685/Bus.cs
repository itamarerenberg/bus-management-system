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
        private string p_id;
        private DateTime date;
        private DateTime last_treat_date;
        private int km;
        private int fuel;
        private bool danger;

        public Bus(string id, DateTime date, int km = 0, int fuel = 0)
        {
            p_id = id;
            this.date = date;
            this.km = km;
            this.fuel = fuel;
            danger = false;
        }
        public string Id
        {
            get { return p_id; }
            set { p_id = value; }
        }
        public bool Ride(int sum_km)
        {
            if (!danger && km >= sum_km)
            {

            }
            return false;
        }
        public void Refulling()
        {
            fuel = 1200;
        }
        public void Treatment()
        {
            last_treat_date = DateTime.Now;
            danger = false;
        }
         public bool Equals(Bus b)
        {
            return Id == b.Id;
        }
    }
}
