using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLApi
{
    public interface IBL
    {
        void AddBus(Bus bus);
        Bus GetBus(string licensNum);
        void UpdateBus(Bus bus);
        void RemoveBus(Bus licensNum);
        List<Bus> GetAllBuses();
        void GetAllBusesBy(Predicate<Bus> pred);
    }
}
