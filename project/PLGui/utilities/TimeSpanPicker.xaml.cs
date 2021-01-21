using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLGui.utilities
{
    /// <summary>
    /// Interaction logic for TimeSpanPicker.xaml
    /// </summary>
    public partial class TimeSpanPicker : UserControl
    {
        public TimeSpanPicker()
        {
            InitializeComponent();
        }

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimeSpanPicker), new PropertyMetadata(new TimeSpan()));

        private void BtnUpHour_OnClick(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(Hour.Text);
            if (value < 23)
            {
                Hour.Text = (value + 1) <= 9 ? "0" + (++value).ToString() : (++value).ToString();
            }
            else if (value == 23)
            {
                Hour.Text = "01";
            }
            Time += TimeSpan.FromHours(1);
        }

        private void BtnUpMinute_OnClick(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(Minute.Text);
            if (value < 59)
            {
                Minute.Text = (value + 1) <= 9 ? "0" + (++value).ToString() : (++value).ToString();
            }
            else if (value == 59)
            {
                Minute.Text = "01";
            }
            Time += TimeSpan.FromMinutes(1);
        }

        private void BtnDownHour_OnClick(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(Hour.Text);
            if (value > 0)
            {
                Hour.Text = (value - 1) <= 9 ? "0" + (--value).ToString() : (--value).ToString();
            }
            else if (value == 0)
            {
                Hour.Text = "23";
            }
            Time -= TimeSpan.FromHours(1);
        }

        private void BtnDownMinute_OnClick(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(Minute.Text);
            if (value > 0)
            {
                Minute.Text = (value - 1) <= 9 ? "0" + (--value).ToString() : (--value).ToString();
            }
            else if (value == 0)
            {
                Minute.Text = "59";
            }
            Time -= TimeSpan.FromMinutes(1);
        }

        private void Hour_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Hour.SelectAll();
        }

        private void Minute_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Minute.SelectAll();
        }

        private void Minute_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Minute.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                Minute.Focus();
            }
        }

        private void Hour_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Hour.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                Hour.Focus();
            }
        }
        private void Minute_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Hour_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Hour_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int resultHour = int.Parse(Hour.Text);
            if (resultHour > 23)
            {
                Hour.Text = "23";
            }
        }
        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //((RepeatButton)sender).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00A8DE"));
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //((RepeatButton)sender).Foreground = Brushes.White;
        }
    
    }
}
