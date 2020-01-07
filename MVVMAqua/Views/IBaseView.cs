using MVVMAqua.ViewModels;

namespace MVVMAqua.Views
{
	internal interface IBaseView<out T> 
		where T : BaseVM
	{
		T ViewModel { get; }
	}
}