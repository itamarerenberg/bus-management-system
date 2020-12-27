using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLGui.Models
{
    public class Maneger : DependencyObject
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Maneger));




        public List<BO.Bus> buses
        {
            get { return (List<BO.Bus>)GetValue(busesProperty); }
            set { SetValue(busesProperty, value); }
        }
        public static readonly DependencyProperty busesProperty =
            DependencyProperty.Register("buses", typeof(List<BO.Bus>), typeof(Maneger));




        public List<BO.Station> Stations
        {
            get { return (List<BO.Station>)GetValue(StationsProperty); }
            set { SetValue(StationsProperty, value); }
        }
        public static readonly DependencyProperty StationsProperty =
            DependencyProperty.Register("Stations", typeof(List<BO.Station>), typeof(Maneger));
    }
}
