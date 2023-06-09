using System;
using Avalonia.Data.Converters;

namespace SmartShortcuts.Converters
{
    public class LastWordFromActionConverter : IValueConverter
    {
        public static readonly LastWordFromActionConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            string? inputString = value as string;
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            string[] words = inputString.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0)
                return words[^1];

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}