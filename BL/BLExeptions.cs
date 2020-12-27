using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InvalidID : Exception
    {
        public InvalidID(string msg) : base(msg) { }
    }
    public class InvalidPassword : Exception
    {
        public InvalidPassword(string msg) : base(msg) { }
    }
}
