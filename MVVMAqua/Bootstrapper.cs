using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using MVVMAqua.ViewModels;
using MVVMAqua.Views;
using MVVMAqua.Windows;
using MVVMAqua.Navigation;

namespace MVVMAqua
{
	public sealed class Bootstrapper
	{
		internal Dictionary<Type, Type> ViewModelToViewMap { get; } = new Dictionary<Type, Type>()
		{
			[typeof(ModalMessageVM)] = typeof(ModalMessageView),
            [typeof(ModalWindowVM)] = typeof(ModalWindowView)
		};


		internal Bootstrapper() { }


        #region Настройка модального и обычного окон

        internal Color ModalWindowColorTheme { get; set; } = Color.FromRgb(0x4A, 0x76, 0xC9);


		private Type windowType = typeof(MainWindow);

		internal void SetWindowType<T>() where T : Window, new()
		{
			windowType = typeof(T);
		}

        #endregion

        #region OpenNewWindow

        public void OpenNewWindow<T>(T viewModel)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, null, null, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, null, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, null, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, null);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM
        {
            OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM
        {
            var window = Activator.CreateInstance(windowType) as Window;
            OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, vm => { afterViewClosed?.Invoke(vm); return true; }, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, vm => { windowClosing?.Invoke(vm); return true; });
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed,  Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM 
            where WindowType : Window
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