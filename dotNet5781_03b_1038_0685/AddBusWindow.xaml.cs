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
            bus = new Bus();
            this.statComboBox.ItemsSource = Enum.GetValues(typeof(StatEnum));
            this.DataContext = bus;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                //licensNumTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //startDateDatePicker.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //lastTreatDateDatePicker.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //sumKmTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //kmAfterTreatTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //fule_in_kmTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                bus = new Bus(licensNumTextBox.Text, startDateDatePicker.DisplayDate, double.Parse(kmAfterTreatTextBox.Text)
                , double.Parse(sumKmTextBox.Text), double.Parse(fule_in_kmTextBox.Text), lastTreatDateDatePicker.DisplayDate,
                (StatEnum)statComboBox.SelectedItem);
                MainWindow.Buses.Add(bus);
            } 
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }
    }
}
