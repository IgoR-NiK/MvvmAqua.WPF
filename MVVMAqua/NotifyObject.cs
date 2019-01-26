using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMAqua
{
	public abstract class NotifyObject : INotifyPropertyChanged
	{
		protected void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				OnPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Action onValueChanged, [CallerMemberName]string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				onValueChanged?.Invoke();
				OnPropertyChanged(propertyName);
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}