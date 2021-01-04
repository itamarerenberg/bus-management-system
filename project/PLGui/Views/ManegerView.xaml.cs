
using PLGui.Models;
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
        public ManegerView(ManegerViewModel vm)
        {
            InitializeComponent();
            vModel = vm;
            this.DataContext = vModel;
            StationList.ItemsSource = vModel.Buses;
        }

        
    }
}
