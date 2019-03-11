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

		public ICommand Navigate { get; }

		public ICommand Close { get; }

		public MainVM()
		{
			Title.AddValidationRule(value => value.Contains('f'));
			Password.AddValidationRule(value => value.Length > 8);

            Next = new RelayCommand(() => ViewNavigator.OpenNewWindow(
                new MainVM(), 
                null, 
                null, 
                null,
                vm => vm.ViewNavigator.ShowDialog(new MainVM())));
            Navigate = new RelayCommand(() => ViewNavigator.NavigateTo(new MainVM(), vm => vm.Title.Value = "2", vm =>
            {
                vm.ViewNavigator.NavigateTo(new MainVM(), vm2 => vm2.Title.Value = "3");
                vm.ViewNavigator.NavigateTo(new MainVM(), vm2 => vm2.Title.Value = "4");
                return false;
            }));

			Close = new RelayCommand(() => ViewNavigator.CloseLastView());
		}
	}
}