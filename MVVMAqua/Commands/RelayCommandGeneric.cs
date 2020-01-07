using System;
using System.Windows.Input;

namespace MVVMAqua.Commands
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool>? _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public RelayCommand(Action<T> execute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}

		public RelayCommand(Action<T> execute, Func<T, bool>? canExecute) 
			: this(execute)
		{
			_canExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			_execute((T)parameter);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke((T)parameter) ?? true;
		}
	}
}