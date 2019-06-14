using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Views
{
	class BaseView<T> : UserControl where T : BaseVM
	{
		protected T ViewModel => DataContext as T;
	}
}