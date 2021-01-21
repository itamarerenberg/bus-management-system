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
        private BO.Bus boBus;
        public BO.Bus BObus 
        {
            get => boBus;
            set
            {
                boBus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicensNumber"));//all the properties changed
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicenesDate"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kilometraz"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fule"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stat"));
            }
        }

        public string LicensNumber 
        {
            get => boBus.LicensNumber;
            set
            {
                boBus.LicensNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicensNumber"));
            }
        }

        public DateTime LicenesDate 
        {
            get => boBus.LicenesDate;
            set
            {
                boBus.LicenesDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LicenesDate"));
            }
        }

        public float Kilometraz 
        {
            get => boBus.Kilometraz;
            set
            {
                boBus.Kilometraz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kilometraz"));
            }
        }

        public float Fule 
        {
            get => boBus.Fule;
            set
            {
                boBus.Fule = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fule"));
            }
        }

        public BO.BusStatus Stat 
        {
            get => boBus.Stat;
            set
            {
                boBus.Stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stat"));
            }
        }
    }
}
