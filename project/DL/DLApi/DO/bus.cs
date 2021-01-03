using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// identity property = LicenseNum
    /// </summary>
    public class Bus
    {
        public string LicenseNum { get; set; }
        public DateTime LicenesDate { get; set; }
        public float Kilometraz { get; set; }
        public float Fule { get; set; }
        public BusStatus Stat { get; set; }
        public bool IsActive{ get; set; }
    }
}
