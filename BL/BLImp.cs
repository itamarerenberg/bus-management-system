using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLApi;
using BO;

namespace BL
{
    public class BLImp : IBL
    {
        public void AddBus(Bus bus)
        {
            //check if the langth of the LicenseNum fit to the LicensDate
            if(bus.LicenesDate.Year > 2018)
            {
                if(bus.LicensNumber.Length != 8)
                {
                    throw new UnvalidID("Licens number is not fit to the licens date");
                }
            }
            else
            {
                if (bus.LicensNumber.Length != 7)
                {
                    throw new UnvalidID("Licens number is not fit to the licens date");
                }
            }

        }

        public List<Bus> GetAllBuses()
        {
            throw new NotImplementedException();
        }

        public void GetAllBusesBy(Predicate<Bus> pred)
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(string licensNum)
        {
            throw new NotImplementedException();
        }

        public void RemoveBus(Bus licensNum)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }
    }
}
