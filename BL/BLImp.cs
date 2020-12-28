using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using BLApi;
using BO;
using DLApi;

namespace BL
{
    public class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();
        //public void AddBus(Bus bus)
        //{
        //    //check if the langth of the LicenseNum fit to the LicensDate
        //    if(bus.LicenesDate.Year > 2018)
        //    {
        //        if(bus.LicensNumber.Length != 8)
        //        {
        //            throw new UnvalidID("Licens number is not fit to the licens date");
        //        }
        //    }
        //    else
        //    {
        //        if (bus.LicensNumber.Length != 7)
        //        {
        //            throw new UnvalidID("Licens number is not fit to the licens date");
        //        }
        //    }

        //}
        #region Manager
        public Manager GetManagar(string name, string password)
        {
            try
            {
                //validation
                DO.User user = dl.GetUser(name, password);
                if (user.Password != password)
                {
                    throw new InvalidPassword("invalid password");
                }

            }
            catch (DO.InvalidObjectExeption msg)
            {
                throw new InvalidID("invalid ID");
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        public void AddManagar(string name, string password)
        {
            try
            {
                DO.User tempUser = new DO.User()
                {
                    Name = name,
                    Password = password,
                    Admin = true
                };
                dl.AddUser(tempUser);
            }
            catch (DO.DuplicateExeption msg)
            {
                throw msg.InnerException;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void UpdateManagar(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void DeleteManagar(string name, string password)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Passenger
        public Passenger GetPassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void AddPassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void UpdatePassenger(string name, string password)
        {
            throw new NotImplementedException();
        }
        public void DeletePassenger(string name, string password)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
