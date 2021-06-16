using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace UI.Code.Convertor
{
    public class ColorErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = (string)value;
            switch (text)
            {
                case "Success":
                    return Colors.Green;
                //case "John":
                //    return System.Windows.Media.Colors.Red;
                default:
                    return Colors.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class ColorFConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = (string)value;
            switch (text)
            {
                case "Yes":
                    return new SolidColorBrush(Colors.Red); ;
                //case "John":
                //    return System.Windows.Media.Colors.Red;
                default:
                    return new SolidColorBrush(Colors.Green); ;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
