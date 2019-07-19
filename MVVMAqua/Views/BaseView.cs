using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Views
{
	public class BaseView<T> : UserControl, IBaseView<T>
		where T : BaseVM
	{
		public T ViewModel => DataContext as T;
	}
}