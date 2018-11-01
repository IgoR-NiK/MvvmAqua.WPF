using System;
using System.Windows.Input;

namespace MVVMAqua.Commands
{
	public class RelayCommand<T> : ICommand
	{
		readonly Action<T> execute;
		readonly Func<T, bool> canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<T> execute)
		{
			this.execute = execute ?? throw new ArgumentNullException("execute", "Необходимо указать действие команды");
		}

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute) : this(execute)
		{
			this.canExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			execute((T)parameter);
		}

		public bool CanExecute(object parameter)
		{
			return canExecute?.Invoke((T)parameter) ?? true;
		}
	}
}