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
        public string Id { get { return Id; } private set{ Id = value; }}
        public DateTime Start_date { get { return Start_date; } private set { Start_date = value; } }
        public DateTime Last_treat_date { get { return Last_treat_date; } private set { Last_treat_date = value; } }
        public int Sum_km { get { return Sum_km; } private set { Sum_km = value; } }
        public int Last_treat_km { get { return Last_treat_km; } private set { Last_treat_km = value; } }
        public int Fuel { get { return Fuel; } private set { Fuel = value; } }
        public bool Danger { get { return Danger; } private set { Danger = value; } }

        //construcror
        public Bus(string id, DateTime date, int km = 0, int fuel = 0)
        {
            Id = id;
            Start_date = date;
            Sum_km = km;
            Fuel = fuel;
            Danger = false;
        }

        public bool Ride(int km)
        {
            if (!Danger && Fuel >= km)
            {
                Sum_km += km;
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
            Last_treat_km = Sum_km;
            Danger = false;
        }
         public bool Equals(Bus b)
        {
            return Id == b.Id;
        }
    }
}
