using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace dotNet5781_03b_1038_0685
{
    public enum StatEnum {READY, IS_TRAVELING, IN_REFUELING, IN_TREATMENT, NEED_TREATMENT }
    public class Bus : INotifyPropertyChanged
    {
        static readonly TimeSpan time_refuling = TimeSpan.FromSeconds(12);
        static readonly TimeSpan time_treatment = TimeSpan.FromSeconds(144);
        #region privates fildes
        private string licensNum;
        private StatEnum stat;
        private double fule_in_km;
        private double sumKm;
        private double kmAfterTreat;
        private DateTime lastTreatDate;
        readonly DateTime startDate;
        private TimeSpan time_until_ready;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region properties
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicensNum"));
            }
        }
        [DisplayName("Start Date")]
        public DateTime StartDate { get => startDate; }
        [DisplayName("Status")]
        public StatEnum Stat { get => stat; 
            set
            {
                stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stat"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Display_stat"));
            }
        }
        [DisplayName("fuel status(km)")]
        public double Fule_in_km { get => fule_in_km;
            set 
            {
                fule_in_km = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fule_in_km"));
            }
        }
        [DisplayName("total km")]
        public double SumKm { get => sumKm;
            set
            {
                sumKm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SumKm"));
            }
        }
        [DisplayName("km after treatment")]
        public double KmAfterTreat { get => kmAfterTreat;
            set
            {
                kmAfterTreat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KmAfterTreat"));
            }
        }
        [DisplayName("Last treatment Date")]
        public DateTime LastTreatDate { get => lastTreatDate;
            set
            {
                lastTreatDate = value;
                if (DateTime.Now - lastTreatDate > new TimeSpan(365, 0, 0, 0))
                {
                    Stat = StatEnum.NEED_TREATMENT;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastTreatDate"));
            }
        }

        /// <summary>
        /// telling for how long the buss status will nod be "ready"
        /// </summary>
        public TimeSpan Time_until_ready
        {
            get => time_until_ready;
            private set
            {
                if (value == new TimeSpan(0))//if value == 0:0:0
                {
                    time_until_ready = new TimeSpan(0, 0, 0);
                    Stat = StatEnum.READY;
                }
                else
                {
                    time_until_ready = value;
                    new Thread(() =>
                    {
                        while (time_until_ready > new TimeSpan(0,0,0))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time_until_ready"));//announce that Time_until_ready chenged
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Display_stat"));//announce that Display_stat chenged
                            Thread.Sleep(1000);
                            time_until_ready = new TimeSpan(hours:0, minutes:0, seconds: (int)time_until_ready.TotalSeconds - 1);//subtruct 1 from Seconds_until_ready   
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time_until_ready"));//announce that Time_until_ready chenged
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Display_stat"));//announce that Display_stat chenged
                        Thread.Sleep(1000);
                        Stat = (DateTime.Now - lastTreatDate > new TimeSpan(365, 0, 0, 0))? StatEnum.NEED_TREATMENT : StatEnum.READY;//change stat to "READY"
                    }).Start();
                }
            }
        }

        public string Display_stat { 
            get
            {
                if(IsBusy)
                {
                    return stat.ToString().Replace("_", " ").ToLower() + " " + time_until_ready;//if busy: add the time until ready to th status
                }
                else
                {
                    return stat.ToString().Replace("_"," ").ToLower() + " ";
                }
            } 
        }

        /// <summary>
        /// true: if the bus in refuling or treatment or traveling
        /// false: if the bus ready or need treatment
        /// </summary>
        public bool IsBusy
        {
            get
            {
                if (this.Stat == StatEnum.IN_REFUELING)
                {
                    return true;
                }

                if (this.Stat == StatEnum.IN_TREATMENT)
                {
                    return true;
                }

                if (this.Stat == StatEnum.IS_TRAVELING)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// true: if the bus in a condition that it can refule at the moment
        /// </summary>
        public bool CanFule => (!IsBusy) && (fule_in_km <= 1200);
        #endregion

        #region constractors
        public Bus() { }
        /// <summary>
        /// Stat = status;
        /// this.startDate = _startDate;
        /// this.LicensNum = _licensNum;
        /// this.KmAfterTreat = kmAfterTreat;
        /// this.SumKm = sumKm;
        /// this.Fule_in_km = _fule_in_km;
        /// this.LastTreatDate = _lastTreatDate;
        /// </summary>
        /// <param name="kmAfterTreat">defult value: 0</param>
        /// <param name="sumKm">defult value: 0</param>
        /// <param name="_fule_in_km">defult value: 1200</param>
        /// <param name="_lastTreatDate">defult value: {01/01/0001 00:00:00}</param>
        /// <param name="status">defult value: StatEnum.READY</param>
        public Bus(string _licensNum, DateTime _startDate, double kmAfterTreat = 0, double sumKm = 0, double _fule_in_km = 1200, DateTime _lastTreatDate = new DateTime(), StatEnum status = StatEnum.READY)
        {
            Stat = status;
            this.startDate = _startDate;
            this.LicensNum = _licensNum;
            this.KmAfterTreat = kmAfterTreat;
            this.SumKm = sumKm;
            this.Fule_in_km = _fule_in_km;
            this.LastTreatDate = _lastTreatDate;
        }

        #endregion

        #region methods

        public void Ride(double km)
        {
            //check if the input is valid
            if (km > 1200)
            {
                throw new ArgumentException("can not preform a ride over then 1200 km");
            }

            //check if busy
            if(IsBusy)
            {
                throw new Busy("the bus is busy");
            }

            if(Stat == StatEnum.NEED_TREATMENT)
            {
                throw new NeedTreatment("need tratment");
            }

            //check if the last treatment was less then one year
            if (DateTime.Now - LastTreatDate > new TimeSpan(365, 0, 0, 0))
            {
                Stat = StatEnum.NEED_TREATMENT;
                throw new NeedTreatment("need treatment");
            }

            //crheck if this ride will cose to pass the 20,000km from last treatment
            if (KmAfterTreat + km > 20000)
            {
                throw new Danger("this ride will over the 20,000 km from the last treatment");
            }

            //check if there is enough fule for the ride
            if (Fule_in_km < km)
            {
                throw new NotEnoughFule("there is not enough fuel for this ride");
            }

            //update the km end the fule
            Fule_in_km -= km;
            KmAfterTreat += km;
            SumKm += km;
            Stat = StatEnum.IS_TRAVELING;
            int time = (int)((km / new Random().Next(20, 50)) * 6);//in seconds
            Time_until_ready = new TimeSpan(0,0,time);
        }

        public void Refule()
        {
            //check if busy
            if (IsBusy)
            {
                throw new ArgumentException("you cannot refuel the bus while driving or treatmenting");
            }

            Stat = StatEnum.IN_REFUELING;//set the stat to refueling
            Time_until_ready = time_refuling;//set Time_until_ready to 'time_refuling' secondes
            Fule_in_km = 1200;//set the Fule_in_km to 1200
        }

        public void Treatment()
        {
            //check if busy
            if (IsBusy)
            {
                throw new ArgumentException("you cannot refuel the bus while driving or treatmenting");
            }

            LastTreatDate = DateTime.Now;//set LastTreatDate to now
            KmAfterTreat = 0;//set KmAfterTreat to 0
            Stat = StatEnum.IN_TREATMENT;//set stat to be IN_TREATMENT
            Time_until_ready = time_treatment;//set Time_until_ready to 'time_treatment' seconds
        }

        public override string ToString()
        {
            return $"license's num: {LicensNum},    km: {KmAfterTreat}";
        }
        #endregion
    }
}
