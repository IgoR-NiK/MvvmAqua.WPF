using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation
{
	class ViewWrapper
	{
		public ContentControl View { get; set; }

		private BaseVM viewModel;
		public BaseVM ViewModel
		{
			get => viewModel;
			set { viewModel = value; View.DataContext = value; }
		}

		/// <summary>
		/// Представляет действие, выполняющееся после закрытия представления.
		/// Возвращает true, если представление может быть закрыто. В противном случае - false.
		/// </summary>
		public Func<BaseVM, bool> AfterViewClosed { get; set; }
	}
}
