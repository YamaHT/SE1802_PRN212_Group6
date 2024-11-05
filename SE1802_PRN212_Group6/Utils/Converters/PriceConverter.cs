using System.Globalization;
using System.Windows.Data;

namespace SE1802_PRN212_Group6.Utils.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return $"${value}";
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
