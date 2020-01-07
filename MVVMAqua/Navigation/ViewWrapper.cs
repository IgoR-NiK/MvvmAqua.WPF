using System;
using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation
{
	internal class ViewWrapper
	{
		public ContentControl View { get; set; }

		private BaseVM _viewModel;
		public BaseVM ViewModel
		{
			get => _viewModel;
			set
			{
				_viewModel = value; 
				View.DataContext = value;
			}
		}

		/// <summary>
		/// Представляет действие, выполняющееся после закрытия представления.
		/// Возвращает true, если представление может быть закрыто. В противном случае - false.
		/// </summary>
		public Func<BaseVM, bool> AfterViewClosed { get; set; }
	}
}