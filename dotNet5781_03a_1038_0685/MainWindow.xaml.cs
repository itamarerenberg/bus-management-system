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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_1038_0685;

namespace dotNet5781_03a_1038_0685
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLine currentDisplayBusLine;
        private Lines lines = new Lines();
        public MainWindow()
        {
            InitializeComponent();

            MyRand.Rand_lines(lines, 8, 8);
            MyRand.Rand_cross_lines(lines, 5);

            cbBusLines.ItemsSource = lines;
            cbBusLines.DisplayMemberPath = " LineNum ";
            cbBusLines.SelectedIndex = 0;

            ShowBusLine(lines.Lines_list[0].LineNum);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).LineNum);
        }
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = lines[index];
            UpGrid.DataContext = currentDisplayBusLine;
            List<int> stsCode = new List<int>();
            currentDisplayBusLine.Stations.ForEach(st => stsCode.Add(st.Code));
            lbStationsCode.DataContext = stsCode;

            List<LocPoint> stsLoc = new List<LocPoint>();
            currentDisplayBusLine.Stations.ForEach(st => stsLoc.Add(st.base_station.Loc));
            lbLoc.DataContext = stsLoc;

            List<TimeSpan> stsRD = new List<TimeSpan>();
            currentDisplayBusLine.Stations.ForEach(st => stsRD.Add(st.RideTime));
            lbRideTime.DataContext = stsRD;

        }
    }
}
