using System;
using System.Globalization;
using System.Windows.Data;

namespace GifApp
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the string value to an integer
            if (value is string stringValue && int.TryParse(stringValue, out int intValue))
            {
                return intValue;
            }

            // Return a default value or handle the conversion failure as needed
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This method is not used for one-way conversions
            throw new NotSupportedException();
        }
    }
}