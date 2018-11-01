using System;
using MVVMAqua.Enums;
using MVVMAqua.ViewModels;
using MVVMAqua.Windows;

namespace MVVMAqua.Interfaces
{
	internal interface IBootstrapper
	{
		void OpenNewWindow<T>(T viewModel, Action<T> initialization = null, Func<BaseWindow, bool> windowClosing = null) 
			where T : BaseVM;

		void OpenNewWindow<TViewModel, TWindow>(TViewModel viewModel, Action<TViewModel> initialization = null, Func<TWindow, bool> windowClosing = null)
			where TViewModel : BaseVM
			where TWindow : BaseWindow, new();
		
		/// <summary>
		/// Отображает модальное диалоговое окно с указанным текстом.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		bool ShowModalWindow(BaseWindow window, string text, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action okResult = null, Action cancelResult = null, ModalIcon icon = ModalIcon.None);

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным представлением.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		bool ShowModalWindow<T>(BaseWindow window, T viewModel, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action<T> okResult = null, Action<T> cancelResult = null, Action<T> initialization = null) where T : BaseVM;

	}
}