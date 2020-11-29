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

namespace dotNet5781_03b_1038_0685
{
    /// <summary>
    /// Interaction logic for NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {
        public MessageBoxResult Result;
        private double? num = null;
        public double? Value 
        { 
            get { return num; } 
            set { 
                if (value > MaxValue)
                {
                    num = MaxValue;
                    if (value < 1200)
                    {
                        Result = MessageBox.Show($"this bus cannot rides over {MaxValue} km!\ndo you want to refuel the bus", "ERORR", MessageBoxButton.YesNo);
                        
                    }
                    else
                        MessageBox.Show("a bus cannot rides over 1200 km!", "ERORR");
                }
                else if (value < MinValue)
                {
                    num = MinValue;
                    MessageBox.Show("enter a positive number", "ERORR");
                }
                else num = value;
                txtNum.Text = num == null ? "" : num.ToString(); 
            }
        }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public NumericUpDownControl()
        { 
            InitializeComponent(); 
        }
        private void cmdUp_Click(object sender, RoutedEventArgs e) { Value++; }
        private void cmdDown_Click(object sender, RoutedEventArgs e) { Value--; }
        private void txtNum_TextChanged(object sender, TextChangedEventArgs e) 
        {
            if (txtNum == null || txtNum.Text == "" || txtNum.Text == "-")
            {
                Value = null;
                return; 
            } 
            if (!double.TryParse(txtNum.Text, out double val)) 
                txtNum.Text = Value.ToString();
            else Value = val; 
        }
    }
}
