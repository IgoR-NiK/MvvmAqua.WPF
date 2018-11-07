using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MVVMAqua.Commands;
using MVVMAqua.Validation;
using MVVMAqua.ViewModels;

namespace MainWPF.ViewModels
{
	class MainVM : BaseVM
	{
		public ValidatableProperty<string> Title { get; } = new ValidatableProperty<string>(true);
		public ValidatableProperty<string> Password { get; } = new ValidatableProperty<string>(true);
		
		public ICommand Next { get; }

		public MainVM()
		{
			Title.AddValidationRule(value => value.Contains('f'));
			Password.AddValidationRule(value => value.Length > 8);
			
			Next = new RelayCommand(() => ViewNavigator.OpenNewWindow(new MainVM(), navigator => navigator.ShowModalWindow("Привет")));
		}
	}
}