using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MVVMAqua.Windows
{
	/// <summary>
	/// Логика взаимодействия для ModalWindow.xaml
	/// </summary>
	partial class ModalWindow : BaseWindow
	{
		public ModalWindow()
		{
			InitializeComponent();
		}

		Point moveStart;

		private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && 
				!(e.OriginalSource is ButtonBase || e.OriginalSource is Thumb || e.OriginalSource is TextBoxBase))
			{
				var deltaPos = e.GetPosition(this) - moveStart;
				Left += deltaPos.X;
				Top += deltaPos.Y;
			}
		}

		private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				moveStart = e.GetPosition(this);
			}
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}