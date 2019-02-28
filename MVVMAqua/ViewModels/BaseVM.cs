using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MVVMAqua.Navigation;
using MVVMAqua.Navigation.Interfaces;
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


		Dictionary<string, ViewNavigator> Regions { get; } = new Dictionary<string, ViewNavigator>();
		
		internal void AddRegion(string regionName, Region region, Bootstrapper bootstrapper)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new ViewNavigator(bootstrapper, region, ViewNavigator.Window));
			}

			Regions[regionName].Container = region;
		}

		internal ViewNavigator GetRegionNavigator(string regionName, Bootstrapper bootstrapper)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new ViewNavigator(bootstrapper, null, ViewNavigator.Window));
			}

			return Regions[regionName];
		}
	}
}