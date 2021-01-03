using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.Models.PO
{
    public class Bus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string licensNumber;
        public string LicensNumber 
        {
            get => licensNumber;
            set
            {
                licensNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicensNumber"));
            }
        }

        private DateTime licenesDate;
        public DateTime LicenesDate 
        {
            get => licenesDate;
            set
            {
                licenesDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicenesDate"));
            }
        }

        private float kilometraz;
        public float Kilometraz 
        {
            get => kilometraz;
            set
            {
                kilometraz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kilometraz"));
            }
        }

        private float fule;
        public float Fule 
        {
            get => fule;
            set
            {
                fule = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fule"));
            }
        }

        private BO.BusStatus stat;
        public BO.BusStatus Stat 
        {
            get => stat;
            set
            {
                stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stat"));
            }
        }
    }
}
