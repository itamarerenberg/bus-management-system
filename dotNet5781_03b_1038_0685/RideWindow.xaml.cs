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
        public Bus BusForRide;
        public MessageBoxResult Result;
        public RideWindow(Bus busForRide)
        {
            InitializeComponent();
            BusForRide = busForRide;
            this.DataContext = BusForRide;
            kmNumUpDown.MaxValue = busForRide.Fule_in_km;
        }

        public void RefuelBus()
        {
            ProgBar.Visibility = Visibility.Visible;
            RefuleMsg.Visibility = Visibility.Visible;
            kmNumUpDown.IsEnabled = false;
            BusForRide.Refule();

            new Thread(() => 
            {
                for (int i = 0; i < 120; i++)
                {
                    Thread.Sleep(100);
                    ProgBar.Value++;
                }
            }).Start();
            ProgBar.Visibility = Visibility.Hidden;
            RefuleMsg.Visibility = Visibility.Hidden;
            kmNumUpDown.IsEnabled = true;
        }

        private void kmNumUpDown_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var input = sender as NumericUpDownControl;
            if (input.Num > input.MaxValue)
            {
                if (input.Num <= 1200)
                {
                    Result = MessageBox.Show($"this bus cannot rides over {input.MaxValue} km!\ndo you want to refuel the bus?", "ERORR", MessageBoxButton.YesNo);
                    if (Result == MessageBoxResult.Yes)
                    {
                        RefuelBus();
                    }
                    else
                    {
                        input.Num = input.MaxValue;
                        input.txtNum.Text = input.Num.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("a bus cannot rides over 1200 km!", "ERORR");
                    input.Num = 0;
                    input.txtNum.Text = input.Num.ToString();
                }
            }
            else if (input.Num < input.MinValue)
            {
                input.Num = input.MinValue;
                MessageBox.Show("enter a positive number", "ERORR");
            }
            else
                input.txtNum.Text = input.Num == null ? "" : input.Num.ToString();
        }

        private void kmNumUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    BusForRide.Ride((double)kmNumUpDown.Num);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERORR");
                }
            }
        }
    }
}
