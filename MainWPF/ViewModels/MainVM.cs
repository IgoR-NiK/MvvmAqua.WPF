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
		private bool isCheck;
		public bool IsCheck
		{
			get => isCheck;
			set => SetProperty(ref isCheck, value, () => Password.Validate());
		}

		public ValidatableProperty<string> Password { get; } = new ValidatableProperty<string>();

        public RelayCommand CheckCommand { get; }
	
		public MainVM()
		{           
            Password.AddRule(x => !IsCheck || x?.Length > 5, "> 5");

			Password.AddRule(x => x?.Length > 5, "> 5");

			Password.AddRule(x => x?.Length > 10, "> 10");
            Password.AddRule(x => !String.IsNullOrWhiteSpace(x), "Введите значение");

            CheckCommand = new RelayCommand(() => Password.Validate());
        }
	}
}