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
	internal class ViewNavigator : IViewNavigator
	{
        #region Свойства

        Bootstrapper Bootstrapper { get; }

        ContentControl Container { get; }

        public Window Window { get; }

        public INavigator Parent { get; }

        public BaseVM ViewModel => Views.Count > 0 ? Views.Last().ViewModel : null;

        public bool IsEmpty => Views.Count == 0;

        public int CountViews => Views.Count;

        public RegionsCollection Regions { get; } = new RegionsCollection();

        LinkedList<ViewWrapper> Views { get; } = new LinkedList<ViewWrapper>();

        #endregion

        public ViewNavigator(Bootstrapper bootstrapper, ContentControl container, Window window, INavigator parent)
		{
			Bootstrapper = bootstrapper;
            Container = container;            
            Window = window;
            Parent = parent;
		}

        #region OpenFirstView

        public void OpenFirstView()
        {
            OpenFirstView<BaseVM>(null, null, null, true);
        }

        public void OpenFirstView(bool isCallbackCloseViewHandler)
        {
            OpenFirstView<BaseVM>(null, null, null, isCallbackCloseViewHandler);
        }

        public void OpenFirstView<T>(T viewModel) where T : BaseVM
        {
            OpenFirstView(viewModel, null, null, true);
        }

        public void OpenFirstView<T>(T viewModel, bool isCallbackCloseViewHandler) where T : BaseVM
        {
            OpenFirstView(viewModel, null, null, isCallbackCloseViewHandler);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization) where T : BaseVM
        {
            OpenFirstView(viewModel, initialization, null, true);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, bool isCallbackCloseViewHandler) where T : BaseVM
        {
            OpenFirstView(viewModel, initialization, null, isCallbackCloseViewHandler);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed) where T : BaseVM
        {
            OpenFirstView(viewModel, initialization, vm => { afterViewClosed?.Invoke(vm); return true; }, true);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed, bool isCallbackCloseViewHandler) where T : BaseVM
        {
            OpenFirstView(viewModel, initialization, vm => { afterViewClosed?.Invoke(vm); return true; }, isCallbackCloseViewHandler);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
        {
            OpenFirstView(viewModel, initialization, afterViewClosed, true);
        }

        public void OpenFirstView<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed, bool isCallbackCloseViewHandler) where T : BaseVM
        {
            if (viewModel == null)
            {
                if (!IsEmpty)
                {                    
                    if (isCallbackCloseViewHandler)
                    {
                        var lastViewWrapper = Views.Last();
                        if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
                        {
                            return;
                        }
                    }

                    for (var i = CountViews; i > 1; i--)
                    {
                        Views.RemoveLast();
                    }

                    Container.Content = Views.Last().View;
                    Container.DataContext = Views.Last().ViewModel;
                }
            }
            else
            {
                if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
                {
                    if (!IsEmpty && isCallbackCloseViewHandler)
                    {
                        var lastViewWrapper = Views.Last();
                        if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
                        {
                            return;
                        }
                    }

                    var viewWrapper = new ViewWrapper()
                    {
                        View = Activator.CreateInstance(viewType) as ContentControl,
                        ViewModel = viewModel,
                        AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true
                    };

                    foreach (var region in NavigationHelper.FindLogicalChildren<Region>(viewWrapper.View))
                    {
                        viewWrapper.ViewModel.RegionNavigators.Add(region.Name, new ViewNavigator(Bootstrapper, region, Window, this));
                    }

                    viewModel.ViewNavigator = this;
                    initialization?.Invoke(viewModel);
                    
                    Views.Clear();
                    Views.AddLast(viewWrapper);

                    Container.Content = viewWrapper.View;
                    Container.DataContext = viewWrapper.ViewModel;
                }
            }
        }

        #endregion

        #region NavigateTo

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
            NavigateTo(viewModel, initialization, vm => { afterViewClosed?.Invoke(vm); return true; });
        }

        /// <summary>
        /// Отображает в окне новое представление, соответствующее указанной <paramref name="viewModel"/>.
        /// </summary>
        /// <param name="viewModel">Указывает на представление, которое необходимо отобразить в окне.</param>
        public void NavigateTo<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
		{
            if (Bootstrapper.ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
                var viewWrapper = new ViewWrapper()
                {
                    View = Activator.CreateInstance(viewType) as ContentControl,
                    ViewModel = viewModel,
                    AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true
                };

                foreach (var region in NavigationHelper.FindLogicalChildren<Region>(viewWrapper.View))
                {
                    viewWrapper.ViewModel.RegionNavigators.Add(region.Name, new ViewNavigator(Bootstrapper, region, Window, this));
                }

                viewModel.ViewNavigator = this;
                initialization?.Invoke(viewModel);

				Views.AddLast(viewWrapper);

                Container.Content = viewWrapper.View;
                Container.DataContext = viewWrapper.ViewModel;
			}
		}

        #endregion

        #region CloseLastView

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
            if (!IsEmpty)
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

                if (IsEmpty)
                {
                    Container.Content = null;
                    Container.DataContext = null;
                }
                else
                {
                    Container.Content = Views.Last().View;
                    Container.DataContext = Views.Last().ViewModel;
                }
            }
		}

        #endregion

        #region CloseAllViews

        /// <summary>
        /// Закрывает все представления.
        /// </summary>
        public void CloseAllViews()
		{
            CloseAllViews(true);
		}

        public void CloseAllViews(bool isCallbackCloseViewHandler)
        {
            if (!IsEmpty)
            {
                if (isCallbackCloseViewHandler)
                {
                    var lastViewWrapper = Views.Last();
                    if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
                    {
                        return;
                    }
                }

                Views.Clear();

                Container.Content = null;
                Container.DataContext = null;
            }
        }

        #endregion

        #region CloseWindow

        /// <summary>
        /// Закрывает все представления и выходит из главного окна.
        /// </summary>
        public void CloseWindow()
		{
            CloseWindow(true);
        }

        public void CloseWindow(bool isCallbackCloseWindowHandler)
        {
            if (Window is BaseWindow baseWindow)
            {
                baseWindow.IsCallbackCloseWindowHandler = isCallbackCloseWindowHandler;
            }

            Window.Close();
        }

        #endregion

        #region OpenNewWindow

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
				var modalWindow = new ModalWindow() { Owner = Window };
				var modalVm = new ModalWindowVM<T>(viewModel, initialization, caption, buttonType, btnOkText, btnCancelText, Bootstrapper.ModalWindowColorTheme);
				modalWindow.DataContext = modalVm;

                var modalViewNavigator = new ViewNavigator(Bootstrapper, modalWindow, modalWindow, null);
                foreach (var region in NavigationHelper.FindLogicalChildren<Region>(modalWindow))
				{
					modalVm.RegionNavigators.Add(region.Name, new ViewNavigator(Bootstrapper, region, modalWindow, modalViewNavigator));
				}
                modalVm.ViewNavigator = modalViewNavigator;

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