using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using MVVMAqua.Navigation;
using MVVMAqua.ViewModels;
using MVVMAqua.Views;
using MVVMAqua.Windows;

namespace MVVMAqua
{
	public sealed class Bootstrapper
	{
		internal Dictionary<Type, Type> ViewModelToViewMap { get; } = new Dictionary<Type, Type>
		{
			[typeof(ModalMessageVM)] = typeof(ModalMessageView),
            [typeof(ModalWindowVM)] = typeof(ModalWindowView)
		};


		internal Bootstrapper() { }


        #region Настройка модального и обычного окон

        internal Color ModalWindowColorTheme { get; set; } = Color.FromRgb(0x4A, 0x76, 0xC9);


		private Type _windowType = typeof(MainWindow);

		internal void SetWindowType<T>() 
            where T : Window, new()
		{
			_windowType = typeof(T);
		}
        
        #endregion

        #region OpenNewWindow

        public void OpenNewWindow<T>(T viewModel)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, null, null, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, null, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Action<T>? afterViewClosed)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Func<T, bool>? afterViewClosed)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Action<T>? afterViewClosed, Action<Window>? windowInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Func<T, bool>? afterViewClosed, Action<Window>?windowInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Action<T>? afterViewClosed, Action<Window>? windowInitialization, Action<T>? windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Action<T>? afterViewClosed, Action<Window>? windowInitialization, Func<T, bool>? windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Func<T, bool>? afterViewClosed, Action<Window>? windowInitialization, Action<T>? windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<T>(T viewModel, Action<T>? viewModelInitialization, Func<T, bool>? afterViewClosed, Action<Window>? windowInitialization, Func<T, bool>? windowClosing)
            where T : BaseVM
        {
            var window = (Window)Activator.CreateInstance(_windowType);
            OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<TViewModel, TWindow>(TWindow window, TViewModel viewModel, Action<TViewModel>? viewModelInitialization, Action<TViewModel>? afterViewClosed, Action<TWindow>? windowInitialization, Action<TViewModel>? windowClosing)
            where TViewModel : BaseVM
            where TWindow : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<TViewModel, TWindow>(TWindow window, TViewModel viewModel, Action<TViewModel>? viewModelInitialization, Action<TViewModel>? afterViewClosed, Action<TWindow>? windowInitialization, Func<TViewModel, bool>? windowClosing)
            where TViewModel : BaseVM
            where TWindow : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<TViewModel, TWindow>(TWindow window, TViewModel viewModel, Action<TViewModel>? viewModelInitialization, Func<TViewModel, bool>? afterViewClosed, Action<TWindow>? windowInitialization, Action<TViewModel>? windowClosing)
            where TViewModel : BaseVM
            where TWindow : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<TViewModel, TWindow>(TWindow window, TViewModel viewModel, Action<TViewModel>? viewModelInitialization, Func<TViewModel, bool>? afterViewClosed,  Action<TWindow>? windowInitialization, Func<TViewModel, bool>? windowClosing)
            where TViewModel : BaseVM 
            where TWindow : Window
        {
            windowInitialization?.Invoke(window);
            var navigator = new ViewNavigator(this, window, window, null);
            if (window is BaseWindow baseWindow)
            {
                baseWindow.WindowClosing = () => windowClosing?.Invoke(viewModel) ?? true;
            }
            navigator.NavigateTo(viewModel, viewModelInitialization, afterViewClosed);

            window.Show();
        }

        #endregion
    }
}