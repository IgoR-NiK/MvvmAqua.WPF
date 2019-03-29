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
		public ValidatableProperty<string> Password { get; } = new ValidatableProperty<string>(default, null, true);

        public RelayCommand CheckCommand { get; }
	
		public MainVM()
		{           

			Password.AddRule(x => x?.Length > 5, "> 5", false);
			Password.AddRule(x => x?.Length > 10, "> 10");
            Password.AddRule(x => !String.IsNullOrWhiteSpace(x), "Введите значение");

            CheckCommand = new RelayCommand(() => Password.Validate());
        }
	}
}