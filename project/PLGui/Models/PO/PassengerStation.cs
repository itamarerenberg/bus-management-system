using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;


namespace PLGui.Models.PO
{
    /// <summary>
    /// station for using in passengers view for scheduling user trip. contains the list of line stations of station
    /// </summary>
    public class PassengerStation : ObservableObject
    {
        private List<BO.LineStation> lineStations;

        public List< BO.LineStation> LineStations
        {
            get => lineStations;
            set => SetProperty(ref lineStations, value);
        }

        public int StationNumber { get; set; }

        public override string ToString()
        {
            return LineStations.First().Name;
        }
    }
}
