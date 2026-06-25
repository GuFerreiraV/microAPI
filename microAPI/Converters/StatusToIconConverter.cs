using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Controls;
namespace microAPI.Converters
{
    public class StatusToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isOnline = value is bool b && b;

            // define o caminho do asset
            string assetPath = isOnline
            ? "avares://microAPI/Assets/online.png"
            : "avares://microAPI/Assets/offline.png";

            try
            {
                // carrega o asset utilizando o gerenciador do avalonia e retorna como WindowIcon
                var asset = AssetLoader.Open(new Uri(assetPath));
                return new WindowIcon(asset);
            }
            catch
            {
                return null;
            }
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
