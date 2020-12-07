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
    /// Interaction logic for NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : INotifyPropertyChanged
    {
        public MessageBoxResult Result;
        private double? num = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public double? Num { get => num;
            set
            {
                num = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Num"));
            }

        }
       
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public NumericUpDownControl()
        {
            InitializeComponent();
        }
        private void cmdUp_Click(object sender, RoutedEventArgs e) { Num++; }
        private void cmdDown_Click(object sender, RoutedEventArgs e) { Num--; }
        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null || txtNum.Text == "" || txtNum.Text == "-")
            {
                Num = null;
                return;
            }
            if (!double.TryParse(txtNum.Text, out double val))
                txtNum.Text = Num.ToString();
            else Num = val;
        }

    }
}
