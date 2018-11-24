using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation.Regions
{
	public sealed class RegionWrapper
	{
		internal Dictionary<Type, Type> ViewModelToViewMap { get; set; }

		private Region region;
		internal Region Region
		{
			get => region;
			set
			{
				region = value;
				Initialization();
			}
		}

		private readonly Stack<ViewWrapper> viewWrappers = new Stack<ViewWrapper>();

		private IViewNavigator ViewNavigator { get; }
		
		private void Initialization()
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

		public void NavigateTo<T>(T viewModel) where T : BaseVM
		{
			NavigateTo(viewModel, null, null);
		}
		public void NavigateTo<T>(T viewModel, Action<T> initialization) where T : BaseVM
		{
			NavigateTo(viewModel, initialization, null);
		}
		public void NavigateTo<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM
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

				viewWrappers.Push(viewWrapper);

				if (Region != null)
				{
					Region.Content = viewWrapper.View;
					Region.DataContext = viewWrapper.ViewModel;
				}
			}
		}

		public void CloseLastView()
		{
			CloseLastView(true);
		}
		public void CloseLastView(bool isCallbackCloseViewHandler)
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
			viewWrappers.Clear();

			if (Region != null)
			{
				Region.Content = null;
				Region.DataContext = null;
			}
		}
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
	}
}