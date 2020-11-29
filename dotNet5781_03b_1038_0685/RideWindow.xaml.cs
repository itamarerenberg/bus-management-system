using System;
using System.Collections.Generic;
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

namespace dotNet5781_03b_1038_0685
{
    /// <summary>
    /// Interaction logic for RideWindow.xaml
    /// </summary>
    public partial class RideWindow : Window
    {
        public Bus busForRide;
        public RideWindow(Bus bus)
        {
            InitializeComponent();
            busForRide = bus;
            kmNumUpDown.MaxValue = busForRide.Fule_in_km;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busForRide.Ride((double)kmNumUpDown.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERORR");
            }
            this.Close();
        }

    }
}
