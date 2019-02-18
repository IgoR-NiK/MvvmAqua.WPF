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
	public abstract class BaseVM : NotifyObject
	{
		private IViewNavigator viewNavigator;
		public IViewNavigator ViewNavigator
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


		Dictionary<string, RegionNavigator> Regions { get; } = new Dictionary<string, RegionNavigator>();
		
		internal void AddRegion(string regionName, Region region)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new RegionNavigator(ViewNavigator));
			}

			Regions[regionName].Region = region;
		}

		internal RegionNavigator GetRegionNavigator(string regionName)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new RegionNavigator(ViewNavigator));
			}

			return Regions[regionName];
		}
	}
}