using System;

using MvvmAqua.Enums;
using MvvmAqua.Interfaces;

namespace MvvmAqua.ViewModels
{
	class ModalMessageVM : BaseVM
	{
		const string Path = "pack://application:,,,/MvvmLibrary;component/Images/";

		private string iconPath;
		public string IconPath
		{
			get => iconPath;
			set { iconPath = value; OnPropertyChanged(); }
		}
		
		private bool iconVisible = true;
		public bool IconVisible
		{
			get => iconVisible;
			set { iconVisible = value; OnPropertyChanged(); }
		}

		private string message;
		public string Message
		{
			get => message;
			set { message = value; OnPropertyChanged(); }
		}

		public ModalMessageVM(string message, ModalIcon icon)
		{
			Message = message;

			switch (icon)
			{
				case ModalIcon.None:
					IconVisible = false;
					break;
				case ModalIcon.Error:
					IconPath = $"{Path}Error.png";
					break;
				case ModalIcon.Information:
					IconPath = $"{Path}Information.png";
					break;
				case ModalIcon.Question:
					IconPath = $"{Path}Question.png";
					break;
				default:
					throw new NotImplementedException("Для данной иконки нет реализации.");
			}
		}
	}
}