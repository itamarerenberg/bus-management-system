using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BL.simulator
{
    class IligalRateExeption : Exception
    {
        public IligalRateExeption(string msg) : base(msg){}
    }
}
