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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace dotNet5781_03b_1038_0685
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public busCollection BusesCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            BusesCollection = new busCollection(RandBus.ListRB(10));
            BusesCollection.Buses.Add(RandBus.RB(fuel:50));
            BusesCollection.Buses.Add(RandBus.RB(km: 19950));
            //BusesCollection.Buses.Add(new Bus(1234567));

            this.DataContext = BusesCollection;
            main_list.ItemsSource = BusesCollection.Buses;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void TestGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = ((PropertyDescriptor)e.PropertyDescriptor).DisplayName;

        }

        private void SetARideButton_Click(object sender, RoutedEventArgs e)
        {
            Bus busForRide = (Bus)((Button)e.Source).DataContext;
            RideWindow RWindow = new RideWindow(busForRide);
            RWindow.ShowDialog();
        }

        private void Add_Bus_Button_Click(object sender, RoutedEventArgs e)
        {
            AddBusWindow addBusWindow = new AddBusWindow(BusesCollection.Buses);
            addBusWindow.ShowDialog();
        }

        private void FuelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bus row = (Bus)((Button)e.Source).DataContext;
                row.Refule();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message, "ERROR");
            }
        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {

            Bus selectedBus = (Bus)((ListViewItem)sender).Content;
            BusDetails busDetails = new BusDetails(selectedBus);
            busDetails.ShowDialog();
        }

        private void Add_Random_Bus_Button_Click(object sender, RoutedEventArgs e)
        {
            BusesCollection.Buses.Add(RandBus.RB());
        }

        private void treat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bus busForTreat = (Bus)((Button)e.Source).DataContext;
                Bus row = (Bus)((Button)e.Source).DataContext;
                busForTreat.Treatment();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message, "ERROR");
            }
        }
    }
}
