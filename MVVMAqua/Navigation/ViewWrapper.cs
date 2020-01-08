using System;
using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation
{
	internal class ViewWrapper
	{
		public ContentControl View { get; }

		public BaseVM ViewModel { get; }

		public ViewWrapper(ContentControl view, BaseVM viewModel)
		{
			View = view;
			ViewModel = viewModel;
			
			View.DataContext = viewModel;
		}

		/// <summary>
		/// Представляет действие, выполняющееся после закрытия представления.
		/// Возвращает true, если представление может быть закрыто. В противном случае - false.
		/// </summary>
		public Func<BaseVM, bool>? AfterViewClosed { get; set; }
	}
}