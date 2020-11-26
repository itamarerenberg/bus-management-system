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
            this.statComboBox.ItemsSource = Enum.GetValues(typeof(StatEnum));
            bus = new Bus();
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
                MainWindow.Buses.Add(bus);
                bus = new Bus();
                this.DataContext = bus;
            } 
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource busViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("busViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // busViewSource.Source = [generic data source]
        }
    }
}
