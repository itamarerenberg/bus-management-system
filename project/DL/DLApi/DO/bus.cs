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
        public string LicenseNumber { get; set; }
        public DateTime LicenesDate { get; set; }
        public double Kilometraz { get; set; }
        public float Fuel { get; set; }
        public BusStatus Stat { get; set; }
        public double KmAfterTreat { get; set; }
        public DateTime LastTreatDate { get; set; }
        public bool IsActive{ get; set; }
    }
}
