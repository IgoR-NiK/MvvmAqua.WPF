using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMAqua
{
	public abstract class NotifyObject : INotifyPropertyChanged
	{
		protected void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				OnPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Action onValueChanged, [CallerMemberName]string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				onValueChanged?.Invoke();
				OnPropertyChanged(propertyName);
			}
		}


		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}