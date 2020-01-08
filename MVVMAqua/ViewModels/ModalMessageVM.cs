using System;

using MVVMAqua.Enums;

namespace MVVMAqua.ViewModels
{
	internal class ModalMessageVM : BaseVM
	{
		private const string Path = "pack://application:,,,/MVVMAqua;component/Images/";

		private string _iconPath = String.Empty;
		public string IconPath
		{
			get => _iconPath;
			set => SetProperty(ref _iconPath, value, nameof(IconPath));
		}
		
		private bool _iconVisible = true;
		public bool IconVisible
		{
			get => _iconVisible;
			set => SetProperty(ref _iconVisible, value, nameof(IconVisible));
		}

		private string _message = String.Empty;
		public string Message
		{
			get => _message;
			set => SetProperty(ref _message, value, nameof(Message));
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
				case ModalIcon.Warning:
					IconPath = $"{Path}Warning.png";
					break;
				default:
					throw new NotImplementedException("Для данной иконки нет реализации.");
			}
		}
	}
}