using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

using MVVMAqua.Enums;
using MVVMAqua.Windows;
using MVVMAqua.ViewModels;
using MVVMAqua.Navigation.Regions;
using System.Windows.Controls;

namespace MVVMAqua.Navigation
{
	/// <summary>
	/// Менеджер навигации между представлениями.
	/// </summary>
	class ViewNavigator : IViewNavigator
	{
		Bootstrapper Bootstrapper { get; }

		/// <summary>
		/// Стек представлений.
		/// </summary>
		Stack<ViewWrapper> Views { get; } = new Stack<ViewWrapper>();
			   
		/// <summary>
		/// Окно для отображения представлений.
		/// </summary>
		Window Window { get; }
		
		public RegionsCollection Regions { get; }
		
		public ViewNavigator(Bootstrapper bootstrapper, Window window)
		{
			Bootstrapper = bootstrapper;
			Window = window;

			Regions = new RegionsCollection(Bootstrapper.ViewModelToViewMap);
		}

		/// <summary>
		/// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
		public void NavigateTo<T>(T viewModel, Action<T> initialization = null, Func<T, bool> afterViewClosed = null) where T : BaseVM
		{
			if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				viewModel.ViewNavigator = this;
				viewModel.ViewNavigatorInitialization();
				var viewWrapper = new ViewWrapper() { AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true };

				viewWrapper.View = Activator.CreateInstance(viewType) as ContentControl;
				viewWrapper.ViewModel = viewModel;

				foreach (var region in NavigationHelper.FindLogicalChildren<Region>(viewWrapper.View))
				{
					viewWrapper.ViewModel.AddRegion(region.Name, region);
				}

				Views.Push(viewWrapper);

				Window.Content = viewWrapper.View;
				Window.DataContext = viewWrapper.ViewModel;
			}
		}

		/// <summary>
		/// Закрывает последнее представление и выполняет действие закрытия представления при необходимости.
		/// </summary>
		/// <param name="isCallbackCloseViewHandler">Флаг, указывающий нужно ли выполнять действие закрытия представления.</param>
		public void CloseLastView(bool isCallbackCloseViewHandler = true)
		{
			var lastViewWrapper = Views.Pop();
			if (isCallbackCloseViewHandler)
			{
				if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
				{
					Views.Push(lastViewWrapper);
					return;
				}
			}

			if (Views.Count == 0)
			{
				CloseAllViews();
				return;
			}

			Window.Content = Views.Peek().View;
			Window.DataContext = Views.Peek().ViewModel;
		}

		/// <summary>
		/// Закрывает все представления и выходит из главного окна.
		/// </summary>
		public void CloseAllViews()
		{
			Window.Close();
		}


		public void OpenNewWindow<T>(T viewModel) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(viewModel);
		}

		public void OpenNewWindow<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(viewModel, initialization);
		}

		public void OpenNewWindow<T>(T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(viewModel, windowClosing);
		}

		public void OpenNewWindow<T>(T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(viewModel, initialization, windowClosing);
		}

		public void OpenNewWindow<T>(Window window, T viewModel) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(window, viewModel);
		}

		public void OpenNewWindow<T>(Window window, T viewModel, Action<T> initialization) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(window, viewModel, initialization);
		}

		public void OpenNewWindow<T>(BaseWindow window, T viewModel, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(window, viewModel, windowClosing);
		}

		public void OpenNewWindow<T>(BaseWindow window, T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(window, viewModel, initialization, windowClosing);
		}

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным текстом.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		public bool ShowModalWindow(string text, string caption = "", ModalButtons buttonType = ModalButtons.Ok, string btnOkText = "Ок", string btnCancelText = "Отмена", Action okResult = null, Action cancelResult = null, ModalIcon icon = ModalIcon.None)
		{
			var viewModel = new ModalMessageVM(text, icon);
			return ShowModalWindow(viewModel, caption, buttonType, btnOkText, btnCancelText, _ => okResult?.Invoke(), _ => cancelResult?.Invoke());
		}

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным представлением.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		public bool ShowModalWindow<T>(T viewModel, string caption = "", ModalButtons buttonType = ModalButtons.Ok, string btnOkText = "Ок", string btnCancelText = "Отмена", Action<T> okResult = null, Action<T> cancelResult = null, Action<T> initialization = null) where T : BaseVM
		{
			var result = false;

			if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				var view = Activator.CreateInstance(viewType) as ContentControl;
				view.DataContext = viewModel;

				var modalWindow = new ModalWindow() { Owner = Window };
				var x = new ModalWindowVM(viewModel, caption, buttonType, btnOkText, btnCancelText, Bootstrapper.ModalWindowColorTheme);
				modalWindow.DataContext = x;

				var navigator = new ViewNavigator(Bootstrapper, modalWindow);
				x.ViewNavigator = navigator;
				x.ViewNavigatorInitialization();

				foreach (var region in NavigationHelper.FindLogicalChildren<Region>(modalWindow))
				{
					x.AddRegion(region.Name, region);
				}

				result = modalWindow.ShowDialog() ?? false;

				if (result)
					okResult?.Invoke(viewModel);
				else
					cancelResult?.Invoke(viewModel);
			}

			return result;
		}
	}
}