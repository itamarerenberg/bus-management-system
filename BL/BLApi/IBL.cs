using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using BO;

namespace BLApi
{
    public interface IBL
    {
        #region Bus
        //void AddBus(Bus bus);
        //Bus GetBus(string licensNum);
        //void UpdateBus(Bus bus);
        //void RemoveBus(Bus licensNum);
        //List<Bus> GetAllBuses();
        //void GetAllBusesBy(Predicate<Bus> pred);
        #endregion

        #region Manager
        void AddManagar(string name, string password);
        Manager GetManagar(string name, string password);
        void UpdateManagar(string name, string password);
        void DeleteManagar(string name, string password);

        #endregion

        #region Passenger
        void AddPassenger(string name, string password);
        Passenger GetPassenger(string name, string password);
        void UpdatePassenger(string name, string password);
        void DeletePassenger(string name, string password);

        #endregion

        #region Managment
        Managment GetManagment(string Name, string password);
        #endregion
    }
}
