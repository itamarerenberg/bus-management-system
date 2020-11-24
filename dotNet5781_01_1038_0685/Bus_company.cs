using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_1038_0685
{
    class Bus_company
    {
        #region fildes end propertys

        private List<Bus> buss;
        public List<Bus> Buss { get => this.buss.FindAll(b => true); private set { this.buss = value; } }

        //indexer//
        /// <returns>bus with the LicensNum == _licensNum</returns>
        /// <exception cref="buss not exist">if ther is no buss with (LicensNum == _licensNum)</exception>
        public Bus this[string _licensNum]
        {
            get
            {
                if (!buss.Exists(b => b.LicensNum == _licensNum))
                {
                    throw new ArgumentException("buss not exist");
                }

                return buss.Find(b => b.LicensNum == _licensNum);
            }
        }

        #endregion

        #region constractors

        /// <summary>
        /// initialize Bus_compeny with _buss as its "Bus"'s colection
        /// </summary>
        /// <param name="_buss">the list of the buss in the new Bus_compeny</param>
        public Bus_company(List<Bus> _buss)
        {
            this.buss = _buss;
        }

        /// <summary>
        /// initialize Bus_compeny with empty colection of "Bus"s
        /// </summary>
        public Bus_company()
        {
            this.buss = new List<Bus>();
        }

        #endregion

        #region methods

        /// <summary>
        /// create a new "Bus" with {LicensNum = _licensNum, StartDate = _startDate, Km = _km, General_km = _general_km, LastTretDate = _lastTretDate}
        /// and add it to the bus's colection
        /// </summary>
        /// <param name="_licensNum"></param>
        /// <param name="_startDate"></param>
        /// <param name="_km">(defult = 0)</param>
        /// <param name="_general_km">(defult = 0)</param>
        /// <param name="_lastTretDate">(defult = new DateTime())</param>
        public void Add_new_bus(string _licensNum, DateTime _startDate, double _km = 0, double _general_km = 0, double _fule_in_km = 1200, DateTime _lastTretDate = new DateTime())
        {
            buss.Add(new Bus(_licensNum, _startDate, _km, _general_km, _fule_in_km, _lastTretDate));
        }

        /// <summary>
        /// add "_new_bus" to the bus's colection
        /// </summary>
        public void Add_new_bus(Bus _new_bus)
        {
            buss.Add(_new_bus);
        }

        public override string ToString()
        {
            string str = "";
            buss.ForEach(b => str += b.ToString() + '\n');
            return str;
        }

        #endregion
    }
}
