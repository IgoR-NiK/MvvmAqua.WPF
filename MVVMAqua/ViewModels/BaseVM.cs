using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MVVMAqua.Interfaces;
using MVVMAqua.Navigation.Regions;

namespace MVVMAqua.ViewModels
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
			set => SetProperty(ref windowTitle, value);
		}

		protected internal virtual void ViewNavigatorInitialization() { }
		

		protected void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				OnPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Action onChanged, [CallerMemberName]string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				onChanged?.Invoke();
				OnPropertyChanged(propertyName);
			}
		}


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