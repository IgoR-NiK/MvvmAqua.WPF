using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace MVVMAqua.Converters
{
    public abstract class BooleanConverter<T> : IValueConverter
    {
        public T True { get; set; }
        
        public T False { get; set; }
        
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                bool boolean => boolean ? True : False,
                _ => throw new InvalidOperationException($"The {nameof(value)} must be a {typeof(bool).Name}")
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                T tValue => EqualityComparer<T>.Default.Equals(tValue, True),
                _ => throw new InvalidOperationException($"The {nameof(value)} must be a {typeof(T).Name}")
            };
        }
    }
}