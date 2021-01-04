using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.Models.PO
{
    public class Station : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int code { get; set; }
        public int Code 
        {
            get => code;
            set
            {
                code = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
            }
        }

        private string name;
        public string Name
        {
            get => Name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private float longitude;
        public float Longitude 
        {
            get => longitude;
            set
            {
                longitude = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Longitude"));
            }
        }

        private float latitude;
        public float Latitude 
        {
            get => latitude;
            set
            {
                latitude = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Latitude"));
            }
        }

        private string address;
        public string Address 
        {
            get => address;
            set
            {
                address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }

        public GeoCoordinate Location
        { 
            get => new GeoCoordinate(Longitude, Latitude); 
        }

        private ObservableCollection<Line> getLines;
        public ObservableCollection<Line> GetLines 
        {
            get => getLines;
            set
            {
                getLines = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GetLines"));
            }
        }
    }
}
