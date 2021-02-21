using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace PLGui.Models.PO
{
    public class Bus : ObservableValidator
    {
        private BO.Bus boBus;
        public BO.Bus BObus 
        {
            get => boBus;
            set
            {
                SetProperty(ref boBus, value);
                OnPropertyChanged(nameof(LicenseNumber));
                OnPropertyChanged(nameof(LicenesDate));
                OnPropertyChanged(nameof(Kilometraz));
                OnPropertyChanged(nameof(Fuel));
                OnPropertyChanged(nameof(Stat));
                OnPropertyChanged(nameof(TimeUntilReady));
                OnPropertyChanged(nameof(BusTrips));
                OnPropertyChanged(nameof(LineNumber));
                OnPropertyChanged(nameof(Progress));

            }
        }

        private string licenseNumber;
        public string LicenseNumber 
        {
            get 
            {
                if (boBus.LicenseNumber.Count() == 8)
                {
                    return int.Parse(boBus.LicenseNumber).ToString("000-00-000");
                }
                return int.Parse(boBus.LicenseNumber).ToString("00-000-00");
            }
            set
            {
                SetProperty(ref licenseNumber, value);
                boBus.LicenseNumber = licenseNumber;
            }
        }

        private DateTime licenesDate;
        public DateTime LicenesDate 
        {
            get => boBus.LicenesDate;
            set
            {
                SetProperty(ref licenesDate, value);
                boBus.LicenesDate = licenesDate;
            }
        }
        private double kilometraz;
        public double Kilometraz 
        {
            get => boBus.Kilometraz;
            set
            {
                SetProperty(ref kilometraz, value);
                boBus.Kilometraz = kilometraz;
            }
        }
        private float fuel;
        public float Fuel 
        {
            get => boBus.Fuel;
            set
            {
                SetProperty(ref fuel, value);
                boBus.Fuel = fuel;
            }
        }
        private BO.BusStatus stat;
        public BO.BusStatus Stat 
        {
            get => boBus.Stat;
            set
            {
                SetProperty(ref stat, value);
                boBus.Stat = stat;
                OnPropertyChanged("IsBusy");
            }
        }
        private TimeSpan timeUntilReady;

        public TimeSpan TimeUntilReady
        {
            get => boBus.TimeUntilReady;
            set
            {
                SetProperty(ref timeUntilReady, value);
                boBus.TimeUntilReady = timeUntilReady;
            }
        }
        public List<BO.BusTrip> BusTrips{ get => boBus.BusTrips; }

        private float progress;

        public float Progress
        {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }

        private int lineNumber;

        /// <summary>
        /// this fild have mining only when Stat = "Traveling"
        /// </summary>
        public int LineNumber
        {
            get { return lineNumber; }
            set { SetProperty(ref lineNumber, value); }
        }

        public bool IsBusy 
        { 
            get
            {
                return stat == BO.BusStatus.In_refueling
                    || stat == BO.BusStatus.In_treatment
                    || stat == BO.BusStatus.Traveling;
            }
        }
    }
}
