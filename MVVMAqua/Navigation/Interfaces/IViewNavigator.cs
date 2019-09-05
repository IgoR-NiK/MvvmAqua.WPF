using System;
using System.Windows;

using MVVMAqua.Enums;
using MVVMAqua.ViewModels;
using MVVMAqua.Navigation.Regions;
using System.Windows.Media;

namespace MVVMAqua.Navigation.Interfaces
{
	/// <summary>
	/// Менеджер навигации между представлениями.
	/// </summary>
	public interface IViewNavigator : INavigator
	{
        Window Window { get; }

        INavigator Parent { get; }
        RegionsCollection Regions { get; }

		Color ModalWindowColorTheme { get; set; }
		void SetWindowType<T>() where T : Window, new();

		void CloseWindow();
        void CloseWindow(bool isCallbackCloseWindowHandler);

        #region Открытие нового окна

        void OpenNewWindow<T>(T viewModel)
           where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization)
           where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed)
          where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed)
           where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM;

        void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM;

        void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window;

        void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window;

        void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window;

        void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed, Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window;

        #endregion
               
        #region ShowDialog

        bool ShowDialog(string text);
		bool ShowDialog(string text, ModalIcon icon);
		bool ShowDialog(string text, ModalIcon icon, string caption);
		bool ShowDialog(string text, ModalIcon icon, string caption, ModalButtons buttonType);
		bool ShowDialog(string text, ModalIcon icon, string caption, string btnOkText);
		bool ShowDialog(string text, ModalIcon icon, string caption, string btnOkText, Action okResult);
		bool ShowDialog(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText);
		bool ShowDialog(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText, Action okResult);
		bool ShowDialog(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText, Action okResult, Action cancelResult);

		bool ShowDialog<T>(T viewModel) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, ModalButtons buttonType) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, string btnOkText) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, string btnOkText, Action<T> okResult) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, string btnOkText, string btnCancelText) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM;

		bool ShowDialog<T>(T viewModel, Action<T> initialization) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, ModalButtons buttonType) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, string btnOkText) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, Action<T> okResult) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM;
		bool ShowDialog<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM;

        bool ShowDialog<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel)
            where ViewModelType : BaseVM, IDialogClosing
            where WindowType : Window;

        bool ShowDialog<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization)
            where ViewModelType : BaseVM, IDialogClosing
            where WindowType : Window;

        bool ShowDialog<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> okResult)
            where ViewModelType : BaseVM, IDialogClosing
            where WindowType : Window;

        bool ShowDialog<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> okResult, Action<ViewModelType> cancelResult)
            where ViewModelType : BaseVM, IDialogClosing
            where WindowType : Window;

        #endregion
	}
}