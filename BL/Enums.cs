using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum BusStatus
    {
        Traveling, 
        Ready,
        In_treatment,
        In_refueling
    }

    public enum AreasEnum
    {
        Jerusalem,
        TelAviv,
        Hifa,
        Rehovot,
        General
    }

    public enum PassengerStat
    {
        Regular,
        Student,
        Old,
        Kid,
        Soldier,
    }
}
