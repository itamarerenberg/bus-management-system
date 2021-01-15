using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PLGui.ViewModels
{
    class Text_converter_to_visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag)
            {
                if (flag)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            //if (value is TabControl tab)
            //{
            //    if (tab.IsFocused || tab.SelectedItem != null)
            //    {                   
            //        if ((tab.SelectedItem as TabItem).Content is ListView selectedList)
            //        {
            //            if(selectedList.SelectedItem != null)
            //            {
            //                return Visibility.Visible;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return Visibility.Collapsed;
            //    }
            //}
            //צריך למחוק
            //if (value is TextBlock)
            //{
            //    if (value as TextBlock == null)
            //    {
            //        return Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        return Visibility.Visible;
            //    }
            //}
            //if (value is Label)
            //{
            //    if (value as Label == null)
            //    {
            //        return Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        return Visibility.Visible;
            //    }
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
