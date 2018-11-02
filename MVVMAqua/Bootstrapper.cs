using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using MVVMAqua.Interfaces;
using MVVMAqua.Messaging;
using MVVMAqua.ViewModels;
using MVVMAqua.Views;
using MVVMAqua.Windows;
using MVVMAqua.Navigation;

namespace MVVMAqua
{
	public class Bootstrapper : IBootstrapper
	{
		Dictionary<BaseWindow, ViewNavigator> Windows { get; } = new Dictionary<BaseWindow, ViewNavigator>();
		
		/// <summary>
		/// Установка соответствия ViewModel к View.
		/// </summary>
		Dictionary<Type, Type> ViewModelToViewMap { get; } = new Dictionary<Type, Type>()
		{
			[typeof(ModalMessageVM)] = typeof(ModalMessageView)
		};

		internal IMessenger Messenger { get; } = new Messenger();
		
		public Bootstrapper()
		{
			/// <summary>
			/// Автоматическая привязка VM к View.
			/// VM должна иметь следующие названия: Name, NameVM или NameViewModel.
			/// View должна иметь следующие названия: Name или NameView.
			/// Регистр значения не имеет.
			/// </summary>
			var callingAssembly = Assembly.GetCallingAssembly();

			var viewModels = callingAssembly.GetTypes().Where(x => typeof(BaseVM).IsAssignableFrom(x)).ToList();
			var views = callingAssembly.GetTypes().Where(x => typeof(ContentControl).IsAssignableFrom(x)).ToList();	// Здесь отбрасывать без пустого конструктора

			viewModels.ForEach(x =>
			{
				var viewModelName = x.Name.ToLower();
				if (viewModelName.EndsWith("vm"))
				{
					viewModelName = viewModelName.Remove(viewModelName.Length - "vm".Length);
				}
				else if (viewModelName.EndsWith("viewmodel"))
				{
					viewModelName = viewModelName.Remove(viewModelName.Length - "viewmodel".Length);
				}

				var view = views.FirstOrDefault(v => v.Name.ToLower() == viewModelName || v.Name.ToLower() == $"{viewModelName}view");

				if (view != null)
				{
					ViewModelToViewMap[x] = view;
				}
			});
		}

		private Type baseWindowType = typeof(MainWindow);

		public void SetTypeWindow<TWindow>() where TWindow : BaseWindow, new()
		{
			baseWindowType = typeof(TWindow);
		}
		
		private Type tempVM;

		public Bootstrapper Bind<T>() where T : BaseVM
		{
			tempVM = typeof(T);
			return this;
		}

		public void To<T>() where T : ContentControl, new()
		{
			ViewModelToViewMap.Add(tempVM, typeof(T));
			tempVM = null;
		}
		
		public void OpenNewWindow<T>(T viewModel, Action<T> initialization = null, Func<IViewNavigator, bool> windowClosing = null) where T : BaseVM
		{
			var window = Activator.CreateInstance(baseWindowType) as BaseWindow;
			window.Closed += (sender, e) => Windows.Remove(window);

			var navigator = new ViewNavigator(this, window, ViewModelToViewMap);
			window.WindowClosing = () => windowClosing?.Invoke(navigator) ?? true;
			viewModel.ViewNavigator = navigator;
			navigator.NavigateTo(viewModel, initialization);

			Windows.Add(window, navigator);
			window.Show();		
		}

		public void OpenNewWindow<TViewModel, TWindow>(TViewModel viewModel, Action<TViewModel> initialization = null, Func<IViewNavigator, bool> windowClosing = null)
			where TViewModel : BaseVM
			where TWindow : BaseWindow, new()
		{
			var typeWindow = typeof(TWindow);
			var window = Activator.CreateInstance(typeWindow) as TWindow;
			window.Closed += (sender, e) => Windows.Remove(window);

			var navigator = new ViewNavigator(this, window, ViewModelToViewMap);
			window.WindowClosing = () => windowClosing?.Invoke(navigator) ?? true;
			viewModel.ViewNavigator = navigator;
			navigator.NavigateTo(viewModel, initialization);

			Windows.Add(window, navigator);
			window.Show();
		}
	}
}