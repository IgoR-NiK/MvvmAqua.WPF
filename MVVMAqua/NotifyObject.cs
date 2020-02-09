using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MVVMAqua.Arguments;

namespace MVVMAqua
{
	public abstract class NotifyObject : INotifyPropertyChanged
	{
		protected void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				NotifyPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Action<ValueChangedArgs<T>> onValueChanged, [CallerMemberName]string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				onValueChanged(new ValueChangedArgs<T>(property, value));
				property = value;
				NotifyPropertyChanged(propertyName);
			}
		}


		public event PropertyChangedEventHandler? PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}