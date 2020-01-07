using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MVVMAqua.Windows
{
	internal partial class ModalWindow
	{
		public ModalWindow()
		{
			InitializeComponent();
		}

		private Point _moveStart;

		private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && 
				!(e.OriginalSource is ButtonBase || e.OriginalSource is Thumb || e.OriginalSource is TextBoxBase))
			{
				var deltaPos = e.GetPosition(this) - _moveStart;
				Left += deltaPos.X;
				Top += deltaPos.Y;
			}
		}

		private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				_moveStart = e.GetPosition(this);
			}
		}
	}
}