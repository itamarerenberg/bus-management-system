using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_1038_0685
{
    class Bus_company
    {
        List<Bus> busList;
        Bus_company(Bus[] b_arr)
        {
            busList = new List<Bus>(b_arr);
        }
        Bus_company()
        {
            busList = new List<Bus>(0);
        }

        public bool NewBus(string id, int km, DateTime start = new DateTime(), DateTime lastTreat = new DateTime(), int fuel = 1200)
        {
            Bus temp_bus;
            for (int i = 0; i < busList.Count; i++)
            {
                temp_bus = busList[i];
                if (temp_bus.Id == id)
                {
                    return false;
                }
            }
            temp_bus = new Bus(id, start, km, fuel);
            busList.Add(temp_bus);
            return true;
        }

        public bool Ride(string id, int km)
        {
            for(int i = 0; i < busList.Count; i++)
            {
                Bus temp_bus = busList[i];
                if(temp_bus.Id == id)
                {
                    return temp_bus.Ride(km);
                }
            }
            return false;
        }

        public bool maintenance(string id, bool maintenance_kind)//maintenance_kind: true - refeul, false - treat
        {
            Bus temp_bus;
            for (int i = 0; i < busList.Count; i++)
            {
                temp_bus = busList[i];
                if (temp_bus.Id == id)
                {
                    if(maintenance_kind)
                    {
                        temp_bus.Refuel();
                    }
                    else
                    {
                        temp_bus.Treatment();
                    }
                    return true;
                }
            }
            return false;
        }

        private void help_func(Bus bus)
        {
            Console.WriteLine("bus: {0} km: {1}", bus.Id, bus.Sum_km - bus.Last_treat_km);
        }

        public void print_stat()
        {
                busList.ForEach(help_func);
        }

    }
}
