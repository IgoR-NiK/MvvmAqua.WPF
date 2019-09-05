using System;
using System.Windows.Media;

using MVVMAqua.Commands;
using MVVMAqua.Enums;
using MVVMAqua.Navigation.Interfaces;

namespace MVVMAqua.ViewModels
{
	class ModalWindowVM : BaseVM, IDialogClosing
	{
		private bool btnVisible;
		public bool BtnVisible
		{
			get => btnVisible;
			set => SetProperty(ref btnVisible, value);
		}

		private bool btnCancelVisible;
		public bool BtnCancelVisible
		{
			get => btnCancelVisible;
			set => SetProperty(ref btnCancelVisible, value);
		}

		private string btnOkText;
		public string BtnOkText
		{
			get => btnOkText;
			set => SetProperty(ref btnOkText, value);
		}

		private string btnCancelText;
		public string BtnCancelText
		{
			get => btnCancelText;
			set => SetProperty(ref btnCancelText, value);
		}

		private Color themeColor;
        public Color ThemeColor
		{
			get => themeColor;
			set => SetProperty(ref themeColor, value);
		}

        BaseVM ContentVM { get; }
        Action<BaseVM> Initialization { get; }

        public RelayCommand BtnOkCommand { get; }
        public RelayCommand CloseCommand { get; }

        public ModalWindowVM(BaseVM contentVM, Action<BaseVM> initialization, string caption, ModalButtons buttonType, string btnOkText, string btnCancelText, Color themeColor)
		{
			WindowTitle = caption;
			BtnVisible = buttonType != ModalButtons.None;
			BtnCancelVisible = buttonType == ModalButtons.OkCancel;
			BtnOkText = btnOkText;
			BtnCancelText = btnCancelText;
			ThemeColor = themeColor;

			ContentVM = contentVM;
            Initialization = initialization;

            BtnOkCommand = new RelayCommand(() => OnCloseDialog(true));
            CloseCommand = new RelayCommand(() => OnCloseDialog(false));
		}

		protected override void ViewNavigatorInitialization()
		{
			ViewNavigator.Regions[this, "ModalContentView"].NavigateTo(ContentVM, Initialization);
		}

        public event CloseDialogEventHandler CloseDialog;

        private void OnCloseDialog(bool dialogResult)
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(dialogResult));
        }
    }
}