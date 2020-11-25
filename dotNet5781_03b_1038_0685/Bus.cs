using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03b_1038_0685
{
    public enum StatEnum {READY,IS_TRAVELING,IN_FULLING,IN_TREATMENT }
    public class Bus
    {
        #region fildes end properteys

        private string licensNum="";/*filde*/
        private string licensNumForm="";/*filde*/
        readonly DateTime StartDate;/*filde*/
        public StatEnum Status {get;set;}
        public double Fule_in_km { get; private set; }/*property*/
        public double SumKm { get; private set; }/*property*/
        public double KmAfterTreat { get; private set; }/*property*/
        public DateTime LastTreatDate { get; private set; }/*property*/
        public string LicensNum/*property*/
        {
            get => this.licensNum;
            set
            {
                if(!value.All(char.IsDigit))//confirm that all carecters in value is digites
                {
                    throw new ArgumentException("licensNum must contain only digites");
                }

                if (this.StartDate >= new DateTime(2018, 1, 1) && value.Length != 8)//if the StartTime is after or equals 1/1/2018 and the number length is not 8
                {
                    throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                }

                if (this.StartDate < new DateTime(2018, 1, 1) && value.Length != 7)//if the StartTime is before 1/1/2018 and the number length is not 7
                {
                    throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                }
                this.licensNum = value;
                LicensNumForm = value;
            }
        }

        public string LicensNumForm
        {
            get => this.licensNumForm;
            set
            {
                //generate the licens number in format xx-xxx-xx or xxx-xx-xxx
                string id = value;
                if (id.Length == 8)
                {
                    id = id.Insert(5, "-");
                    id = id.Insert(3, "-");
                }
                else
                {
                    id = id.Insert(5, "-");
                    id = id.Insert(2, "-");
                }
                this.licensNumForm = id;
            }
        }

        #endregion

        #region constractor

        public Bus(string _licensNum, DateTime _startDate, double kmAfterTreat = 0, double sumKm = 0, double _fule_in_km = 1200, DateTime _lastTretDate = new DateTime(), StatEnum status = 0)
        {
            this.StartDate = _startDate;
            this.LicensNum = _licensNum;
            this.KmAfterTreat = kmAfterTreat;
            this.SumKm = sumKm;
            this.Fule_in_km = _fule_in_km;
            this.LastTreatDate = _lastTretDate;//??????????????
            Status = status;
        }

        #endregion

        #region methods

        public bool Ride(double km)
        {
            //check if the last treatment was less then one year
            if (DateTime.Now - LastTreatDate > new TimeSpan(365, 0, 0, 0))
            {
                return false;
            }

            //check if this ride will cose to pass the 20,000km from last treatment
            if (KmAfterTreat + km > 20000)
            {
                return false;
            }

            //check if there is enough fule for the ride
            if (Fule_in_km < km)
            {
                return false;
            }

            //update the km end the fule
            Fule_in_km -= km;
            KmAfterTreat += km;
            SumKm += km;

            return true;
        }

        public void Refule(double fule_in_km = 1200)
        {
            Fule_in_km += fule_in_km;
            Status = StatEnum.READY;
        }

        public void Treatment()
        {
            LastTreatDate = DateTime.Now;
            KmAfterTreat = 0;
            Status = StatEnum.READY;
        }

        public override string ToString()
        {
            return $"license's num: {LicensNum},    km: {KmAfterTreat}";
        }

        #endregion

    }
}
