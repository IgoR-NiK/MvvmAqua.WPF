using MVVMAqua.Enums;
using System.Windows.Media;

namespace MVVMAqua.ViewModels
{
	class ModalWindowVM : BaseVM
	{
		private bool btnVisible;
		public bool BtnVisible
		{
			get => btnVisible;
			set => SetProperty(ref btnVisible, value, "BtnVisible");
		}

		private bool btnCancelVisible;
		public bool BtnCancelVisible
		{
			get => btnCancelVisible;
			set => SetProperty(ref btnCancelVisible, value, "BtnCancelVisible");
		}

		private string btnOkText;
		public string BtnOkText
		{
			get => btnOkText;
			set => SetProperty(ref btnOkText, value, "BtnOkText");
		}

		private string btnCancelText;
		public string BtnCancelText
		{
			get => btnCancelText;
			set => SetProperty(ref btnCancelText, value, "BtnCancelText");
		}

		private Color themeColor;
		public Color ThemeColor
		{
			get => themeColor;
			set => SetProperty(ref themeColor, value, "ThemeColor");
		}

		BaseVM ContentVM { get; }

		public ModalWindowVM(BaseVM contentVM, string caption, ModalButtons buttonType, string btnOkText, string btnCancelText, Color themeColor)
		{
			WindowTitle = caption;
			BtnVisible = buttonType != ModalButtons.None;
			BtnCancelVisible = buttonType == ModalButtons.OkCancel;
			BtnOkText = btnOkText;
			BtnCancelText = btnCancelText;
			ThemeColor = themeColor;

			ContentVM = contentVM;
		}

		protected override void ViewNavigatorInitialization()
		{
			ViewNavigator.Regions[this, "ModalContentView"].NavigateTo(ContentVM);
		}
	}
}