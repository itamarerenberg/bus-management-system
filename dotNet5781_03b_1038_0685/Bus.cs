using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        #region fildes end properties
        private string licensNum;/*filde*/
        readonly DateTime startDate;/*filde*/
        [DisplayName("license number")]
        public string LicensNum
        {
            get => this.licensNum;
            set
            {
                if (!value.All(char.IsDigit))//confirm that all characters in value are digites
                {
                    throw new ArgumentException("licensNum must contain only digites");
                }
                if (this.startDate >= new DateTime(2018, 1, 1) && value.Length != 8)//if the StartTime is after or equals 1/1/2018 and the number length is not 8
                {
                    throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                }
                if (this.startDate < new DateTime(2018, 1, 1) && value.Length != 7)//if the StartTime is before 1/1/2018 and the number length is not 7
                {
                    throw new ArgumentException("the length of the licen's num must be suitible to the year of the bus");
                }
                //generates the licens number in format xx-xxx-xx or xxx-xx-xxx
                if (value.Length == 8)
                {
                    value = value.Insert(5, "-");
                    value = value.Insert(3, "-");
                }
                else
                {
                    value = value.Insert(5, "-");
                    value = value.Insert(2, "-");
                }
                this.licensNum = value;
            }
        }
        [DisplayName("Start Date")]
        public DateTime StartDate { get => startDate; }
        [DisplayName("Status")]
        public StatEnum Stat {get;set;}
        [DisplayName("fuel status(km)")]
        public double Fule_in_km { get; set; }
        [DisplayName("total km")]
        public double SumKm { get; set; }
        [DisplayName("km after treatment")]
        public double KmAfterTreat { get; set; }
        [DisplayName("Last treatment Date")]
        public DateTime LastTreatDate { get; set; }

        #endregion

        #region constractors
        public Bus() { }
        public Bus(string _licensNum, DateTime _startDate, double kmAfterTreat = 0, double sumKm = 0, double _fule_in_km = 1200, DateTime _lastTreatDate = new DateTime(), StatEnum status = 0)
        {
            this.startDate = _startDate;
            this.LicensNum = _licensNum;
            this.KmAfterTreat = kmAfterTreat;
            this.SumKm = sumKm;
            this.Fule_in_km = _fule_in_km;
            this.LastTreatDate = _lastTreatDate;
            Stat = status;
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
            Stat = StatEnum.READY;
        }

        public void Treatment()
        {
            LastTreatDate = DateTime.Now;
            KmAfterTreat = 0;
            Stat = StatEnum.READY;
        }

        public override string ToString()
        {
            return $"license's num: {LicensNum},    km: {KmAfterTreat}";
        }

        #endregion

    }
}
