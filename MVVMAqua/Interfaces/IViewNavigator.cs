using System;
using System.Windows;

using MVVMAqua.Enums;
using MVVMAqua.ViewModels;
using MVVMAqua.Navigation.Regions;
using MVVMAqua.Windows;

namespace MVVMAqua.Interfaces
{
	/// <summary>
	/// Менеджер навигации между представлениями.
	/// </summary>
	public interface IViewNavigator
	{
		/// <summary>
		/// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
		void NavigateTo<T>(T viewModel, Action<T> initialization = null, Func<T, bool> afterViewClosed = null) where T : BaseVM;

		/// <summary>
		/// Закрывает последнее представление и выполняет действие закрытия представления при необходимости.
		/// </summary>
		/// <param name="isCallbackCloseViewHandler">Флаг, указывающий нужно ли выполнять действие закрытия представления.</param>
		void CloseLastView(bool isCallbackCloseViewHandler = true);

		/// <summary>
		/// Закрывает все представления и выходит из главного окна.
		/// </summary>
		void CloseAllViews();

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным текстом.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		bool ShowModalWindow(string text, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action okResult = null, Action cancelResult = null, ModalIcon icon = ModalIcon.None);

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным представлением.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		bool ShowModalWindow<T>(T viewModel, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action<T> okResult = null, Action<T> cancelResult = null, Action<T> initialization = null) where T : BaseVM;

		void OpenNewWindow<T>(T viewModel) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Action<T> initialization) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;
		void OpenNewWindow<T>(T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;
		void OpenNewWindow<T>(Window window, T viewModel) where T : BaseVM;
		void OpenNewWindow<T>(Window window, T viewModel, Action<T> initialization) where T : BaseVM;
		void OpenNewWindow<T>(BaseWindow window, T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;
		void OpenNewWindow<T>(BaseWindow window, T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM;

		RegionsCollection Regions { get; }
	}
}