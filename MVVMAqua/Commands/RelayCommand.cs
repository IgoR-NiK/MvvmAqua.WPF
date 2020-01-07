using System;
using System.Windows.Input;

namespace MVVMAqua.Commands
{
	public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool>? _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public RelayCommand(Action execute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}

		public RelayCommand(Action execute, Func<bool>? canExecute)
			: this(execute)
		{
			_canExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			_execute();
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke() ?? true;
		}
	}
}