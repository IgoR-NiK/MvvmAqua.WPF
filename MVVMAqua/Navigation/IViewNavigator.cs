using System;
using System.Windows;

using MVVMAqua.Enums;
using MVVMAqua.ViewModels;
using MVVMAqua.Navigation.Regions;
using MVVMAqua.Windows;

namespace MVVMAqua.Navigation
{
	/// <summary>
	/// Менеджер навигации между представлениями.
	/// </summary>
	public interface IViewNavigator
	{
		void NavigateTo<T>(T viewModel) where T : BaseVM;
		void NavigateTo<T>(T viewModel, Action<T> initialization) where T : BaseVM;

		/// <summary>
		/// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
		void NavigateTo<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM;


		void CloseLastView();

		/// <summary>
		/// Закрывает последнее представление и выполняет действие закрытия представления при необходимости.
		/// </summary>
		/// <param name="isCallbackCloseViewHandler">Флаг, указывающий нужно ли выполнять действие закрытия представления.</param>
		void CloseLastView(bool isCallbackCloseViewHandler);

		/// <summary>
		/// Закрывает все представления и выходит из главного окна.
		/// </summary>
		void CloseAllViews();


		bool ShowModalWindow(string text);
		bool ShowModalWindow(string text, ModalIcon icon);
		bool ShowModalWindow(string text, ModalIcon icon, string caption);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, ModalButtons buttonType);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText, Action okResult);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText, Action okResult);
		bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText, Action okResult, Action cancelResult);

		bool ShowModalWindow<T>(T viewModel) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, ModalButtons buttonType) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, Action<T> okResult) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM;

		bool ShowModalWindow<T>(T viewModel, Action<T> initialization) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, ModalButtons buttonType) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, Action<T> okResult) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM;
		bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM;


		void OpenNewWindow<T>(T viewModel) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<Window> windowInitialization) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<Window> windowInitialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;
		void OpenNewWindow<T>(Window window, T viewModel) where T : BaseVM;
		void OpenNewWindow<T>(Window window, T viewModel, Action<T> viewModelInitialization) where T : BaseVM;
		void OpenNewWindow<T>(BaseWindow window, T viewModel, Action<T> viewModelInitialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;

		RegionsCollection Regions { get; }
	}
}