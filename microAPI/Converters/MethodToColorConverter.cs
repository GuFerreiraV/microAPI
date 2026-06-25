using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace microAPI.Converters
{
    public class MethodToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string method)
            {
                string hexColor = method.ToUpper() switch
                {
                    "GET" => "#2E7D32",
                    "PUT" => "#E65100",
                    "POST" => "#1565C0",
                    "DELETE" => "#C62828",
                    _ => "#757575"
                };
                return SolidColorBrush.Parse(hexColor);
            }
            return Brushes.Gray;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter,
  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
