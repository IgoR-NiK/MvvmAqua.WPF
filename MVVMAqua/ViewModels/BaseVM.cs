using System.Collections.Generic;

using MVVMAqua.Navigation.Interfaces;

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
            set => SetProperty(ref windowTitle, value);
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