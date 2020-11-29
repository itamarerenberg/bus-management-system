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
    /// Interaction logic for AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        Bus bus;
        public AddBusWindow()
        {
            InitializeComponent();
            statComboBox.ItemsSource = Enum.GetValues(typeof(StatEnum));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // try to generate bus from the input details
                bus = new Bus(licensNumTextBox.Text, startDateDatePicker.DisplayDate, double.Parse(kmAfterTreatTextBox.Text)
                , double.Parse(sumKmTextBox.Text), double.Parse(fule_in_kmTextBox.Text), lastTreatDateDatePicker.DisplayDate,
                (StatEnum)statComboBox.SelectedItem);
                
                RandBus.Buses.Add(bus);//add the bus to the bus's list
            } 
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "ERORR");
            }
            this.Close();
        }
    }
}
