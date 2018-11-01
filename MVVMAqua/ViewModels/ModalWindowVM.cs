using MvvmLibrary.Enums;

namespace MvvmAqua.ViewModels
{
	class ModalWindowVM : BaseVM
	{
		private bool btnVisible;
		public bool BtnVisible
		{
			get => btnVisible;
			set { btnVisible = value; OnPropertyChanged(); }
		}

		private bool btnCancelVisible;
		public bool BtnCancelVisible
		{
			get => btnCancelVisible;
			set { btnCancelVisible = value; OnPropertyChanged(); }
		}

		private string btnOkText;
		public string BtnOkText
		{
			get => btnOkText;
			set { btnOkText = value; OnPropertyChanged(); }
		}

		private string btnCancelText;
		public string BtnCancelText
		{
			get => btnCancelText;
			set { btnCancelText = value; OnPropertyChanged(); }
		}

		BaseVM ContentVM { get; }

		public ModalWindowVM(BaseVM contentVM, string caption, ModalButtons buttonType, string btnOkText, string btnCancelText)
		{
			WindowTitle = caption;
			BtnVisible = buttonType != ModalButtons.None;
			BtnCancelVisible = buttonType == ModalButtons.OkCancel;
			BtnOkText = btnOkText;
			BtnCancelText = btnCancelText;

			ContentVM = contentVM;
		}

		protected internal override void ViewNavigatorInitialization()
		{
			ViewNavigator.Regions[this, "ModalContentView"].NavigateTo(ContentVM);
		}
	}
}