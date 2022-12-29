using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UI.Code.Convertor
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(value.ToString()));
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            //bitmap.DecodePixelHeight =  25;
            //bitmap.DecodePixelWidth = 25;
            return bitmap;
        }
        
        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

       
    }
}
