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

namespace dotNet5781_03b_1038_0685
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            RandBus.Buses = RandBus.ListRB(10);
            TestGrid.ItemsSource = RandBus.Buses;
        }

        private void TestGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = ((PropertyDescriptor)e.PropertyDescriptor).DisplayName;
            
            //var row = (DataGridRow)sender;
            //if (RandBus.Buses[row.GetIndex()].Stat == StatEnum.READY)
            //{
            //    row.Background;
            //}
        }

        private void SetARideButton_Click(object sender, RoutedEventArgs e)
        {
            Bus busForRide = (Bus)((Button)e.Source).DataContext;
            RideWindow RWindow = new RideWindow(busForRide);
            RWindow.ShowDialog();
            TestGrid.Items.Refresh();
        }

        private void Add_Bus_Button_Click(object sender, RoutedEventArgs e)
        {
            AddBusWindow addBusWindow = new AddBusWindow();
            addBusWindow.ShowDialog();
            TestGrid.Items.Refresh();
        }

        private void FuelButton_Click(object sender, RoutedEventArgs e)
        {
            Bus row = (Bus)((Button)e.Source).DataContext;
            row.Refule();
            TestGrid.Items.Refresh();
        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = (DataGridRow)sender;
            MessageBox.Show(RandBus.Buses[row.GetIndex()].LicensNum);
        }

    }
}
