using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace PLGui.Models.PO
{
    public class Station : ObservableValidator
    {
        public Station()
        {
            this.ErrorsChanged += Station_ErrorsChanged;
            this.PropertyChanged += Station_PropertyChanged;
        }

        private BO.Station station;
        public BO.Station BOstation {
            get 
            {
                station.LinesNums = linesNums.ToList();
                return station; 
            }
            set 
            {
                station = value;
                OnPropertyChanged("Code");
                OnPropertyChanged("Name");
                OnPropertyChanged("Longitude");
                OnPropertyChanged("Latitude");
                OnPropertyChanged("Address");
                OnPropertyChanged("Location");
                linesNums = new ObservableCollection<int>(station.LinesNums);//bcause that lineNums need to be an ObservableCollection it a spetial case
                OnPropertyChanged("LinesNums");
            
            }
        }

        private int code;
        [Required(ErrorMessage = "code cannot be empty")]
        public int Code 
        {
            get => station.Code;
            set
            {
                SetProperty(ref code, value, true);
                station.Code = code;
            }
        }

        private string name;
        public string Name
        {
            get => station.Name;
            set
            {
                SetProperty(ref name, value, true);
                station.Name = name;
            }
        }

        private double longitude;
        [Range(34.3, 35.5, ErrorMessage = "longitude should be between 34.3 - 35.5")]
        public double Longitude 
        {
            get => station.Location.Longitude;
            set
            {
                SetProperty(ref longitude, value, true);
                station.Location.Longitude = longitude;
                OnPropertyChanged("Location");
            }
        }

        private double latitude;
        [Range(31, 33.3, ErrorMessage = "latitude should be between 31, 33.3")]
        public double Latitude 
        {
            get => station.Location.Latitude;
            set
            {
                SetProperty(ref latitude, value, true);
                station.Location.Latitude = latitude;
                OnPropertyChanged("Location");
            }
        }

        private string address;
        public string Address 
        {
            get => station.Address;
            set
            {
                SetProperty(ref address, value, true);
                station.Address = address;
            }
        }

        public GeoCoordinate Location
        {
            get => station.Location;
        }

        private ObservableCollection<int> linesNums;
        public ObservableCollection<int> LinesNums//bcause that lineNums need to be an ObservableCollection it a spetial case
        {
            get => linesNums;
            set
            {
                SetProperty(ref linesNums, value, true);
                station.LinesNums = linesNums.ToList();
            }
        }


        private void Station_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(HasErrors))
            {
                OnPropertyChanged(nameof(HasErrors)); // Update HasErrors on every change
            }
        }

        private void Station_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
