using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

using MVVMAqua.Enums;
using MVVMAqua.Windows;
using MVVMAqua.ViewModels;
using MVVMAqua.Navigation.Regions;
using MVVMAqua.Navigation.Interfaces;
using System.Windows.Controls;

namespace MVVMAqua.Navigation
{
	/// <summary>
	/// Менеджер навигации между представлениями.
	/// </summary>
	class ViewNavigator : IViewNavigator
	{
		internal Bootstrapper Bootstrapper { get; }

		/// <summary>
		/// Стек представлений.
		/// </summary>
		LinkedList<ViewWrapper> Views { get; } = new LinkedList<ViewWrapper>();
               
        private ContentControl container;
        internal ContentControl Container
        {
            get => container;
            set
            {
                container = value;
                InitializationСontainer();
            }
        }

        private void InitializationСontainer()
        {
            if (Views.Count != 0)
            {
                Container.Content = Views.Last().View;
                Container.DataContext = Views.Last().ViewModel;
            }
        }

        /// <summary>
        /// Окно для отображения представлений.
        /// </summary>
        public Window Window { get; }

		public RegionsCollection Regions { get; }

        public INavigator Parent => throw new NotImplementedException();

        public BaseVM ViewModel => throw new NotImplementedException();

        public ViewNavigator(Bootstrapper bootstrapper, ContentControl container, Window window)
		{
			Bootstrapper = bootstrapper;
            Container = container;            
            Window = window;

			Regions = new RegionsCollection(Bootstrapper);
		}


        public void OpenFirstView()
        {
            throw new NotImplementedException();
        }

        public void OpenFirstView<T>(T viewModel) where T : BaseVM
        {
            throw new NotImplementedException();
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization) where T : BaseVM
        {
            throw new NotImplementedException();
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed) where T : BaseVM
        {
            throw new NotImplementedException();
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
        {
            throw new NotImplementedException();
        }

        /*
        public void UpdateRegion<T>(T viewModel) where T : BaseVM
        {
            UpdateRegion(viewModel, null, null);
        }
        public void UpdateRegion<T>(T viewModel, Action<T> initialization) where T : BaseVM
        {
            UpdateRegion(viewModel, initialization, null);
        }
        public void UpdateRegion<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
        {
            if (ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
            {
                initialization?.Invoke(viewModel);
                viewModel.ViewNavigator = ViewNavigator;
                var viewWrapper = new ViewWrapper() { AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true };

                viewWrapper.View = Activator.CreateInstance(viewType) as ContentControl;
                viewWrapper.ViewModel = viewModel;

                foreach (var region in NavigationHelper.FindLogicalChildren<Region>(viewWrapper.View))
                {
                    viewWrapper.ViewModel.AddRegion(region.Name, region);
                }

                viewWrappers.Clear();
                viewWrappers.Push(viewWrapper);

                if (Region != null)
                {
                    Region.Content = viewWrapper.View;
                    Region.DataContext = viewWrapper.ViewModel;
                }
            }
        }
*/










        public void NavigateTo<T>(T viewModel) where T : BaseVM
		{
			NavigateTo(viewModel, null, null);
		}
		public void NavigateTo<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			NavigateTo(viewModel, initialization, null);
		}

        public void NavigateTo<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed) where T : BaseVM
        {
            throw new NotImplementedException();
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
					viewWrapper.ViewModel.AddRegion(region.Name, region, Bootstrapper);
				}

				Views.AddLast(viewWrapper);

                if (Container != null)
                {
                    Container.Content = viewWrapper.View;
                    Container.DataContext = viewWrapper.ViewModel;
                }
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
			var lastViewWrapper = Views.Last();
			if (isCallbackCloseViewHandler)
			{
				if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
				{
					return;
				}
			}

			Views.Remove(lastViewWrapper);
			if (Views.Count == 0)
			{
                if (Container != null)
                {
                    Container.Content = null;
                    Container.DataContext = null;
                }
			}
			else
			{
                if (Container != null)
                {
                    Container.Content = Views.Last().View;
                    Container.DataContext = Views.Last().ViewModel;
                }
			}
		}

		/// <summary>
		/// Закрывает все представления.
		/// </summary>
		public void CloseAllViews()
		{
			Views.Clear();

            if (Container != null)
            {
                Container.Content = null;
                Container.DataContext = null;
            }
		}

		/// <summary>
		/// Закрывает все представления и выходит из главного окна.
		/// </summary>
		public void CloseWindow()
		{
			Window.Close();
		}


        #region Открытие нового окна

        public void OpenNewWindow<T>(T viewModel)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization)
           where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Action<T> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Action<T> windowClosing)
            where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<T>(T viewModel, Action<T> viewModelInitialization, Func<T, bool> afterViewClosed, Action<Window> windowInitialization, Func<T, bool> windowClosing)
            where T : BaseVM
        {
            Bootstrapper.OpenNewWindow(viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            Bootstrapper.OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Action<ViewModelType> afterViewClosed, Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            Bootstrapper.OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed, Action<WindowType> windowInitialization, Action<ViewModelType> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            Bootstrapper.OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        public void OpenNewWindow<ViewModelType, WindowType>(WindowType window, ViewModelType viewModel, Action<ViewModelType> viewModelInitialization, Func<ViewModelType, bool> afterViewClosed, Action<WindowType> windowInitialization, Func<ViewModelType, bool> windowClosing)
            where ViewModelType : BaseVM
            where WindowType : Window
        {
            Bootstrapper.OpenNewWindow(window, viewModel, viewModelInitialization, afterViewClosed, windowInitialization, windowClosing);
        }

        #endregion

        #region ShowModalWindow

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

				var navigator = new ViewNavigator(Bootstrapper, modalWindow, modalWindow);
				modalVm.ViewNavigator = navigator;

				foreach (var region in NavigationHelper.FindLogicalChildren<Region>(modalWindow))
				{
					modalVm.AddRegion(region.Name, region, Bootstrapper);
				}

				result = modalWindow.ShowDialog() ?? false;

				if (result)
					okResult?.Invoke(viewModel);
				else
					cancelResult?.Invoke(viewModel);
			}

			return result;
		}

        #endregion
    }
}