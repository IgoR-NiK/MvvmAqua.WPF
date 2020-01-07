using System.Collections.Generic;

using MVVMAqua.Navigation.Interfaces;

namespace MVVMAqua.ViewModels
{
	/// <summary>
	/// Базовый класс для создания модели представления.
	/// </summary>
	public abstract class BaseVM : NotifyObject
	{
        private string _windowTitle;

        /// <summary>
        /// Заголовок окна.
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private IViewNavigator _viewNavigator;
		public IViewNavigator ViewNavigator
		{
			get => _viewNavigator;
			internal set
			{
				_viewNavigator = value;
				ViewNavigatorInitialization();
			}
		}		

		protected virtual void ViewNavigatorInitialization() { }

		internal Dictionary<string, INavigator> RegionNavigators { get; } = new Dictionary<string, INavigator>();
	}
}