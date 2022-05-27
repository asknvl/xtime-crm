using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Views.converters
{
    public class UserCheckedToListItemBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            SolidColorBrush chkd = new SolidColorBrush(0xFFF5F5F5);
            SolidColorBrush unchkd = new SolidColorBrush(0x00000000);
            return (isChecked) ? chkd : unchkd;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
