using System;
using System.Globalization;
using System.Windows.Data;

namespace WellmansAndHaraldsEconomyApp
{
    class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str))
                return 0;
            return double.Parse(str, culture);
        }
    }
}
