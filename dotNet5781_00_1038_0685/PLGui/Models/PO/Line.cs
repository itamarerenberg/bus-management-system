using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.Models.PO
{
    public class Line : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int ID 
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ID"));
            }
        }

        private int lineNumber;
        public int LineNumber 
        {
            get => lineNumber;
            set
            {
                lineNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LineNumber"));
            }
        }

        private BO.AreasEnum area;
        public BO.AreasEnum Area 
        {
            get => area;
            set
            {
                area = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Area"));
            }
        }

        public BO.LineStation FirstStation { get => Stations.First(); }
        public BO.LineStation LastStation { get => Stations.Last(); }

        private ObservableCollection<BO.LineStation> stations;
        public ObservableCollection<BO.LineStation> Stations 
        {
            get => stations;
            set
            {
                stations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stations"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastStation"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstStation"));
            }
        }
    }
}
