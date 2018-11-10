using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MVVMAqua.Navigation;
using MVVMAqua.Navigation.Regions;

namespace MVVMAqua.ViewModels
{
	/// <summary>
	/// Базовый класс для создания модели представления.
	/// </summary>
	public abstract class BaseVM : BindableObject
	{
		private IViewNavigator viewNavigator;
		protected internal IViewNavigator ViewNavigator
		{
			get => viewNavigator;
			internal set
			{
				viewNavigator = value;
				ViewNavigatorInitialization();
			}
		}

		private string windowTitle;

		/// <summary>
		/// Заголовок окна.
		/// </summary>
		public string WindowTitle
		{
			get => windowTitle;
			set => SetProperty(ref windowTitle, value);
		}

		protected virtual void ViewNavigatorInitialization() { }


		Dictionary<string, RegionWrapper> Regions { get; } = new Dictionary<string, RegionWrapper>();
		
		internal void AddRegion(string regionName, Region region)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new RegionWrapper(ViewNavigator));
			}

			Regions[regionName].Region = region;
		}

		internal RegionWrapper GetRegionWrapper(string regionName)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new RegionWrapper(ViewNavigator));
			}

			return Regions[regionName];
		}
	}
}