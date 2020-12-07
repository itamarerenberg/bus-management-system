using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace dotNet5781_03b_1038_0685
{
    class Status_to_visibility : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatEnum status = (StatEnum)value;
            if (status == StatEnum.NEED_TREATMENT)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

