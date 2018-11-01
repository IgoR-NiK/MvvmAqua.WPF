using System;
using System.Windows.Input;

namespace MvvmAqua.Commands
{
	public class RelayCommand : ICommand
	{
		readonly Action execute;
		readonly Func<bool> canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action execute)
		{
			this.execute = execute ?? throw new ArgumentNullException("execute", "Необходимо указать действие команды");
		}

		public RelayCommand(Action execute, Func<bool> canExecute) : this(execute)
		{
			this.canExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			execute();
		}

		public bool CanExecute(object parameter)
		{
			return canExecute?.Invoke() ?? true;
		}
	}
}