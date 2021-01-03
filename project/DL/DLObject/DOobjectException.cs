using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class DuplicateExeption:Exception
    {
        public DuplicateExeption(string msg) : base(msg) { }
    }
    public class NotExistExeption : Exception
    {
        public NotExistExeption(string msg) : base(msg) { }
    }
    public class InvalidObjectExeption : Exception
    {
        public InvalidObjectExeption(string msg) : base(msg) { }
    }

}
