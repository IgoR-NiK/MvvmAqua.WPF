using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MvvmAqua.Interfaces;
using MvvmAqua.Navigation.Regions;

namespace MvvmAqua.ViewModels
{
	/// <summary>
	/// Базовый класс для создания модели представления.
	/// </summary>
	public abstract class BaseVM : INotifyPropertyChanged
	{
		protected internal IViewNavigator ViewNavigator { get; internal set; }

		private string windowTitle;

		/// <summary>
		/// Заголовок окна.
		/// </summary>
		public string WindowTitle
		{
			get => windowTitle;
			set { windowTitle = value; OnPropertyChanged(); }
		}

		protected internal virtual void ViewNavigatorInitialization() { }


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		Dictionary<string, RegionWrapper> Regions { get; } = new Dictionary<string, RegionWrapper>();
		
		internal void AddRegion(string regionName, Region region)
		{
			if (!Regions.ContainsKey(regionName))
			{
				Regions.Add(regionName, new RegionWrapper(ViewNavigator));
			}

			Regions[regionName].Region = region;
			Regions[regionName].Initialization();
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