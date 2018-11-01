using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

using MVVMAqua.Enums;
using MVVMAqua.Windows;
using MVVMAqua.Interfaces;
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
		IBootstrapper Bootstrapper { get; }
		Dictionary<Type, Type> ViewModelToViewMap { get; }

		/// <summary>
		/// Стек представлений.
		/// </summary>
		Stack<ViewWrapper> Views { get; } = new Stack<ViewWrapper>();
			   
		/// <summary>
		/// Окно для отображения представлений.
		/// </summary>
		BaseWindow Window { get;  }
		
		public RegionsCollection Regions { get; }
		
		public ViewNavigator(IBootstrapper bootstrapper, BaseWindow window, Dictionary<Type, Type> viewModelToViewMap)
		{
			Bootstrapper = bootstrapper;
			Window = window;
			ViewModelToViewMap = viewModelToViewMap;

			Regions = new RegionsCollection(ViewModelToViewMap);
		}

		/// <summary>
		/// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
		public void NavigateTo<T>(T viewModel, Action<T> initialization = null, Func<T, bool> afterViewClosed = null) where T : BaseVM
		{
			if (ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
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

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным текстом.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		public bool ShowModalWindow(string text, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action okResult = null, Action cancelResult = null, ModalIcon icon = ModalIcon.None)
		{
			return Bootstrapper.ShowModalWindow(Window, text, caption, buttonType, btnOkText, btnCancelText, okResult, cancelResult, icon);
		}

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным представлением.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		public bool ShowModalWindow<T>(T viewModel, string caption = "", ModalButtons buttonType = ModalButtons.Ok,
									string btnOkText = "Ок", string btnCancelText = "Отмена",
									Action<T> okResult = null, Action<T> cancelResult = null, Action<T> initialization = null) where T : BaseVM
		{
			return Bootstrapper.ShowModalWindow(Window, viewModel, caption, buttonType, btnOkText, btnCancelText, okResult, cancelResult, initialization);
		}

		public void OpenNewWindow<T>(T viewModel, Action<T> initialization = null, Func<bool> windowClosing = null) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(viewModel, initialization, Window => windowClosing?.Invoke() ?? true);
		}

		public void OpenNewWindow<TViewModel, TWindow>(TViewModel viewModel, Action<TViewModel> initialization = null, Func<bool> windowClosing = null)
			where TViewModel : BaseVM
			where TWindow : BaseWindow, new()
		{
			Bootstrapper.OpenNewWindow<TViewModel, TWindow>(viewModel, initialization, Window => windowClosing?.Invoke() ?? true);
		}
	}
}