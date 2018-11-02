using System;
using MVVMAqua.ViewModels;
using MVVMAqua.Windows;

namespace MVVMAqua.Interfaces
{
	internal interface IBootstrapper
	{
		void OpenNewWindow<T>(T viewModel, Action<T> initialization = null, Func<IViewNavigator, bool> windowClosing = null) 
			where T : BaseVM;

		void OpenNewWindow<TViewModel, TWindow>(TViewModel viewModel, Action<TViewModel> initialization = null, Func<IViewNavigator, bool> windowClosing = null)
			where TViewModel : BaseVM
			where TWindow : BaseWindow, new();
	}
}