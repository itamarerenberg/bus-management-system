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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class BusDetails : Window
    {
        public BusDetails(Bus bus)
        {
            InitializeComponent();
            this.DataContext = bus;
            this.Title = $"bus number: {bus.LicensNum}";
            tbTitle.Text = $"bus number: {bus.LicensNum}";
        }

        private void SetARideButton_Click(object sender, RoutedEventArgs e)
        {
            Bus busForRide = (Bus)((Button)e.Source).DataContext;
            RideWindow RWindow = new RideWindow(busForRide);//opens the ride window
            RWindow.ShowDialog();
        }

        private void FuelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bus row = (Bus)((Button)e.Source).DataContext;
                row.Refule();//send the bus to refule
                this.Close();//close the window
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message, "ERROR");
            }
        }
    }
}
