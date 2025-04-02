using System;
using System.Globalization;
using System.Windows.Data;

namespace Beauty_Salon.Resources
{
    public class TruncateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int maxLength = 15; // Максимальная длина строки
            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue) && stringValue.Length > maxLength)
            {
                return stringValue.Substring(0, maxLength) + "...";
            }

            return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
