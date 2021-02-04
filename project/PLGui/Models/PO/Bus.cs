using System;
using System.Collections.Generic;
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
                boBus = value;
                OnPropertyChanged(nameof(LicenseNumber));
                OnPropertyChanged(nameof(LicenesDate));
                OnPropertyChanged(nameof(Kilometraz));
                OnPropertyChanged(nameof(Fule));
                OnPropertyChanged(nameof(Stat));
            }
        }

        private string licenseNumber;
        public string LicenseNumber 
        {
            get => boBus.LicenseNumber;
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
        private float fule;
        public float Fule 
        {
            get => boBus.Fule;
            set
            {
                SetProperty(ref fule, value);
                boBus.Fule = fule;
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
            }
        }
        private TimeSpan timeUntilReady;

        public TimeSpan MyProperty
        {
            get => boBus.TimeUntilReady;
            set
            {
                SetProperty(ref timeUntilReady, value);
                boBus.TimeUntilReady = timeUntilReady;
            }
        }

    }
}
