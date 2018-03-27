using System;
using Xamarin.Forms;

namespace ShareQR.Converters
{
	public class DateTimeToLocalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

			var dateTime = (DateTime) value;
         
			return dateTime.ToLocalTime().ToString("MMMM dd, yyyy HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
