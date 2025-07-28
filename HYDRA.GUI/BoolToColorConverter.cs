using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HYDRA.GUI
{
    public class BoolToColorConverter : IValueConverter
    {
        // Singleton instance for XAML usage
        public static readonly BoolToColorConverter Instance = new BoolToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? new SolidColorBrush(Color.FromRgb(46, 204, 113)) // Example: green for true
                         : new SolidColorBrush(Color.FromRgb(231, 76, 60)); // Example: red for false
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}