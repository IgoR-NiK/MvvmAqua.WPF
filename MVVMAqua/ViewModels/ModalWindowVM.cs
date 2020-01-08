using System;
using System.Windows.Media;

using MVVMAqua.Commands;
using MVVMAqua.Enums;
using MVVMAqua.Navigation.Interfaces;

namespace MVVMAqua.ViewModels
{
	internal class ModalWindowVM : BaseVM, IDialogClosing
	{
		private bool _btnVisible;
		public bool BtnVisible
		{
			get => _btnVisible;
			set => SetProperty(ref _btnVisible, value);
		}

		private bool _btnCancelVisible;
		public bool BtnCancelVisible
		{
			get => _btnCancelVisible;
			set => SetProperty(ref _btnCancelVisible, value);
		}

		private string _btnOkText = String.Empty;
		public string BtnOkText
		{
			get => _btnOkText;
			set => SetProperty(ref _btnOkText, value);
		}

		private string _btnCancelText = String.Empty;
		public string BtnCancelText
		{
			get => _btnCancelText;
			set => SetProperty(ref _btnCancelText, value);
		}

		private Color _themeColor;
        public Color ThemeColor
		{
			get => _themeColor;
			set => SetProperty(ref _themeColor, value);
		}

        private BaseVM ContentVM { get; }
        private Action<BaseVM> Initialization { get; }

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

        public event CloseDialogEventHandler? CloseDialog;

        private void OnCloseDialog(bool dialogResult)
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(dialogResult));
        }
    }
}