
using PLGui.Models;
using PLGui.Models.PO;
using PLGui.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for Maneger.xaml
    /// </summary>
    public partial class ManegerView : Window
    {
        ManegerViewModel vModel;
        public ManegerView()
        {
            vModel = new ManegerViewModel();
            this.DataContext = vModel;
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void StationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Station selectedStation = (Station)((ListViewItem)sender).Content;
            Header1.Visibility = Visibility.Visible;
            Header1.Text = "Name";
            content1.Visibility = Visibility.Visible;
            content1.Content = selectedStation.Name;
        }
    }
}
