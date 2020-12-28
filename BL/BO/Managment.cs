using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Managment
    {
        #region singelton
        static readonly Managment instance = new Managment();
        static Managment() { }
        Managment() { }
        public static Managment Instance { get => instance; }
        #endregion

        public List<Line> Lines;
        public List<Bus> Buses;
        public List<Station> Stations;
    }
}
