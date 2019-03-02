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
		private string windowTitle;

		/// <summary>
		/// Заголовок окна.
		/// </summary>
		public string WindowTitle
		{
			get => windowTitle;
			set => SetProperty(ref windowTitle, value, "WindowTitle");
		}
        
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

		protected virtual void ViewNavigatorInitialization() { }

		internal Dictionary<string, INavigator> RegionNavigators { get; } = new Dictionary<string, INavigator>();
	}
}