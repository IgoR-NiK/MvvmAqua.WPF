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


		/// <summary>
		/// Цвет темы модального окна.
		/// </summary>
		public Color ModalWindowColorTheme { get; set; } = Color.FromRgb(0x4A, 0x76, 0xC9);


		private Type windowType = typeof(MainWindow);

		public void SetWindowType<T>() where T : Window, new()
		{
			windowType = typeof(T);
		}


		public void OpenNewWindow<T>(T viewModel) where T : BaseVM
		{
			OpenNewWindow(viewModel, null, null);
		}
		public void OpenNewWindow<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			OpenNewWindow(viewModel, initialization, null);
		}
		public void OpenNewWindow<T>(T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			OpenNewWindow(viewModel, null, windowClosing);
		}	 
		public void OpenNewWindow<T>(T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			var window = Activator.CreateInstance(windowType) as Window;
			OpenNewWindowPrivate(window, viewModel, initialization, windowClosing);
		}
		public void OpenNewWindow<T>(Window window, T viewModel) where T : BaseVM
		{
			OpenNewWindowPrivate(window, viewModel, null, null);
		}
		public void OpenNewWindow<T>(Window window, T viewModel, Action<T> initialization) where T : BaseVM
		{
			OpenNewWindowPrivate(window, viewModel, initialization, null);
		}
		public void OpenNewWindow<T>(BaseWindow window, T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			OpenNewWindowPrivate(window, viewModel, null, windowClosing);
		}
		public void OpenNewWindow<T>(BaseWindow window, T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			OpenNewWindowPrivate(window, viewModel, initialization, windowClosing);
		}

		private void OpenNewWindowPrivate<T>(Window window, T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			var navigator = new ViewNavigator(this, window);
			if (window is BaseWindow baseWindow)
			{
				baseWindow.WindowClosing = () => windowClosing?.Invoke(navigator) ?? true;
			}
			navigator.NavigateTo(viewModel, initialization);

			window.Show();
		}
	}
}