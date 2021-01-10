using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLApi;

namespace BL.BLApi
{
    public static class BLFactory
    {
        public static IBL GetBL(string type)
        {
            switch (type)
            {
                case "passenger":
                    return BLImpPassenger.Instance;
                case "admin":
                    return BLImpAdmin.Instance;
                default:
                    return BLImpPassenger.Instance;
            }
        }
    }
}
