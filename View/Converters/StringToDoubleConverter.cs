using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace View.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter,
            CultureInfo culture) => value?.ToString();


        public object? ConvertBack(object? value, Type targetType, object? parameter,
            CultureInfo culture)
        {
            if (value == null)
            {
                return BindingOperations.DoNothing;
            }
            double result = 0.0d;
            if (double.TryParse((string)value, out result))
            {
                return result;
            }
            return BindingOperations.DoNothing;
        }
    }
}
