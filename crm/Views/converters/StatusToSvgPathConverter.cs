using Avalonia.Data.Converters;
using crm.ViewModels.tabs.home.screens.users;
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
            //bool status = (bool)value;
            //string res = (status) ? "/Assets/svgs/screens/status_online.svg" : "/Assets/svgs/screens/status_offline.svg";
            //return res;

            UserStatus status = (UserStatus)value;
            switch (status)
            {
                case UserStatus.offline:                    
                    return "/Assets/svgs/screens/status_offline.svg";

                case UserStatus.online:
                    return "/Assets/svgs/screens/status_online.svg";

                case UserStatus.deleted:
                    return "/Assets/svgs/screens/status_deleted.svg";                    

                default:
                    return "/Assets/svgs/screens/status_offline.svg";
            }

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
