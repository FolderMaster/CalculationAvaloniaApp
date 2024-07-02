using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace View.Converters
{
    internal class ValueToAlignmentConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter?.ToString() == value?.ToString())
            {
                return targetType == typeof(HorizontalAlignment) ?
                    HorizontalAlignment.Stretch : VerticalAlignment.Stretch;
            }
            return targetType == typeof(HorizontalAlignment) ?
                HorizontalAlignment.Center : VerticalAlignment.Center;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter,
            CultureInfo culture) => throw new NotImplementedException();
    }
}
