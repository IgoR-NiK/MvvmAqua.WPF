using System;

using MVVMAqua.Enums;

namespace MVVMAqua.ViewModels
{
	class ModalMessageVM : BaseVM
	{
		const string Path = "pack://application:,,,/MVVMAqua;component/Images/";

		private string iconPath;
		public string IconPath
		{
			get => iconPath;
			set => SetProperty(ref iconPath, value);
		}
		
		private bool iconVisible = true;
		public bool IconVisible
		{
			get => iconVisible;
			set => SetProperty(ref iconVisible, value);
		}

		private string message;
		public string Message
		{
			get => message;
			set => SetProperty(ref message, value);
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