using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UnvalidID : Exception
    {
        public UnvalidID(string msg) : base(msg) { }
    }
}
