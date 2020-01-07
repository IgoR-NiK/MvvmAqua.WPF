using System.Windows;

namespace MVVMAqua.Converters
{
	public sealed class InvertedBooleanToVisibilityCollapsedConverter : BooleanConverter<Visibility>
	{
		public InvertedBooleanToVisibilityCollapsedConverter()
			: base(Visibility.Collapsed, Visibility.Visible)
		{
		}
	}
}