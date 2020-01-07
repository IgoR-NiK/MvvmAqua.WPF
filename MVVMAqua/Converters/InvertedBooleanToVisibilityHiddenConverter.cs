using System.Windows;

namespace MVVMAqua.Converters
{
	public sealed class InvertedBooleanToVisibilityHiddenConverter : BooleanConverter<Visibility>
	{
		public InvertedBooleanToVisibilityHiddenConverter()
			: base(Visibility.Hidden, Visibility.Visible)
		{
		}
	}
}