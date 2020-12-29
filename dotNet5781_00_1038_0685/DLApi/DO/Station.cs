using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = Code
    /// </summary>
    public class Station
    {
        public int Code { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
