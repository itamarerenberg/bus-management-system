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
        public ObservableCollection<Bus> Buses { get; set; }

        public ObservableCollection<Line> Lines { get; set; }

        public ObservableCollection<Station> Stations { get; set; }
    }
}
