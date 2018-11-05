using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MVVMAqua.Commands;
using MVVMAqua.ViewModels;

namespace MainWPF.ViewModels
{
	class MainVM : BaseVM
	{
		private string title;
		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}

		public ICommand Next { get; }

		public MainVM()
		{
			Next = new RelayCommand(() => ViewNavigator.OpenNewWindow(new MainVM(), navigator => navigator.ShowModalWindow("Привет")));
		}
	}
}