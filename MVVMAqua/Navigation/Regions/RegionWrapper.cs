using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using MVVMAqua.Interfaces;
using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation.Regions
{
	public class RegionWrapper
	{
		internal Dictionary<Type, Type> ViewModelToViewMap { get; set; }
		internal Region Region { get; set; }
		private readonly Stack<ViewWrapper> viewWrappers = new Stack<ViewWrapper>();

		private IViewNavigator ViewNavigator { get; }
		
		internal void Initialization()
		{
			if (viewWrappers.Count != 0)
			{
				Region.Content = viewWrappers.Peek().View;
				Region.DataContext = viewWrappers.Peek().ViewModel;
			}
		}

		internal RegionWrapper(IViewNavigator viewNavigator)
		{
			ViewNavigator = viewNavigator;
		}


		public void NavigateTo<T>(T viewModel, Action<T> initialization = null, Func<T, bool> afterViewClosed = null) where T : BaseVM
		{
			if (ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				viewModel.ViewNavigator = ViewNavigator;
				viewModel.ViewNavigatorInitialization();
				var viewWrapper = new ViewWrapper() { AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true };

				viewWrapper.View = Activator.CreateInstance(viewType) as ContentControl;
				viewWrapper.ViewModel = viewModel;

				viewWrappers.Push(viewWrapper);

				if (Region != null)
				{
					Region.Content = viewWrapper.View;
					Region.DataContext = viewWrapper.ViewModel;
				}
			}
		}

		public void CloseLastView(bool isCallbackCloseViewHandler = true)
		{
			var lastViewWrapper = viewWrappers.Pop();
			if (isCallbackCloseViewHandler)
			{
				if (!lastViewWrapper.AfterViewClosed?.Invoke(lastViewWrapper.ViewModel) ?? false)
				{
					viewWrappers.Push(lastViewWrapper);
					return;
				}
			}

			if (viewWrappers.Count == 0)
			{
				CloseAllViews();
				return;
			}

			if (Region != null)
			{
				Region.Content = viewWrappers.Peek().View;
				Region.DataContext = viewWrappers.Peek().ViewModel;
			}
		}

		public void CloseAllViews()
		{
			if (Region != null)
			{
				Region.Content = null;
				Region.DataContext = null;
			}
		}

		public void UpdateRegion<T>(T viewModel, Action<T> initialization = null, Func<T, bool> afterViewClosed = null) where T : BaseVM
		{
			if (ViewModelToViewMap.TryGetValue(viewModel.GetType(), out Type viewType))
			{
				initialization?.Invoke(viewModel);
				viewModel.ViewNavigator = ViewNavigator;
				viewModel.ViewNavigatorInitialization();
				var viewWrapper = new ViewWrapper() { AfterViewClosed = vm => afterViewClosed?.Invoke((T)vm) ?? true };

				viewWrapper.View = Activator.CreateInstance(viewType) as ContentControl;
				viewWrapper.ViewModel = viewModel;

				viewWrappers.Clear();
				viewWrappers.Push(viewWrapper);

				if (Region != null)
				{
					Region.Content = viewWrapper.View;
					Region.DataContext = viewWrapper.ViewModel;
				}
			}
		}
	}
}