using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using MVVMAqua.ViewModels;
using MVVMAqua.Views;
using MVVMAqua.Windows;
using MVVMAqua.Navigation;
using System.Windows.Media;

namespace MVVMAqua
{
	public sealed class Bootstrapper
	{
		internal Dictionary<Type, Type> ViewModelToViewMap { get; } = new Dictionary<Type, Type>()
		{
			[typeof(ModalMessageVM)] = typeof(ModalMessageView)
		};

		public Bootstrapper()
		{
			var callingAssembly = Assembly.GetCallingAssembly();
			AutoMappingViewModelToView(callingAssembly);
		}

        #region Привязка View к ViewModel

        /// <summary>
        /// Автоматическая привязка VM к View. 
        /// VM должна иметь следующие названия: Name, NameVM или NameViewModel. 
        /// View должна иметь следующие названия: Name или NameView. 
        /// Регистр значения не имеет.
        /// </summary>
        /// <param name="assembly">Сборка, в которой производится поиск ViewModel и View.</param>
        private void AutoMappingViewModelToView(Assembly assembly)
		{
			var viewModels = assembly
				.GetTypes()
				.Where(x => typeof(BaseVM).IsAssignableFrom(x))
				.ToList();

			var views = assembly
				.GetTypes()
				.Where(x => typeof(ContentControl).IsAssignableFrom(x) && x.GetConstructor(Type.EmptyTypes) != null)
				.ToList();				

			viewModels.ForEach(vm =>
			{
				var viewModelName = vm.Name.ToLower();
				if (viewModelName.EndsWith("vm"))
				{
					viewModelName = viewModelName.Remove(viewModelName.Length - "vm".Length);
				}
				else if (viewModelName.EndsWith("viewmodel"))
				{
					viewModelName = viewModelName.Remove(viewModelName.Length - "viewmodel".Length);
				}

				var view = views.FirstOrDefault(v => v.Name.ToLower() == viewModelName || v.Name.ToLower() == $"{viewModelName}view");

				if (view != null && !ViewModelToViewMap.ContainsKey(vm))
				{
					ViewModelToViewMap.Add(vm, view);
				}
			});
		}


		private Type tempVM;

		public Bootstrapper Bind<T>() where T : BaseVM
		{
			if (ViewModelToViewMap.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("Для указанного типа ViewModel представление уже зарегистрировано.");
			}

			tempVM = typeof(T);
			return this;
		}

		public void To<T>() where T : ContentControl, new()
		{
			if (tempVM != null)
			{
				ViewModelToViewMap.Add(tempVM, typeof(T));
				tempVM = null;
			}
		}

        #endregion

        #region Настройка модального и обычного окон

        /// <summary>
        /// Цвет темы модального окна.
        /// </summary>
        public Color ModalWindowColorTheme { get; set; } = Color.FromRgb(0x4A, 0x76, 0xC9);


		private Type windowType = typeof(MainWindow);

		public void SetWindowType<T>() where T : Window, new()
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