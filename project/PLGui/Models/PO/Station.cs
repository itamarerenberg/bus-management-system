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

        private int code;
        [Required(ErrorMessage = "code cannot be empty")]
        public int Code 
        {
            get => code;
            set => SetProperty(ref code, value, true);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        private double longitude;
        [Range(34.3, 35.5, ErrorMessage = "longitude should be between 34.3 - 35.5")]
        public double Longitude 
        {
            get => longitude;
            set
            {
                SetProperty(ref longitude, value, true);
                OnPropertyChanged("Location");
            }
        }

        private double latitude;
        [Range(31, 33.3, ErrorMessage = "latitude should be between 31, 33.3")]
        public double Latitude 
        {
            get => latitude;
            set
            {
                SetProperty(ref latitude, value, true);
                OnPropertyChanged("Location");
            }
        }

        private string address;
        public string Address 
        {
            get => address;
            set => SetProperty(ref address, value, true);
        }

        public GeoCoordinate Location
        { 
            get => new GeoCoordinate(Latitude, Longitude); 
        }

        private ObservableCollection<int> linesNums;
        public ObservableCollection<int> LinesNums 
        {
            get => linesNums;
            set => SetProperty(ref linesNums, value, true);
        }
    }
}
