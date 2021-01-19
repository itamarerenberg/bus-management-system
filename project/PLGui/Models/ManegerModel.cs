using PLGui.Models.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLGui.Models
{
    public class ManegerModel
    {
        public ObservableCollection<Bus> Buses;

        public ObservableCollection<Line> Lines;

        public ObservableCollection<Station> Stations;

        public ObservableCollection<LineTrip> LineTrips;

    }
}
