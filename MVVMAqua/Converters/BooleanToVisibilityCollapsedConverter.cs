using System.Windows;

namespace MVVMAqua.Converters
{
	public sealed class BooleanToVisibilityCollapsedConverter : BooleanConverter<Visibility>
	{
		public BooleanToVisibilityCollapsedConverter()
			: base(Visibility.Visible, Visibility.Collapsed)
		{
		}
	}
}