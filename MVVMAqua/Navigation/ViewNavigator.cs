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

		public void NavigateTo<T>(T viewModel) where T : BaseVM
		{
			NavigateTo(viewModel, null, null);
		}
		public void NavigateTo<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			NavigateTo(viewModel, initialization, null);
		}

		/// <summary>
		/// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
		public void NavigateTo<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
		{
			if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				viewModel.ViewNavigator = this;
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

		public void CloseLastView()
		{
			CloseLastView(true);
		}

		/// <summary>
		/// Закрывает последнее представление и выполняет действие закрытия представления при необходимости.
		/// </summary>
		/// <param name="isCallbackCloseViewHandler">Флаг, указывающий нужно ли выполнять действие закрытия представления.</param>
		public void CloseLastView(bool isCallbackCloseViewHandler)
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
		/// Закрывает все представления.
		/// </summary>
		public void CloseAllViews()
		{
			Views.Clear();

			Window.Content = null;
			Window.DataContext = null;
		}

		/// <summary>
		/// Закрывает все представления и выходит из главного окна.
		/// </summary>
		public void CloseWindow()
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

		public void OpenNewWindow<T>(BaseWindow window, T viewModel, Action<T> initialization, Func<IViewNavigator, bool> windowClosing) where T : BaseVM
		{
			Bootstrapper.OpenNewWindow(window, viewModel, initialization, windowClosing);
		}

		public bool ShowModalWindow(string text)
		{
			return ShowModalWindow(text, ModalIcon.None, "Уведомление", ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon)
		{
			return ShowModalWindow(text, icon, "Уведомление", ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, ModalButtons buttonType)
		{
			return ShowModalWindow(text, icon, caption, buttonType, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.Ok, btnOkText, "Отмена", null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText, Action okResult)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.Ok, btnOkText, "Отмена", okResult, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText, string btnCancelText)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, null, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText,	string btnCancelText, Action okResult)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, null);
		}
		public bool ShowModalWindow(string text, ModalIcon icon, string caption, string btnOkText, string btnCancelText, Action okResult, Action cancelResult)
		{
			return ShowModalWindow(text, icon, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, cancelResult);
		}

		public bool ShowModalWindow<T>(T viewModel) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, "Уведомление", ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, ModalButtons buttonType) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, buttonType, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.Ok, btnOkText, "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, Action<T> okResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.Ok, btnOkText, "Отмена", okResult, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, null);
		}
		public bool ShowModalWindow<T>(T viewModel, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, null, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, cancelResult);
		}

		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, "Уведомление", ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.Ok, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, ModalButtons buttonType) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, buttonType, "Ок", "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.Ok, btnOkText, "Отмена", null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, Action<T> okResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.Ok, btnOkText, "Отмена", okResult, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, null, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, null);
		}
		public bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM
		{
			return ShowModalWindow(viewModel, initialization, caption, ModalButtons.OkCancel, btnOkText, btnCancelText, okResult, cancelResult);
		}

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным текстом.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		private bool ShowModalWindow(string text, ModalIcon icon, string caption, ModalButtons buttonType,
								string btnOkText, string btnCancelText, Action okResult, Action cancelResult)
		{
			var viewModel = new ModalMessageVM(text, icon);
			return ShowModalWindow(viewModel, null, caption, buttonType, btnOkText, btnCancelText, _ => okResult?.Invoke(), _ => cancelResult?.Invoke());
		}

		/// <summary>
		/// Отображает модальное диалоговое окно с указанным представлением.
		/// </summary>
		/// <param name="viewModel">Указывает на представление, которое необходимо отобразить в модальном окне.</param>
		private bool ShowModalWindow<T>(T viewModel, Action<T> initialization, string caption, ModalButtons buttonType,
								string btnOkText, string btnCancelText, Action<T> okResult, Action<T> cancelResult) where T : BaseVM
		{
			var result = false;

			if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				var view = Activator.CreateInstance(viewType) as ContentControl;
				view.DataContext = viewModel;

				var modalWindow = new ModalWindow() { Owner = Window };
				var modalVm = new ModalWindowVM(viewModel, caption, buttonType, btnOkText, btnCancelText, Bootstrapper.ModalWindowColorTheme);
				modalWindow.DataContext = modalVm;

				var navigator = new ViewNavigator(Bootstrapper, modalWindow);
				modalVm.ViewNavigator = navigator;

				foreach (var region in NavigationHelper.FindLogicalChildren<Region>(modalWindow))
				{
					modalVm.AddRegion(region.Name, region);
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