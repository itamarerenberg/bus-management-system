using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public MessageBoxResult Result;
        public RideWindow(Bus bus)
        {
            InitializeComponent();
            busForRide = bus;
            kmNumUpDown.MaxValue = busForRide.Fule_in_km;
            if (kmNumUpDown.NeedsRefuel)
                RefuelBus();
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
        public void RefuelBus()
        {
            SetRideButton.IsEnabled = false;
            ProgBar.Visibility = Visibility.Visible;
            RefuleMsg.Visibility = Visibility.Visible;
            busForRide.Refule();
            for (int i = 0; i < 120; i++)
            {
            new Thread(() => {
                Thread.Sleep(100);
                ProgBar.Value++;
            }).Start();
            }
            SetRideButton.IsEnabled = true;
            ProgBar.Visibility = Visibility.Hidden;
            RefuleMsg.Visibility = Visibility.Hidden;

        }

    }
}
