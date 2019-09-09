using MVVMAqua.ViewModels;

namespace MVVMAqua.Views
{
	interface IBaseView<out T> 
		where T : BaseVM
	{
		T ViewModel { get; }
	}
}