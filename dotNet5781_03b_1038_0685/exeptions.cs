using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03b_1038_0685
{
    class Busy : Exception
    {
        public Busy(string massege) : base(massege) { }
    }

    class NeedTreatment : Exception
    {
        public NeedTreatment(string massege) : base(massege) { }
    }

    class Danger : Exception
    {
        public Danger(string massege) : base(massege) { }
    }

    class NotEnoughFule : Exception
    {
        public NotEnoughFule(string massege) : base(massege) { }
    }
}
