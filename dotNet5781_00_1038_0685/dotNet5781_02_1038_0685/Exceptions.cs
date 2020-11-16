using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    class LineAlreadyExist : Exception
    {
        public LineAlreadyExist(string massege) : base(massege){}
    }
    class NotExist : Exception
    {
        public NotExist(string massege) : base(massege){}
    }
    class TooSmall : Exception
    {
        public TooSmall(string massege) : base(massege) { }
    }
}
