using System.Windows;

namespace MVVMAqua.Converters
{
	public sealed class BooleanToVisibilityHiddenConverter : BooleanConverter<Visibility>
	{
		public BooleanToVisibilityHiddenConverter()
			: base(Visibility.Visible, Visibility.Hidden)
		{
		}
	}
}