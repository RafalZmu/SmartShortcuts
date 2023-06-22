using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Avalonia.Data.Converters;
using SmartShortcuts.Models;

namespace SmartShortcuts.Converters
{
    public class LastWordFromActionConverter : IValueConverter
    {
        public static readonly LastWordFromActionConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            List<Models.Action> inputAction = value as List<Models.Action>;
            List<string> actionsList = new();
            inputAction.ForEach(action =>
            {
                actionsList.Add(action.Path.Split(@"\").Last());
            });

            string word = string.Join("+", actionsList);
            return word;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}