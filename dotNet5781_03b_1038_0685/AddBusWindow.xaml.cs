using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Bus> Buses;
        public AddBusWindow(ObservableCollection<Bus> buses)
        {
            InitializeComponent();
            statComboBox.ItemsSource = Enum.GetValues(typeof(StatEnum));
            Buses = buses;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // try to generate bus from the input details
                string ln = licensNumTextBox.Text;
                DateTime sd = startDateDatePicker.DisplayDate;
                double kmat = double.TryParse(kmAfterTreatTextBox.Text, out kmat) ? kmat : 0,
                       skm = double.TryParse(sumKmTextBox.Text, out skm) ? skm : 0,
                       fikm = double.TryParse(fule_in_kmTextBox.Text, out fikm) ? fikm : 0;
                DateTime ltd = lastTreatDateDatePicker.DisplayDate;
                StatEnum st = statComboBox.SelectedItem == null ? StatEnum.READY : (StatEnum)statComboBox.SelectedItem;

                bus = new Bus(ln, sd, kmat, skm, fikm, ltd, st);

                
                Buses.Add(bus);//add the bus to the bus's list
            } 
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "ERORR");
            }
            this.Close();//close window
        }
    }
}
