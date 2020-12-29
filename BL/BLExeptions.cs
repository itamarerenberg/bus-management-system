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

    public class InvalidInput : Exception
    {
        public InvalidInput(string msg) : base(msg) { }
    }

    public class LocationOutOfRange : Exception//implemented for station's unvalid Location
    {
        public LocationOutOfRange(string msg) : base(msg) { } 
    }

    /// <summary>
    /// for atempt to add to a colection an object that allready exist 
    /// </summary>
    public class DuplicateExeption : Exception
    {
        /// <summary>
        /// for atempt to add to a colection an object that allready exist 
        /// </summary>
        public DuplicateExeption(string msg) : base(msg) { }
    }
}
