using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace dotNet5781_03b_1038_0685
{
    class Is_need_fule : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is double))
            {
                throw new ArgumentException("incorect value type");
            }
            double intvalue = (double)value;
            if(intvalue >= 1200)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
