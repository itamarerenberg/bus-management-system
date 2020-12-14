using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
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
            kmNumUpDown.txtNum.Focus();
            kmNumUpDown.txtNum.SelectAll();
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
                        BusForRide.Refule();
                        start_progresbar("refueling... please wait!");
                    }
                    else
                    {
                        input.Num = input.MaxValue;
                        input.txtNum.Text = input.Num.ToString();
                        kmNumUpDown.txtNum.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("a bus cannot rides over 1200 km!", "ERORR");
                    input.Num = 0;
                    input.txtNum.Text = input.Num.ToString();
                    kmNumUpDown.txtNum.SelectAll();
                }
            }
            else if (input.Num < input.MinValue)
            {
                input.Num = input.MinValue;
                MessageBox.Show("enter a positive number", "ERORR");
                kmNumUpDown.txtNum.SelectAll();
            }
            else
                input.txtNum.Text = input.Num == null ? "" : input.Num.ToString();
        }

        private void kmNumUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (kmNumUpDown.Num == null || kmNumUpDown.Num == 0)
                {
                    MessageBox.Show("insert a value", "ERORR");
                    kmNumUpDown.txtNum.SelectAll();
                    return;
                }
                try
                {
                    BusForRide.Ride((double)kmNumUpDown.Num);
                    this.Close();
                }
                catch(Busy ex)
                {
                    Result = MessageBox.Show(ex.Message, "ERORR", MessageBoxButton.OK);
                }
                catch(Danger ex)
                {
                    Result = MessageBox.Show(ex.Message + "\ndo you want to send the bus to treatment?", "ERORR", MessageBoxButton.YesNo);
                    if (Result == MessageBoxResult.Yes)
                    {
                        BusForRide.Treatment();
                        start_progresbar("in treatment... plese wait");
                    }
                }
                catch (NotEnoughFule ex)//#split tot two difrent exptions
                {
                    Result = MessageBox.Show(ex.Message + "\ndo you want to refule the bus?", "ERORR", MessageBoxButton.YesNo);
                    if (Result == MessageBoxResult.Yes)
                    {
                        BusForRide.Refule();
                        start_progresbar("in refuling... plese wait");
                    }
                }
                catch (NeedTreatment ex)//#split tot two difrent exptions
                {
                    Result = MessageBox.Show(ex.Message + "\ndo you want to send the bus to treatment?", "ERORR", MessageBoxButton.YesNo);
                    if (Result == MessageBoxResult.Yes)
                    {
                        BusForRide.Treatment();
                        start_progresbar("in treatment... plese wait");
                    }
                }
                catch(Exception ex)
                {
                    Result = MessageBox.Show(ex.Message, "ERORR", MessageBoxButton.OK);
                }
            }
        }
        private void start_progresbar( string str)
        {
            kmNumUpDown.IsEnabled = false;
            ProgBarMsg.Content = str;
            ProgBarMsg.Visibility = Visibility.Visible;
            ProgBar.Visibility = Visibility.Visible;

            Duration duration = new Duration(BusForRide.Time_until_ready);
            DoubleAnimation doubleanimation = new DoubleAnimation(ProgBar.Maximum, duration)
            {
                FillBehavior = FillBehavior.Stop
            };
            doubleanimation.Completed += (s, e) =>
            {
                ProgBarMsg.Visibility = Visibility.Collapsed;
                ProgBar.Visibility = Visibility.Collapsed;
                kmNumUpDown.IsEnabled = true;
                ProgBar.Value = 0;
                kmNumUpDown.txtNum.Focus();
            };
            ProgBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }

    }
}
