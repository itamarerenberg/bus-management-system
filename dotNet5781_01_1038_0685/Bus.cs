using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_1038_0685
{
    class Bus
    {
        #region fildes end properteys

        private int licensNum;/*filde*/
        readonly DateTime StartDate;/*filde*/
        public double Fule_in_km { get; private set; }/*property*/
        public double General_km { get; private set; }/*property*/
        public double Km { get; private set; }/*property*/
        public DateTime lastTretDate { get; private set; }/*property*/
        public int LicensNum/*property*/
        { 
            get => this.licensNum;
            set 
            {
                if(this.StartDate >= new DateTime(2018,0,0))//if the StartTime is after or equals 1/1/2018 then confirm that the number length is 8
                {
                    if(licensNum.ToString().Length != 8)
                    {
                        throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                    }
                }

                if (this.StartDate < new DateTime(2018, 0, 0))//if the StartTime is before 1/1/2018 then confirm that the number length is 7
                {
                    if (licensNum.ToString().Length != 7)
                    {
                        throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                    }
                }
                this.licensNum = value;
            }
        }

        #endregion

        #region constractor

        public Bus(int _licensNum, DateTime _startDate, int _km = 0, int _general_km = 0, DateTime _lastTretDate = new DateTime())
        {
            this.licensNum = _licensNum;
            this.StartDate = _startDate;
            this.Km = _km;
            this.General_km = _general_km;
            this.lastTretDate = _lastTretDate;
        }

        #endregion

        #region methods

        public bool ride(int km)
        {
            //check if the last treatment was less then one year
            if(DateTime.Now - lastTretDate > new TimeSpan(365,0,0,0))
            {
                return false;
            }

            //check if this ride will cose to pass the 20,000km from last treatment
            if(Km + km > 20000)
            {
                return false;
            }

            //check if ther is enough fule for the ride
            if(Fule_in_km < km)
            {
                return false;
            }

            //update the km end the fule
            Fule_in_km -= km;
            Km += km;
            General_km += km;

            return true;
        }

        public void refule(int fule_in_km = 1200)
        {
            Fule_in_km += fule_in_km;
        }

        public void treatment()
        {
            lastTretDate = DateTime.Now;
            Km = 0;
        }

        public override string ToString()
        {
            return $"license's num: {LicensNum},    km: {Km}";
        }

        #endregion

    }
}
