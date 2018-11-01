using System;
using System.Globalization;
using System.Windows.Data;

namespace MvvmAqua.Converters
{
	class InvertedBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is bool ? !(bool)value : throw new InvalidOperationException("The target must be a boolean");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is bool ? !(bool)value : throw new InvalidOperationException("The target must be a boolean");
		}
	}
}
