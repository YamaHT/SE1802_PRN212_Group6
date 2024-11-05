using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace SE1802_PRN212_Group6.Utils.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            string imageName = value.ToString();
            string entity = parameter.ToString();

            string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string directoryPath = Path.Combine(rootPath, "Images", entity);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string imagePath = Path.Combine(directoryPath, imageName);
            
            return imagePath;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
