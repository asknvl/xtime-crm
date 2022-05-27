using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Views.converters
{
    public class StatusToSvgPathConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            string res = (status) ? "/Assets/svgs/screens/status_online.svg" : "/Assets/svgs/screens/status_offline.svg";
            return res;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
